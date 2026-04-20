using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HW5_GROUP_PROJECT.sand
{
    public class SandGridComponent
    {
        private SandGrid grid;
        private Texture2D? tex;
        private LookupTable fallingSandTable;
        private LookupTable looseSandTable;
        private byte updateCount = 0;

        /// <summary>
        /// the sand grid component
        /// </summary>
        /// <param name="dev">device to init sand tile texture on.</param>
        public SandGridComponent(GraphicsDevice? dev)
        {
            if (dev != null){
                this.tex = new Texture2D(dev, 1, 1);
                this.tex.SetData([Color.White]);
            }
            this.grid = new();
            this.Init();
        }

        /// <summary>
        /// initializes the falling sand LUT
        /// </summary>
        private void Init()
        {
            this.fallingSandTable = new(3);
            this.fallingSandTable.Build((tl, tr, bl, br) =>
            {
                bool bl_available = bl == 0;
                bool br_available = br == 0;
                ReplaceSandGroup group = new();
                if (bl_available && tl == 1)
                {
                    bl_available = false;
                    (group.TopLeft, group.BottomLeft) = (group.BottomLeft, group.TopLeft);
                }
                if (br_available && tr == 1)
                {
                    br_available = false;
                    (group.TopRight, group.BottomRight) = (group.BottomRight, group.TopRight);
                }
                if (br_available && tl == 1)
                {
                    br_available = false;
                    (group.TopLeft, group.BottomRight) = (group.BottomRight, group.TopLeft);
                }
                if (bl_available && tr == 1)
                {
                    bl_available = false;
                    (group.TopRight, group.BottomLeft) = (group.BottomLeft, group.TopRight);
                }
                return group;
            });
            this.looseSandTable = new(3);
            this.looseSandTable.Build((tl, tr, bl, br) =>
            {
                ReplaceSandGroup group = new();
                //if top left is loose sand
                if (tl == 1)
                {
                    if(tr == 0)
                    {
                        group.TopLeft = new(0,PixelId.FALLING_SAND);
                    }
                }
                if (tr == 1)
                {
                    if(tl == 0)
                    {
                        group.TopRight = new(1,PixelId.FALLING_SAND);
                    }
                }

                if (bl == 1)
                {
                    if(tl == 0 || br == 0)
                    {
                        group.BottomLeft = new(2, PixelId.FALLING_SAND);
                    }
                }
                if (br == 1)
                {
                    if(tr == 0 || bl == 0)
                    {
                        group.BottomRight = new(3,PixelId.FALLING_SAND);
                    }
                }
                return group;
            });
        }

        public void Draw(SpriteBatch batch, Camera camera)
        {
            Vector2 topLeft = camera.TopLeftWorldSpace;
            Vector2 bottomRight = camera.BottomRightWorldSpace;
            Vector2 drawBottomRight = new(
                bottomRight.X + 1, 
                bottomRight.Y + 1);
            Vector2 drawTopLeft = new(
                topLeft.X, 
                topLeft.Y);
    
            Point p = camera.Zoom.ToPoint();
            this.grid.Draw((x,y,pixel) =>
            {
                if (pixel.id == PixelId.FALLING_SAND || pixel.id == PixelId.SAND)
                {
                    Point tileTopLeft = camera.FromWorldSpace(new(x,y)).ToPoint();
                    batch.Draw(this.tex, new Rectangle(tileTopLeft, p), pixel.GetColor());
                }
            }, 
                drawTopLeft.ToPoint(),
                drawBottomRight.ToPoint()
            );
        }

        /// <summary>
        /// updates the state of the current board
        /// </summary>
        public void Update()
        {
            grid.Update(
                [this.looseSandTable, this.fallingSandTable],
                [[2,2,0,1], [2,0,1,2]],
                this.updateCount
            );
            this.updateCount ++;
            this.updateCount %= 4;
        }

        /// <summary>
        /// method to check if a tile in the grid is a solid tile.
        /// </summary>
        /// <param name="x">x coordinate to check</param>
        /// <param name="y">y coordinate to check</param>
        /// <returns>if the tile is solid</returns>
        public bool IsSolid(uint x, uint y)
        {
            return this.grid.GetPixel(new(x,y)).id == PixelId.FALLING_SAND;
        }

        /// <summary>
        /// sets a pixel in the targeted positions
        /// </summary>
        /// <param name="x">x to set</param>
        /// <param name="y">y to set</param>
        /// <param name="id">id to set tile to.</param>
        public void SetPixel(uint x, uint y, PixelId id, Color color)
        {
            ref SandPixel pixel = ref this.grid.GetPixel(new(x,y));
            pixel.id = id;
            pixel.SetColor(color);
        }
    }
}