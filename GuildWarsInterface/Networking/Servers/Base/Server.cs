#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using GuildWarsInterface.Controllers.Base;
using GuildWarsInterface.Networking.Base;

#endregion

namespace GuildWarsInterface.Networking.Servers.Base
{
        internal abstract class Server : IControllerManager
        {
                private readonly Dictionary<int, Action<List<object>>> _controllers = new Dictionary<int, Action<List<object>>>();
                private Connection _connection;
                private Listener _listener;
                public Protocol.Protocol Protocol { get; set; }

                protected abstract short Port { get; }

                public void RegisterHandler(int messageId, Action<List<object>> handler)
                {
                        _controllers.Add(messageId, handler);
                }

                protected virtual void Received(byte[] data)
                {
                        try
                        {
                                List<object> packet;
                                while (Protocol.Deserialize(data, out packet, out data))
                                {
                                        Console.WriteLine(GetType().Name + ": " + packet[0]);

                                        if (_controllers.ContainsKey((int) packet[0]))
                                        {
                                                _controllers[(int) packet[0]](packet);
                                        }

                                        if (data.Length == 0) break;
                                }
                        }
                        catch (Exception e)
                        {
                                // TODO: auth 0 seems to cause problems
                        }
                }

                public void Start()
                {
                        _listener = new Listener();
                        _listener.Listen(new IPEndPoint(IPAddress.Any, Port));
                        _listener.ConnectionAccepted += connection =>
                                {
                                        _connection = connection;
                                        _connection.Received += Received;
                                        connection.BeginReceiving();
                                };
                }

                protected void Send(int messageId, params object[] parameters)
                {
                        using (var stream = new MemoryStream())
                        using (var writer = new BinaryWriter(stream))
                        {
                                writer.Write((ushort) messageId);

                                for (int i = 0; i < parameters.Length; i++)
                                {
                                        dynamic value = parameters[i];

                                        if (value is string) value = value.ToCharArray();

                                        if (value is Array)
                                        {
                                                switch (Protocol.PrefixSize(messageId, i + 1))
                                                {
                                                        case 1:
                                                                writer.Write((byte) value.Length);
                                                                break;
                                                        case 2:
                                                                writer.Write((ushort) value.Length);
                                                                break;
                                                }

                                                foreach (dynamic element in value)
                                                {
                                                        if (element is char)
                                                        {
                                                                writer.Write((ushort) element);
                                                        }
                                                        else
                                                        {
                                                                writer.Write(element);
                                                        }
                                                }
                                        }
                                        else
                                        {
                                                writer.Write(value);
                                        }
                                }

                                Send(stream.ToArray());
                        }
                }

                protected void Send(byte[] data)
                {
                        _connection.Send(data);
                }

                public void Disconnect()
                {
                        _connection.Disconnect();
                }
        }
}