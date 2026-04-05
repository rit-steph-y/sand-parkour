using System.Runtime.InteropServices;

namespace HW5_GROUP_PROJECT.extern_testing{
    [StructLayout(LayoutKind.Sequential, Size = 8, Pack = 1)]
    struct ZIndex
    {
        ulong index;
    }
}