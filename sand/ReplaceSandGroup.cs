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

        public ReplaceSandGroup(): this(new(0), new(1), new(2), new(3))
        {
        }

        public ReplaceSandGroup(ReplaceOperation tl, ReplaceOperation tr, ReplaceOperation bl, ReplaceOperation br)
        {
            this.TopLeft = tl;
            this.TopRight = tr;
            this.BottomLeft = bl;
            this.BottomRight = br;
        }


        /// <summary>
        /// apply this operation.
        /// 
        /// DO NOT INLINE THIS, SEEMS TO SLOW DOWN PROGRAM?
        /// </summary>
        /// <param name="src">source sand group</param>
        public readonly void Apply(ref SrcSandGroup src)
        {
            SandPixel newTopLeft = this.TopLeft.Apply(ref src);
            SandPixel newTopRight = this.TopRight.Apply(ref src);
            SandPixel newBottomLeft = this.BottomLeft.Apply(ref src);
            SandPixel newBottomRight = this.BottomRight.Apply(ref src);
            src.TopLeft = newTopLeft;
            src.TopRight = newTopRight;
            src.BottomLeft = newBottomLeft;
            src.BottomRight = newBottomRight;
        }
    }
}