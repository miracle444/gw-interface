#region

using GuildWarsInterface.Networking.Servers.Base;

#endregion

namespace GuildWarsInterface.Networking.Servers
{
        internal class FileServer : Server
        {
                internal const short PORT = 5112;

                protected override short Port
                {
                        get { return PORT; }
                }

                protected override void Received(byte[] data)
                {
                        Send(new byte[]
                                {
                                        0xF1, 0x02, 0x20, 0x00, 0x6F, 0xC0, 0x05, 0x00,
                                        0x7E, 0xC9, 0x05, 0x00, 0x9C, 0x43, 0x05, 0x00,
                                        0x8C, 0xC9, 0x05, 0x00, 0x8D, 0xC9, 0x05, 0x00,
                                        0x8E, 0xC9, 0x05, 0x00, 0x8B, 0xC9, 0x05, 0x00
                                });
                }
        }
}