using System.Runtime.CompilerServices;

namespace HW5_GROUP_PROJECT.sand
{
    /// <summary>
    /// complete madness.
    /// 
    /// using effectively a union to try to save on memory cost.
    /// why.
    /// 
    /// it's not even a c# feature
    /// 
    /// then cram it into the 4 group that we will use it in.
    /// seriously
    /// 
    /// normal ideas.
    /// </summary>
    public struct ReplaceSandGroup
    {
        public ReplaceOperation TopLeft;
        public ReplaceOperation TopRight;
        public ReplaceOperation BottomLeft;
        public ReplaceOperation BottomRight;

        public static readonly ReplaceOperation GetTopLeft = new(0);
        public static readonly ReplaceOperation GetTopRight = new(1);
        public static readonly ReplaceOperation GetBottomLeft = new(2);
        public static readonly ReplaceOperation GetBottomRight = new(3);

        public ReplaceSandGroup(): this(GetTopLeft, GetTopRight, GetBottomLeft, GetBottomRight)
        {
        }

        public ReplaceSandGroup(ReplaceOperation tl, ReplaceOperation tr, ReplaceOperation bl, ReplaceOperation br)
        {
            this.TopLeft = tl;
            this.TopRight = tr;
            this.BottomLeft = bl;
            this.BottomRight = br;
        }

        public void UpdateIsChanged()
        {
            this.TopLeft.UpdateIsChanged(GetTopLeft);
            this.TopRight.UpdateIsChanged(GetTopRight);
            this.BottomLeft.UpdateIsChanged(GetBottomLeft);
            this.BottomRight.UpdateIsChanged(GetBottomRight);
        }

        /// <summary>
        /// apply this operation.
        /// 
        /// DO NOT INLINE THIS, SEEMS TO SLOW DOWN PROGRAM?
        /// </summary>
        /// <param name="src">source sand group</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ChangeResult Apply(ref SrcSandGroup src)
        {
            ChangeResult res = new();
            SandPixel newTopLeft = this.TopLeft.Apply(ref src, out bool tlchange);
            SandPixel newTopRight = this.TopRight.Apply(ref src, out bool trchange);
            SandPixel newBottomLeft = this.BottomLeft.Apply(ref src, out bool blchange);
            SandPixel newBottomRight = this.BottomRight.Apply(ref src, out bool brchange);
            src.TopLeft = newTopLeft;
            src.TopRight = newTopRight;
            src.BottomLeft = newBottomLeft;
            src.BottomRight = newBottomRight;
            return res;
        }
    }

    public struct ChangeResult
    {
        byte res;
        public byte Min
        {
            get => (byte)(this.res & 0b11);
            set => this.res |= value;
        }
        public byte Max
        {
            get => (byte)(this.res>>4 & 0b11);
            set => this.res |= (byte)(value<<4);
        }
        public bool Changed
        {
            get => (this.res & 0b1000_0000) != 0;
            set {
                this.res &= 0b0111_1111;
                this.res |= (byte)(value? 0b1000_0000: 0);
            }
        }
    }
}