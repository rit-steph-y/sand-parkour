using Microsoft.Xna.Framework;

namespace HW5_GROUP_PROJECT.sand
{
    public enum PixelId: byte
    {
        INVALID = 0,
        AIR = 1,
        SAND = 2
    }

    public struct SandPixel
    {
        public const uint COLOR_EXPAND_MASK = 0b0000_0000_1111_1000_1111_1100_1111_1000;
        public PixelId id;
        public byte flags;
        ushort color;

        public void SetColor(Color color)
        {
            this.color = (ushort)System.Runtime.Intrinsics.X86.Bmi2.ParallelBitExtract(color.PackedValue, COLOR_EXPAND_MASK);
        }
        public void SetColor(ushort color)
        {
            this.color = color;
        }

        public readonly Color GetColor()
        {
            uint packedValue = System.Runtime.Intrinsics.X86.Bmi2.ParallelBitDeposit(this.color, COLOR_EXPAND_MASK);
            packedValue |= 0xFF000000;
            return new(packedValue);
        }
    }
}