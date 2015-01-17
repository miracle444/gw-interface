#region

using System;
using System.Text;
using GuildWarsInterface.Debugging;

#endregion

namespace GuildWarsInterface.Declarations
{
        internal class HString
        {
                private readonly char[] _serializedValue;

                public HString(char[] hash)
                {
                        _serializedValue = hash;
                }

                public HString(string name)
                {
                        var tmp = new StringBuilder();
                        tmp.Append(BitConverter.ToChar(new byte[] {0x08, 0x01}, 0));
                        tmp.Append(BitConverter.ToChar(new byte[] {0x07, 0x01}, 0));
                        tmp.Append(name);
                        tmp.Append(BitConverter.ToChar(new byte[] {0x01, 0x00}, 0));
                        _serializedValue = tmp.ToString().ToCharArray();
                }

                public HString(byte[] stream)
                {
                        if (stream.Length % 2 != 0)
                        {
                                Debug.ThrowException(new Exception("stream length must be even"));
                        }

                        _serializedValue = new char[stream.Length / 2];

                        for (int i = 0; i < stream.Length / 2; i++)
                        {
                                _serializedValue[i] = BitConverter.ToChar(stream, i * 2);
                        }
                }

                public char[] Serialize()
                {
                        return _serializedValue;
                }
        }
}