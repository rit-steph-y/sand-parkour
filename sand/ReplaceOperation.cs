using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace HW5_GROUP_PROJECT.sand
{

    public enum ReplaceOperationType: byte
    {
        Move = 0,
        Spawn = 1,
    }

    /// <summary>
    /// imitation rust enum in order to try to cram as much data into a struct
    /// without any null nonesense.
    /// 
    /// basically, this struct should be 5 bytes,
    /// 
    /// the changed flag is a boolean on the move command.
    /// this simplifies the check for if the move operation
    /// changed anything about the cell or if we can count
    /// this area as unchanged.
    /// where the interpretation will function as:
    /// byte:   1    2    3    4    5
    ///       [    Sand pixel    ] [Spawn]
    ///       [src][chg][        ] [Move]
    ///      ///keep operation is the same as move but with original coordinates.
    /// 
    /// effectively a union type with a byte at the end that determines how
    /// this union should be interpreted, I hope this is easier to understand
    /// than me just using an enum in rust and improves the readability of my
    /// code to my teammates.
    /// 
    /// sorry if it doesn't, I tried... みんなさん、ごめんなさい。
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct ReplaceOperation
    {
        [FieldOffset(4)] public ReplaceOperationType type;
        //use if struct is a spawn operation
        [FieldOffset(0)] SandPixel pixel;
        //use if struct is a move operation
        [FieldOffset(0)] byte Source;
        [FieldOffset(1)] bool changed;

        public ReplaceOperation(byte Source)
        {
            this.Source = Source;
            this.type = ReplaceOperationType.Move;
        }

        public ReplaceOperation(SandPixel pixel)
        {
            this.pixel = pixel;
            this.type = ReplaceOperationType.Spawn;
        }
        
        /// <summary>
        /// applies the replace operation to the target sand pixel.
        /// 
        /// note: aggressive inlining on this saved about .3ms which is
        /// pretty good, I think we have gotten close enough to the speed
        /// of light.
        /// </summary>
        /// <param name="src">source sand group</param>
        /// <returns>new pixel generated.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly SandPixel Apply(ref SrcSandGroup src, out bool changed)
        {
            if (this.type == ReplaceOperationType.Move){
                changed = this.changed;
                return Source switch
                {
                    0 => src.TopLeft,
                    1 => src.TopRight,
                    2 => src.BottomLeft,
                    _ => src.BottomRight,
                };
            }
            changed = true;
            return this.pixel;
        }

        public void UpdateIsChanged(ReplaceOperation other)
        {
            this.changed = !this.SrcEquals(other);
        }

        private readonly bool SrcEquals(ReplaceOperation other)
        {
            return  (this.type == ReplaceOperationType.Move) && 
                    (this.type == other.type) && 
                    (this.Source == other.Source);
        }
    }
}