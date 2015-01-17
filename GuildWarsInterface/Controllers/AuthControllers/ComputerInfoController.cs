#region

using System.Collections.Generic;
using GuildWarsInterface.Controllers.Base;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

#endregion

namespace GuildWarsInterface.Controllers.AuthControllers
{
        internal class ComputerInfoController : IController
        {
                public void Register(IControllerManager controllerManager)
                {
                        controllerManager.RegisterHandler(2, ComputerInfoHandler);
                }

                private void ComputerInfoHandler(List<object> enumerable)
                {
                        Network.AuthServer.Send(AuthServerMessage.ComputerInfoReply, 0, 0, 1, 1);
                }
        }
}