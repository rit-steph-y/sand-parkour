using HW5_GROUP_PROJECT.sand;
using HW5_GROUP_PROJECT.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace HW5_GROUP_PROJECT
{
    internal class Player
    {
        private uint myX = 0;
        private uint myY = 0;

        private Vector2 myPosition;

        private int myHeight;
        private int myWidth;

        private Texture2D myTexture;
        internal Player(Vector2 position, Texture2D texture)
        {
            this.myPosition = position;
            this.myX = (uint)position.X;
            this.myY = (uint)position.Y;

            this.myTexture = texture;
            this.myWidth = texture.Width;
            this.myHeight = texture.Height;
        }

        internal void Draw(SpriteBatch sprite)
        {
            sprite.Draw(myTexture, myPosition, Color.White);
        }

        internal void Update(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up)) {
                this.myY = this.myY - 3;
            }

            if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left))
            {
                this.myX = this.myX - 3;
            }

            if (state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down))
            {
                this.myY = this.myY + 3;
            }

            if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right))
            {
                this.myX = this.myX + 3;
            }
            this.GetPlayerPosistionVector(myX, myY);
        }

        private void GetPlayerPosistionVector(uint x, uint y) 
        {
            myPosition = new Vector2(x, y);
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
