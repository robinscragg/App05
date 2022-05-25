using App05MonoGame.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace App05MonoGame.Sprites
{
    /// <summary>
    /// This is a basic sprite which has a single image which
    /// can be scaled and rotated around an origin.  The Bounding
    /// Box is the Rectangle the image occupies, and the Bounday
    /// if it exists is the area inside outside which the Sprite can
    /// not move.  Direction is a Vector such as (0, 1) which indicate
    /// the down direction, and Speed is the rate of movement.  A
    /// Speed of 60 is one pixel per second.  The Sprite can only
    /// move if it is Active and Alive.
    /// </summary>
    public class Sprite: 
        ICloneable, IDrawableInterface, IUpdateableInterface
    {
        #region Properties

        // Single Image
        public Texture2D Image { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 NewPosition { get; set; }

        // A rectangle limiting where the sprite can move
        public Rectangle Boundary { get; set; }

        // Speed = 60 is 1 Pixel/second
        public float Speed { get; set; }

        public Vector2 Origin { get; set; }

        public float Rotation { get; set; }

        public float RotationSpeed { get; set; }

        public Vector2 Direction { get; set; }

        public float Scale { get; set; }

        public SpriteFont TextFont { get; set; }

        public bool IsVisible { get; set; }

        public bool IsAlive { get; set; }

        public bool IsActive { get; set; }

        public bool ReachedEdge { get; set; }

        public virtual int Width
        {
            get { return Image.Width; }
        }

        public virtual int Height
        {
            get { return Image.Height; }
        }

        // The rectangle occupied by the unscaled image
        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle
                (
                    (int)Position.X,
                    (int)Position.Y,
                    (int)(Width * Scale), (int)(Height * Scale)
                );
            }
        }

        public SpriteAttribute Score { get; set; }
        public SpriteAttribute Health { get; set; }

        // Variables

        #endregion

        #region Attributes

        protected float deltaTime;

        protected bool debug = false;

        #endregion

        /// <summary>
        /// Create a sprite that is active, alive and
        /// visible with no speed, rotation or scale
        /// facing east (to the right)
        /// </summary>
        public Sprite()
        {
            if (Image != null)
                Origin = new Vector2(Width / 2, Height / 2);
            else Origin = Vector2.Zero;

            Direction = new Vector2(1, 0);
            Speed = 0;

            IsVisible = true;
            IsAlive = true;
            IsActive = true;
            ReachedEdge = false;

            Scale = 1;
            Rotation = 0;
            RotationSpeed = 0;

            Boundary = new Rectangle(0,0,App05Game.Game_Width, App05Game.Game_Height);

            Score = new SpriteAttribute(0, 50, 0);
            Health = new SpriteAttribute(0, 100, 100);
        }

        /// <summary>
        /// Constructor sets the main image and starting position of
        /// the Sprite as a Vector2
        /// </summary>
        public Sprite(Texture2D image, int x, int y) : this()
        {
            Image = image;
            Position = new Vector2(x, y);
            NewPosition = new Vector2(x, y);
        }

        public Sprite(Texture2D image)
        {
            Image = image;
        }

        public bool HasCollided(Sprite otherSprite)
        {
            if (BoundingBox.Intersects(otherSprite.BoundingBox))
            {
                int margin = 8 * (int)Scale;
                Rectangle overlap = Rectangle.Intersect(BoundingBox, otherSprite.BoundingBox);
                if (overlap.Width > margin)
                    return true;
            }

            return false;
        }

        public virtual void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (IsActive && IsAlive)
            {
                Rotation += MathHelper.ToRadians(RotationSpeed);
                NewPosition = Position + ((Direction * Speed) * deltaTime);

                if (Boundary.Width == 0 || Boundary.Height == 0)
                {
                    Position = NewPosition;
                }
                else if (NewPosition.X >= Boundary.X &&
                    NewPosition.Y >= Boundary.Y &&
                    NewPosition.X + Width < Boundary.X + Boundary.Width &&
                    NewPosition.Y + Height < Boundary.Y + Boundary.Height)
                {
                    Position = NewPosition;
                }
                else
                {
                    ReachedEdge = true;
                }

            }
            
            if (Health.Value == 0)
            {
                IsVisible = false;
                IsAlive = false;
                IsActive = false;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (debug)
            {
                TextHelper.DrawString(
                    $"({Position.X:0},{Position.Y:0})", Position);
            }

            if (Origin == Vector2.Zero)
                Origin = new Vector2(Width / 2, Height / 2);

            if(IsVisible)
            {
                spriteBatch.Draw
                    (Image,
                     Position,
                     null,
                     Color.White, Rotation, Origin,
                     Scale, SpriteEffects.None, 1);
            }
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
