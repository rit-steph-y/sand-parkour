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
        public PixelId id;
        public byte flags;
        public short color;
    }
}