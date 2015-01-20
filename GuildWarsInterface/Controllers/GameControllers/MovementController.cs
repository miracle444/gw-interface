#region

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GuildWarsInterface.Controllers.Base;
using GuildWarsInterface.Datastructures.Agents.Components;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Logic;

#endregion

namespace GuildWarsInterface.Controllers.GameControllers
{
        internal class MovementController : IController
        {
                public void Register(IControllerManager controllerManager)
                {
                        controllerManager.RegisterHandler(55, KeyboardMoveHandler);
                        controllerManager.RegisterHandler(56, MouseMoveHandler);
                        controllerManager.RegisterHandler(65, KeyboardStopMovingHandler);
                }

                private void KeyboardMoveHandler(List<object> objects)
                {
                        AgentTransformation.MovementType = (MovementType)(uint)objects[4];
                }

                private void MouseMoveHandler(List<object> objects)
                {
                        AgentTransformation.MovementType = MovementType.Forward;
                }

                private void KeyboardStopMovingHandler(List<object> objects)
                {
                        AgentTransformation.MovementType = MovementType.Stop;
                }
        }
}