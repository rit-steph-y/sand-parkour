using System;

namespace HW5_GROUP_PROJECT.sand{
    /**
    z index struct to store 2D coordinates as inherently interleaved bits.
    */
    public struct ZOrderIndex
    {
        ulong index;

        public ZOrderIndex(ulong i)
        {
            this.index = i;
        }
        public ZOrderIndex(uint x, uint y)
        {
            this.index = 
                System.Runtime.Intrinsics.X86.Bmi2.X64.ParallelBitDeposit(x, X_BITS) |  
                System.Runtime.Intrinsics.X86.Bmi2.X64.ParallelBitDeposit(y, Y_BITS); 
        }

        public const ulong X_BITS = 0x5555_5555_5555_5555;

        public const ulong Y_BITS = 0xaaaa_aaaa_aaaa_aaaa;
        public static implicit operator ulong(ZOrderIndex index) => index.index;
        public static implicit operator ZOrderIndex(ulong u) => new(u);

        public static ZOrderIndex operator <<(ZOrderIndex index, int amount) => new((ulong)index << amount);
        public static ZOrderIndex operator >>(ZOrderIndex index, int amount) => new((ulong)index >> amount);
        
        public readonly uint Y => (uint)System.Runtime.Intrinsics.X86.Bmi2.X64.ParallelBitExtract(this.index, Y_BITS);
        public readonly uint X => (uint)System.Runtime.Intrinsics.X86.Bmi2.X64.ParallelBitExtract(this.index, X_BITS);

        public readonly ZOrderIndex XBits()
        {
            return this.index & X_BITS;
        }

        public readonly ZOrderIndex YBits()
        {
            return this.index & Y_BITS;
        }

        public readonly ZOrderIndex PerElementMin(ZOrderIndex other)
        {
            return 
                ulong.Min(this.XBits(), other.XBits()) | 
                ulong.Min(this.YBits(), other.YBits());
        }
        public readonly ZOrderIndex PerElementMax(ZOrderIndex other)
        {
            return 
                ulong.Max(this.XBits(), other.XBits()) | 
                ulong.Max(this.YBits(), other.YBits());
        }

        public readonly ZOrderIndex XBitsPlus1()
        {
            return ((this.index | Y_BITS) + 1) & X_BITS;
        }

        public readonly ZOrderIndex YBitsPlus1()
        {
            return ((this.index | X_BITS) + 1) & Y_BITS;            
        }

        /// <summary>
        /// this method does not check if the bits you are setting
        /// are only Y! attempting to set also X bits will result in
        /// undefined behavior
        /// </summary>
        /// <param name="bits">the Y bits to set</param>
        public void SetYBits(ulong bits)
        {
            this.index &= X_BITS;
            this.index |= bits;
        }

        /// <summary>
        /// this method does not check if the bits you are setting
        /// are only X! attempting to set also y bits will result in
        /// undefined behavior
        /// </summary>
        /// <param name="bits">the X bits to set</param>
        public void SetXBits(ulong bits)
        {
            this.index &= Y_BITS;
            this.index |= bits;
        }
    }
}