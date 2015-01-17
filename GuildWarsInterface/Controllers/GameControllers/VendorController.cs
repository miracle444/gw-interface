#region

using System.Collections.Generic;
using GuildWarsInterface.Controllers.Base;

#endregion

namespace GuildWarsInterface.Controllers.GameControllers
{
        internal class VendorController : IController
        {
                public void Register(IControllerManager controllerManager)
                {
                        controllerManager.RegisterHandler(71, BuyItemHandler);
                }

                private void BuyItemHandler(List<object> objects)
                {
                }
        }
}