
using System;
using Godot;

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
        {}
#endif
    }
}