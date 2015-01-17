#region

using System;
using System.Net;
using System.Net.Sockets;

#endregion

namespace GuildWarsInterface.Networking.Base
{
        public sealed class Listener
        {
                private const int BACKLOG_SIZE = 100;

                private readonly Socket _socket;

                public Listener()
                {
                        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }

                public event Action<Connection> ConnectionAccepted;

                public bool Listen(IPEndPoint endPoint)
                {
                        try
                        {
                                _socket.Bind(endPoint);

                                _socket.Listen(BACKLOG_SIZE);

                                _socket.BeginAccept(AcceptCallback, null);

                                return true;
                        }
                        catch (Exception)
                        {
                                return false;
                        }
                }

                private void AcceptCallback(IAsyncResult asyncResult)
                {
                        var connection = new Connection(_socket.EndAccept(asyncResult));

                        if (ConnectionAccepted != null)
                        {
                                ConnectionAccepted(connection);
                        }

                        _socket.BeginAccept(AcceptCallback, null);
                }
        }
}