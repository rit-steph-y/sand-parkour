using HW5_GROUP_PROJECT.sand;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HW5_GROUP_PROJECT
{
    internal class Player
    {
        private static Texture2D? cachedPlayerTexture;
        private uint myX => (uint)myPosition.X;
        private uint myY => (uint)myPosition.Y;

        private Vector2 myPosition;
        private Vector2 myBottomRight => myPosition + new Vector2(myWidth, myHeight);

        private int myHeight;
        private int myWidth;

        private Texture2D myTexture;
        private Vector2 myVelocity;
        private Vector2 myGravity = new Vector2(0,0.05f);
        private Vector2 myFriction = new Vector2(1,0);
        internal Player(Vector2 position, Game game)
        {
            if(cachedPlayerTexture == null)
            {
                cachedPlayerTexture = game.Content.Load<Texture2D>("sandPlayerSprite");
            }
            this.myTexture = cachedPlayerTexture;
            this.myPosition = position;

            this.myWidth = this.myTexture.Width;
            this.myHeight = this.myTexture.Height;
        }

        internal void Draw(SpriteBatch sprite, Camera camera)
        {
            sprite.Draw(myTexture, camera.FromWorldSpaceRect(this.myPosition, this.myBottomRight), Color.White);
        }

        internal void Update(KeyboardState state, GameTime time, SandGridComponent grid)
        {
            
            myVelocity += myGravity - myFriction * myVelocity;
            if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up)) {
                if (myPosition.Y + myHeight >= 600)
                {
                    myVelocity.Y = -4;
                }
               
            }

            if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left))
            {
                myVelocity.X = -3;
            }

            if (state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down))
            {
                myVelocity.Y = + 3;
            }

            if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right))
            {
                myVelocity.X =  + 3;
            }
            this.GetPlayerPosistionVector(grid);


        }

        private void GetPlayerPosistionVector(SandGridComponent grid) 
        {
            myPosition += this.myVelocity;
            if (grid.IsSolid(this.myX, this.myY, (uint)(this.myX + this.myWidth), (uint)(this.myY + this.myHeight)))
            {
                myVelocity.Y = 0;
                myPosition.Y = 600 - myHeight;
            }
            
        }

        // Player with movement, collisions not implemented yet.
        // the following methods are for convience
        // and because getting the corners of a sprite always annoys me.
        // and we might need them. - AJ

        internal Vector2 GetTopLeftCorner()
        {
            Vector2 temp = new Vector2(this.myX, this.myY);
            return temp;
        }
        internal Vector2 GetTopRightCorner()
        {
            Vector2 temp = new Vector2(this.myX + this.myWidth, this.myY);
            return temp;
        }

        internal Vector2 GetBottomLeftCorner()
        {
            Vector2 temp = new Vector2(this.myX, this.myY + this.myHeight);
            return temp;
        }

        internal Vector2 GetBottomRightCorner()
        {
            Vector2 temp = new Vector2(this.myX + this.myWidth, this.myY + this.myHeight);
            return temp;
        }
    }
}
