#region

using System;
using System.Collections.Generic;

#endregion

namespace GuildWarsInterface.Controllers.Base
{
        internal interface IControllerManager
        {
                void RegisterHandler(int messageId, Action<List<object>> handler);
        }
}