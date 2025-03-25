using System;

namespace Riftstrike
{
        internal static class Debug
        {
                internal static void Assert(bool condition, string message)
#if DEBUG
                {
                        if (condition) return;
                        GD.PrintErr(message);
                        throw new ApplicationException($"Assert failed: {message}");
                }
#else
        { } // does not effect binary in release mode
#endif

                internal static void Print(string what)
#if DEBUG
                => GD.Print(what);
#else
        { } // does not effect binary in release mode
#endif

                internal static void Print(params object[] what)
#if DEBUG
                => GD.Print(what);
#else
        { } // does not effect binary in release mode
#endif
        }
}