#region

using System;
using System.Net.Sockets;

#endregion

namespace GuildWarsInterface.Networking.Base
{
        public sealed class Connection
        {
                private readonly byte[] _receiveBuffer = new byte[8192];
                private readonly Socket _socket;

                public Connection(Socket socket)
                {
                        _socket = socket;
                }

                public event Action<byte[]> Received;

                public void BeginReceiving()
                {
                        _socket.BeginReceive(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, ReceiveCallback, null);
                }

                public void Send(byte[] data)
                {
                        try
                        {
                                _socket.Send(data);
                        }
                        catch (Exception)
                        {
                                Disconnect();
                        }
                }

                public void Disconnect()
                {
                        _socket.Close();
                }

                private void ReceiveCallback(IAsyncResult asyncResult)
                {
                        try
                        {
                                int bytesRead = _socket.EndReceive(asyncResult);

                                if (Received != null)
                                {
                                        Received(SubArray(_receiveBuffer, 0, bytesRead));
                                }

                                _socket.BeginReceive(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, ReceiveCallback,
                                                     null);
                        }
                        catch (Exception)
                        {
                                Disconnect();
                        }
                }

                private static byte[] SubArray(byte[] array, int start, int count)
                {
                        var result = new byte[count];
                        Array.Copy(array, start, result, 0, count);
                        return result;
                }
        }
}