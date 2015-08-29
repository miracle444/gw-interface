using System.Collections.Generic;
using GuildWarsInterface.Controllers.Base;
using GuildWarsInterface.Datastructures.Agents;
using GuildWarsInterface.Interaction;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

namespace GuildWarsInterface.Controllers.GameControllers
{
        internal class AttackController : IController
        {
                public void Register(IControllerManager controllerManager)
                {
                        controllerManager.RegisterHandler(45, AttackHandler);
                }

                private void AttackHandler(List<object> objects)
                {
                        Creature target;
                        if (!IdManager.TryGet((uint) objects[1], out target)) return;

                        Chat.ShowMessage(string.Format("attacking {0}!", target));

                        Network.GameServer.Send((GameServerMessage) 42, IdManager.GetId(Game.Player.Character), 1.75F, 0xF);

                        /*Network.GameServer.Send(GameServerMessage.Projectile, 
                                IdManager.GetId(Game.Player.Character),
                                target.Transformation.ExplicitPosition[0],
                                target.Transformation.ExplicitPosition[1],
                                (ushort)0,
                                (float)1,
                                1,
                                1,
                                (byte)0);*/

                        /*Game.Player.Character.PerformAnimation(CreatureAnimation.None);
                        for (var i = 0; i < 10000; i++)
                        {
                                for (var j = 1; j < 5; j++)
                                        Network.GameServer.Send(GameServerMessage.Projectile,
                                                                IdManager.GetId(Game.Player.Character),
                                                                target.Transformation.ExplicitPosition[0],
                                                                target.Transformation.ExplicitPosition[1],
                                                                (ushort)0,
                                                                (float)j / 5F,
                                                                0x8F,
                                                                1,
                                                                (byte)0);
                                Thread.Sleep(1);
                        }*/
                }
        }
}