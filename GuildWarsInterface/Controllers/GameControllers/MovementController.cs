#region

using System.Collections.Generic;
using GuildWarsInterface.Controllers.Base;

#endregion

namespace GuildWarsInterface.Controllers.GameControllers
{
        internal class MovementController : IController
        {
                public void Register(IControllerManager controllerManager)
                {
                        controllerManager.RegisterHandler(55, KeyboardMoveHandler);
                        controllerManager.RegisterHandler(65, KeyboardStopMovingHandler);
                }

                private void KeyboardMoveHandler(List<object> objects)
                {
                }

                private void KeyboardStopMovingHandler(List<object> objects)
                {
                }
        }
}