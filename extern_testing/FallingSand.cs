using System;
using System.Runtime.InteropServices;

namespace HW5_GROUP_PROJECT.extern_testing{

    [StructLayout(LayoutKind.Sequential, Size = 8, Pack = 1)]
    struct ZIndex
    {
        ulong index;
    }
    class FallingSand: IDisposable
    {

        [StructLayout(LayoutKind.Sequential, Size = 8, Pack = 1)]
        private struct SandHandle
        {
        }

        [DllImport("falling_sand_lib", EntryPoint="new_falling_sand_handle")]
        private static extern SandHandle CreateHandle();

        [DllImport("falling_sand_lib", EntryPoint ="test_falling_sand_handle")]
        private static extern ushort Test(ref SandHandle handle, ZIndex index);

        [DllImport("falling_sand_lib", EntryPoint ="dispose_falling_sand_handle")]
        private static extern bool DisposeHandle(SandHandle handle);
        
        private SandHandle handle;

        public FallingSand()
        {
            ZIndex index = new ZIndex();
            handle = CreateHandle();
            Console.WriteLine($"{Test(ref handle, index)}");
            Console.WriteLine($"{Test(ref handle, index)}");
            Console.WriteLine($"{Test(ref handle, index)}");
            Console.WriteLine($"{Test(ref handle, index)}");
        }

        public void Dispose()
        {
            DisposeHandle(this.handle);
            this.handle = new();
        }
    }
}