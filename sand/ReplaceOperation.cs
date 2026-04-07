using System.Runtime.InteropServices;

namespace HW5_GROUP_PROJECT.sand
{

    enum ReplaceOperationType: byte
    {
        Move = 0,
        Spawn = 1,
    }

    /// <summary>
    /// imitation rust enum in order to try to cram as much data into a struct
    /// without any null nonesense.
    /// 
    /// basically, this struct should be 5 bytes,
    /// where the interpretation will function as:
    /// byte:   1    2    3    4    5
    ///       [    Sand pixel    ] [Spawn]
    ///       [src] [            ] [Move]
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
    struct ReplaceOperation
    {
        [FieldOffset(4)] public ReplaceOperationType type;
        //use if struct is a spawn operation
        [FieldOffset(0)] SandPixel pixel;
        //use if struct is a move operation
        [FieldOffset(0)] byte Source;

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

        public readonly SandPixel Apply(ref SrcSandGroup src)
        {
            if (this.type == ReplaceOperationType.Move){
                switch (Source)
                {
                    case 0:
                        return src.TopLeft;
                    case 1:
                        return src.TopRight;
                    case 2:
                        return src.BottomLeft;
                    default:
                        return src.BottomRight;
                }
            }
            return this.pixel;
        }
    }
}