#region

using System;
using System.Collections.Generic;
using GuildWarsInterface.Datastructures.Agents;
using GuildWarsInterface.Debugging;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

#endregion

namespace GuildWarsInterface.Datastructures.Misc
{
        public sealed class Dialog
        {
                private readonly List<DialogButton> _buttons = new List<DialogButton>();
                private readonly Creature _sender;
                private string _body;

                public Dialog(string body, Creature sender = null)
                {
                        _body = body;
                        _sender = sender;
                }

                public Dialog AddLine(string caption)
                {
                        _body = _body + "<brx>" + caption;

                        return this;
                }

                public Dialog AddDialogButton(string caption, uint buttonId)
                {
                        _body = _body + "<a=" + buttonId.ToString() + ">" + caption + "</a>";

                        return this;
                }

                public Dialog AddButton(DialogButtonIcon icon, string caption, uint buttonId, uint spell = 0xFFFFFFFF)
                {
                        _buttons.Add(new DialogButton(icon, caption, buttonId, spell));

                        return this;
                }

                public void Open()
                {
                        Debug.Requires(_sender == null || _sender.Created);

                        var remainder = new string(new HString(_body).Serialize());
                        int length = remainder.Length;

                        while (length > 0)
                        {
                                int stubLength = Math.Min(61, length);
                                Network.GameServer.Send(GameServerMessage.DialogBody, remainder.Substring(0, stubLength));
                                remainder = remainder.Substring(stubLength);
                                length = remainder.Length;
                        }

                        Network.GameServer.Send(GameServerMessage.DialogSender, (_sender != null ? IdManager.GetId(_sender) : 0));

                        foreach (DialogButton button in _buttons)
                        {
                                Network.GameServer.Send(GameServerMessage.DialogButton,
                                                        (byte) button.Icon,
                                                        new HString(button.Caption).Serialize(),
                                                        button.ButtonId,
                                                        button.Spell);
                        }
                }

                private class DialogButton
                {
                        public DialogButton(DialogButtonIcon icon, string caption, uint buttonId, uint spell = 0xFFFFFFFF)
                        {
                                Icon = icon;
                                Caption = caption;
                                ButtonId = buttonId;
                                Spell = spell;
                        }

                        public DialogButtonIcon Icon { get; private set; }
                        public string Caption { get; private set; }
                        public uint ButtonId { get; private set; }
                        public uint Spell { get; private set; }
                }
        }
}