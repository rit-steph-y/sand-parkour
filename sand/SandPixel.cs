using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace HW5_GROUP_PROJECT.sand
{

    enum PixelId: byte
    {
        INVALID = 0,
        AIR = 1,
        SAND = 2
    }

    class PixelIdHandler
    {
        // public PixelVelocity UpdateVelocity(PixelId self, PixelVelocity velocity)
        // {
            
        // }
    }

    enum NeighborPos: byte
    {
        UP,
        ULEFT,
        URIGHT,
        DOWN,
        DLEFT,
        DRIGHT,
        LEFT,
        RIGHT,
    }
    // the 8 neighbors for a cell
    struct Neighbors
    {
        PixelId u;
        PixelId ur;
        PixelId r;
        PixelId dr;
        PixelId d;
        PixelId dl;
        PixelId l;
        PixelId ul;
    }

    struct SandPixel
    {
        PixelId id;
        SubpixelPosition subpixel;

        // public NeighborPos next(Neighbors neighbors)
        // {
        //     sbyte newX = this.subpixel.x;
        //     sbyte newY = this.subpixel.y;
        //     newX += this.velocity.x;
        //     newY += this.velocity.y;
            
        // }
    }

    public delegate void SandSystem(SandGrid grid) ;
    
    public struct SandGrid
    {
        private Dictionary<ZOrderIndex, SandPixel[]> chunks;
        public ref SandPixel Get(ZOrderIndex index)
        {
            return ref ;
        }
    }

    struct SubpixelPosition
    {
        const byte SubpixelBits = 0xF;
        const byte SubpixelCenter = 0b1000;
        byte position;
        public sbyte x => (sbyte)(position & SubpixelBits - SubpixelCenter);
        public sbyte y => (sbyte)((position >> 4) & SubpixelBits - SubpixelCenter);
    }


}