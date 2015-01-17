#region

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GuildWarsInterface.Datastructures.Agents;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

#endregion

namespace GuildWarsInterface.Interaction
{
        public static class Chat
        {
                public enum Channel
                {
                        All = '!',
                        Guild = '@',
                        Team = '#',
                        Trade = '$',
                        Ally = '%',
                        Whisper = '"',
                        Command = '/',
                        InternalCommand = '¨'
                }

                public enum Color
                {
                        /// <summary>
                        ///         Alliance Chat
                        /// </summary>
                        OrangeWhite,

                        /// <summary>
                        ///         Allied Party Chat
                        /// </summary>
                        BlueAllyWhite,

                        /// <summary>
                        ///         Broadcasts
                        /// </summary>
                        LightBlueLightBlue,

                        /// <summary>
                        ///         All Chat
                        /// </summary>
                        YellowWhite,

                        /// <summary>
                        ///         No Official Usage
                        /// </summary>
                        GrayDarkGray,

                        /// <summary>
                        ///         Gaile's Chat / turquoise
                        /// </summary>
                        TurquoiseTurquoise,

                        /// <summary>
                        ///         Emotes
                        /// </summary>
                        WhiteWhite,

                        /// <summary>
                        ///         No Official Usage
                        /// </summary>
                        InvisibleInvisible,

                        /// <summary>
                        ///         The Frog's Chat / turquoise
                        /// </summary>
                        GrayTurquoise,

                        /// <summary>
                        ///         Guild Chat
                        /// </summary>
                        GreenWhite,

                        /// <summary>
                        ///         Announcements
                        /// </summary>
                        LightGreenLightGreen,

                        /// <summary>
                        ///         Team Chat
                        /// </summary>
                        BlueTeamWhite,

                        /// <summary>
                        ///         Trade Chat
                        /// </summary>
                        LightPinkLightPink,

                        /// <summary>
                        ///         Command reply
                        /// </summary>
                        DarkOrangeDarkOrange,

                        /// <summary>
                        ///         NPC's Chat
                        /// </summary>
                        BlueNpcWhite
                }

                public static event InternalCommand InternalCommand;

                internal static void HandleInternalCommand(string commandWithArguments)
                {
                        List<string> arguments;
                        string command = ParseCommand(commandWithArguments, out arguments);
                        if (command != null && InternalCommand != null) InternalCommand(command, arguments);
                }

                private static string ParseCommand(string commandWithArguments, out List<string> arguments)
                {
                        if (commandWithArguments == null)
                        {
                                arguments = null;
                                return null;
                        }

                        arguments = new List<string>();

                        MatchCollection matches = Regex.Matches(commandWithArguments, "\\s*(\"[^\"]+\"|[^\\s\"]+)");

                        var queue = new Queue(matches);

                        if (queue.Count < 1) return null;

                        string command = queue.Dequeue().ToString();

                        while (queue.Count > 0)
                        {
                                string argument = queue.Dequeue().ToString();

                                arguments.Add(argument.Replace("\"", "").Trim(' '));
                        }

                        return command;
                }

                public static void ShowMessage(string message, Agent sender, Color color = Color.DarkOrangeDarkOrange)
                {
                        Network.GameServer.Send(GameServerMessage.Message, new HString(message).Serialize());
                        Network.GameServer.Send(GameServerMessage.MessageSender, (ushort) IdManager.GetId(sender), (byte) color);
                }

                public static void ShowMessage(string message, Color color = Color.DarkOrangeDarkOrange)
                {
                        Network.GameServer.Send(GameServerMessage.Message, new HString(message).Serialize());
                        Network.GameServer.Send(GameServerMessage.MessageSenderAnonymous, (ushort) 2, (byte) color);
                }

                public static void ShowMessage(string message, string sender, string tag, Color color = Color.DarkOrangeDarkOrange)
                {
                        Network.GameServer.Send(GameServerMessage.Message, new HString(message).Serialize());
                        Network.GameServer.Send(GameServerMessage.MessageSenderWithTag, (byte) color, sender, tag);
                }

                public static Color GetColorForChannel(Channel channel)
                {
                        switch (channel)
                        {
                                case Channel.Ally:
                                        return Color.OrangeWhite;
                                case Channel.Team:
                                        return Color.BlueTeamWhite;
                                case Channel.All:
                                        return Color.YellowWhite;
                                case Channel.Guild:
                                        return Color.GreenWhite;
                                case Channel.Trade:
                                        return Color.LightPinkLightPink;
                                default:
                                        return Color.DarkOrangeDarkOrange;
                        }
                }
        }

        public delegate void InternalCommand(string command, List<string> arguments);
}