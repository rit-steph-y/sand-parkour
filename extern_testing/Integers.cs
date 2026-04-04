using System;
using System.Runtime.InteropServices;

namespace HW5_GROUP_PROJECT.extern_testing{
    class Integers
    {
        [DllImport("falling_sand_lib", EntryPoint="lib_addition")]
        public static extern uint Addition(uint a, uint b);
    }
}