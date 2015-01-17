#region

using System;
using System.Runtime.InteropServices;
using GuildWarsInterface.Misc;

#endregion

namespace GuildWarsInterface.Datastructures.Agents.Components
{
        public sealed class AgentClientMemory
        {
                private readonly Agent _agent;
                private readonly IntPtr _agentBase = (IntPtr) 0x00D559E0;

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

                internal float ClientMemoryX
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

                internal float ClientMemoryY
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

                internal int ClientMemoryModelState
                {
                        get
                        {
                                try
                                {
                                        return Marshal.ReadInt32(ClientMemoryBase + 424);
                                }
                                catch
                                {
                                        return 0;
                                }
                        }
                }
        }
}