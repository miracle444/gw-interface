#region

using System;
using System.Runtime.InteropServices;
using GuildWarsInterface.Misc;

#endregion

namespace GuildWarsInterface.Datastructures.Agents.Components
{
        internal sealed class AgentClientMemory
        {
                private readonly Agent _agent;
                private readonly IntPtr _agentBase = (IntPtr) 0x00D559B8;

                public AgentClientMemory(Agent agent)
                {
                        _agent = agent;
                }

                private IntPtr ClientMemoryBase
                {
                        get
                        {
                                try
                                {
                                        return Marshal.ReadIntPtr(Marshal.ReadIntPtr(_agentBase) + (int) (4 * IdManager.GetId(_agent)));
                                }
                                catch
                                {
                                        return IntPtr.Zero;
                                }
                        }
                }

                public float Speed
                {
                        get
                        {
                                float mx = MoveX;
                                float my = MoveY;

                                return (float) Math.Sqrt(mx * mx + my * my);
                        }
                }

                public short Plane
                {
                        get
                        {
                                try
                                {
                                        return Marshal.ReadInt16(ClientMemoryBase + 92);
                                }
                                catch
                                {
                                        return 0;
                                }
                        }
                }

                public float X
                {
                        get
                        {
                                try
                                {
                                        return BitConverter.ToSingle(BitConverter.GetBytes(Marshal.ReadInt32(ClientMemoryBase + 116)), 0);
                                }
                                catch
                                {
                                        return 0;
                                }
                        }
                }

                public float Y
                {
                        get
                        {
                                try
                                {
                                        return BitConverter.ToSingle(BitConverter.GetBytes(Marshal.ReadInt32(ClientMemoryBase + 120)), 0);
                                }
                                catch
                                {
                                        return 0;
                                }
                        }
                }

                public float MoveX
                {
                        get
                        {
                                try
                                {
                                        return BitConverter.ToSingle(BitConverter.GetBytes(Marshal.ReadInt32(ClientMemoryBase + 160)), 0);
                                }
                                catch
                                {
                                        return 0;
                                }
                        }
                }

                public float MoveY
                {
                        get
                        {
                                try
                                {
                                        return BitConverter.ToSingle(BitConverter.GetBytes(Marshal.ReadInt32(ClientMemoryBase + 164)), 0);
                                }
                                catch
                                {
                                        return 0;
                                }
                        }
                }

                public Position Position
                {
                        get { return new Position(X, Y, Plane); }
                }
        }
}