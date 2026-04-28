using System;
using System.Collections.Generic;
using HW5_GROUP_PROJECT.sand;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HW5_GROUP_PROJECT
{
    internal enum PlayerState
    {
        LookRight,
        LookLeft,
        WalkRight,
        WalkLeft,
        JumpRight,
        JumpLeft,
        FallRight,
        FallLeft
    }

    internal class Player
    {
        private float JumpVelocity = 230;
        private static Texture2D? cachedPlayerTexture;
        public Vector2 Center => Position + new Vector2(myWidth, myHeight) * .5f;
        internal Rectangle Rect => new Rectangle(this.PixelPosition, new Point(myWidth, myHeight));

        private Point PixelPosition
        {
            get
            {
                Point p = this.Position.ToPoint();
                if(float.IsNegative(this.Position.X))
                    p.X -= 1;
                if(float.IsNegative(this.Position.Y))
                    p.Y -= 1;
                return p;
            }
        }

        private PlayerState playerState;
        private Vector2 Position;
        private Vector2 myBottomRight => Position + new Vector2(myWidth, myHeight);

        private float jumpBufferTime = .15f;
        private float groundedBuffer = 0;
        private float jumpBuffer = 0;
        private bool Grounded = false; 
        private int myHeight;
        private int myWidth;

        private float WalkAccel = 1000f;
        private float friction = .2f;

        private Texture2D myTexture;
        private Vector2 velocity;
        private Vector2 gravity = new Vector2(0,400f);
        internal Player(Vector2 position, Game game)
        {
            if(cachedPlayerTexture == null)
            {
                // https://lucky-loops.itch.io/character-satyr?download Credit technically not required
                cachedPlayerTexture = game.Content.Load<Texture2D>("sandPlayerSprite");
            }
            this.myTexture = cachedPlayerTexture;
            this.Position = position;
            PlayerState state = PlayerState.LookRight;

            // I would manually set a width and height for the collision so it doesn't include the antlers
            // You would have to calculate some sort of offset when drawing the sprite so it draws in the right place though
            this.myWidth = this.myTexture.Width;
            this.myHeight = this.myTexture.Height;
        }

        internal void Draw(SpriteBatch sprite, Camera camera)
        {
            switch (playerState)
            {
                case PlayerState.LookRight:
                    sprite.Draw(myTexture, camera.FromWorldSpaceRect(this.Position, this.myBottomRight), Color.White);
                    break;
                case PlayerState.LookLeft:
                    break;
                case PlayerState.WalkRight:
                    break;
                case PlayerState.WalkLeft:
                    break;
                case PlayerState.JumpRight:
                    break;
                case PlayerState.JumpLeft:
                    break;
                case PlayerState.FallRight:
                    break;
                case PlayerState.FallLeft:
                    break;
            }
        }

        internal void Update(KeyboardState state, GameTime time, SandGridComponent grid)
        {
            float delta = (float)time.ElapsedGameTime.TotalSeconds;
            Vector2 acceleration = Vector2.Zero;
            acceleration += this.gravity;
            acceleration += -this.friction * this.velocity;
            acceleration += GetMoveForce(state);

            // if (state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down))
            // {
            //     if (this.IsColliding(grid) == false)
            //     {
            //         velocity.Y = +3;
            //     }
            // }
            acceleration = HandleJump(state, delta, acceleration);

            this.velocity += acceleration * delta;

            this.MoveAndSlide(grid, delta);

            this.ApplyFallingSand(grid);
        }

        private Vector2 HandleJump(KeyboardState state, float delta, Vector2 acceleration)
        {
            if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.Space))
            {
                this.jumpBuffer = this.jumpBufferTime;
            }

            if (this.groundedBuffer > 0 && this.jumpBuffer > 0)
            {
                this.groundedBuffer = 0;
                this.jumpBuffer = 0;
                acceleration.Y -= JumpVelocity / delta;
                acceleration.Y -= this.velocity.Y / delta;
            }

            this.groundedBuffer -= delta;
            this.jumpBuffer -= delta;
            return acceleration;
        }

        private Vector2 GetMoveForce(KeyboardState state)
        {
            Vector2 force = Vector2.Zero;
            if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left))
            {
                force -= Vector2.UnitX;
            }
            if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right))
            {
                force += Vector2.UnitX;
            }

            //reverse direction boost
            if (Vector2.Dot(force, this.velocity) < float.Epsilon)
            {
                force *= 2;
            }
            Vector2 xFriction = new Vector2(this.velocity.X, 0) * -4;
            return force * this.WalkAccel + xFriction;
        }

        private void MoveAndSlide(SandGridComponent grid, float delta)
        {
            const float StepSize = 1;
            Vector2 originalPos = this.Position;
            Vector2 movedBy = this.velocity * delta;

            if (this.Grounded)
            {
                movedBy.Y += 2;
            }

            this.Position.X += movedBy.X;
            if (this.IsColliding(grid))
            {
                this.Position.X = originalPos.X;
                float distanceLeft = movedBy.X;
                float sign = float.Sign(distanceLeft);
                distanceLeft *= sign; // force this to be positive
                while(distanceLeft > 0)
                {
                    float step = sign * Math.Min(distanceLeft, StepSize);
                    this.Position.X += step;
                    distanceLeft -= StepSize;
                    if (this.IsColliding(grid))
                    {
                        float slopeUpStep = 1f;
                        this.Position.Y -= slopeUpStep;
                        if(this.IsColliding(grid)){
                            this.Position.X -= step;
                            this.Position.Y += slopeUpStep;
                            break;
                        }
                        else
                        {
                            this.velocity.Y = (this.Position.Y - originalPos.Y) / delta;
                        }
                    }
                }
                this.velocity.X = (this.Position.X - originalPos.X) / delta;
            }

            bool newGrounded = false;
            this.Position.Y += movedBy.Y;
            if (this.IsColliding(grid))
            {
                this.Position.Y = originalPos.Y;
                float distanceLeft = movedBy.Y;
                float sign = float.Sign(distanceLeft);

                distanceLeft *= sign; // force this to be positive

                if(sign > 0)
                {
                    newGrounded = true;
                    this.groundedBuffer = this.jumpBufferTime;
                    if (this.Grounded)
                    {
                        distanceLeft += 1;
                        this.Position.Y -= sign;
                    }
                }
                
                while(distanceLeft > 0)
                {
                    float step = sign * Math.Min(distanceLeft, StepSize);
                    this.Position.Y += step;
                    distanceLeft -= StepSize;
                    if (this.IsColliding(grid))
                    {
                        this.Position.Y -= step;
                        break;
                    }
                }
                this.velocity.Y = (this.Position.Y - originalPos.Y) / delta;
            }
            this.Grounded = newGrounded;
        }

        private bool IsColliding (SandGridComponent grid)
        {
            
            return grid.IsSolid(this.PixelPosition, this.PixelPosition + new Point(this.myWidth, this.myHeight));
        }

        private void ApplyFallingSand(SandGridComponent grid)
        {
            Point min = this.PixelPosition;
            Point max = min + new Point(this.myWidth, this.myHeight);
            max += new Point(1,1);
            min -= new Point(1,1);

            ZCut curr = new ZCut(SandGrid.ToUintRange(min), SandGrid.ToUintRange(max));
            List<ZCut> cutsStack = new();
            int items = 0;
            while (true)
            {
                if(curr.Split(out ZCut cut, 0))
                {
                    cutsStack.Add(cut);
                    items ++;
                }
                else
                {
                    for(ulong i = curr.min; i <= curr.max; i++)
                    {
                        ref SandPixel pixel = ref grid.GetPixel(i);
                        if(pixel.id == PixelId.SAND)
                            pixel.id = PixelId.FALLING_SAND;
                    }
                    if(items == 0)
                        break;
                    curr = cutsStack[items - 1];
                    cutsStack.RemoveAt(items - 1);
                    items --;
                }
            }
        }
    }
}
