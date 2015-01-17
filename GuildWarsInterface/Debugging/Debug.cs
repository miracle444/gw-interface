#region

using System;

#endregion

namespace GuildWarsInterface.Debugging
{
        public static class Debug
        {
                public static Action<Exception> ThrowException = exception
                                                                 => { throw exception; };

                internal static void Requires(bool condition)
                {
                        if (!condition)
                        {
                                ThrowException(new Exception("precondition violated"));
                        }
                }
        }
}