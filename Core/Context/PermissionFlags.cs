using System;

namespace WillFrameworkPro.Core.Context
{
    [Flags]
    internal enum PermissionFlags : uint
    {
        _None = 0,
        View = 0b00000000000000000000000000000001,
        Service = View << 1,
        Model = View << 2, 
        Controller = View << 3,
        // --- command manager
        HighLevelCommandManager = View << 4,
        LowLevelCommandManager = View << 5,
        CommandManager = View << 6,
    }
}