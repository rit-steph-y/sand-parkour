using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace HW5_GROUP_PROJECT
{
    public struct Camera
    {
        public Vector2 Center {get; set;}
        public Vector2 Zoom {get; set;}
        public Vector2 ClientBounds {get; set;}
        public readonly Vector2 TopLeftWorldSpace => this.ToWorldSpace(Vector2.Zero);
        public readonly Vector2 BottomRightWorldSpace => this.ToWorldSpace(this.ClientBounds);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Vector2 FromWorldSpace(Vector2 vec)
        {
            return (vec - Center) * Zoom + ClientBounds * .5f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Rectangle FromWorldSpaceRect(Vector2 min, Vector2 max)
        {
            Point minPoint = this.FromWorldSpace(min).ToPoint();
            return new(minPoint, this.FromWorldSpace(max).ToPoint() - minPoint);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Vector2 ToWorldSpace(Vector2 vec)
        {
            return (vec - ClientBounds * .5f) / Zoom + Center;
        }

        public const float E = 2.718281828459045235360f;

        public void TweenTo(Vector2 target, float speed, float delta)
        {
	        float scaleFac = 1f-float.Pow(E,-delta * speed);
            Vector2 moveBy = (target - this.Center) * scaleFac;
            this.Center += moveBy;
        }
    }
}
