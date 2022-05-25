using App05MonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace App05MonoGame.Controllers
{

    /// <summary>
    /// This class creates doors which
    /// can be updated and drawn and checked for
    /// collisions with the player sprite
    /// </summary>
    /// <author>
    /// Robin Scragg
    /// </author>
    public class DoorsController : IUpdateableInterface,
        IDrawableInterface, ICollideableInterface
    {
        private App05Game game;

        public Texture2D doorImage;

        public AnimatedSprite door;

        public int doorNumber;

        private int chosenY;

        /// <summary>
        /// Create a door
        /// </summary>
        public DoorsController(App05Game game)
        {
            this.game = game;

            doorImage = game.Content.Load<Texture2D>("Actors/door_animation");

            CreateDoor();
        }

        /// <summary>
        /// Create an animation sprite of a door. Door appears on opposite sides every time
        /// in a random position. Door animation only happens when door is actived
        /// when key is picked up
        /// </summary>
        public void CreateDoor()
        {
            doorNumber++;

            Random number = new Random();

            if(doorNumber % 2 == 0 )
            {
                chosenY = 550;
            }

            else
            {
                chosenY = 50;
            }

            int randomX = number.Next(100, App05Game.Game_Width - 100);

            Animation animation = new Animation(game.Graphics, "door", doorImage, 9);

            door = new AnimatedSprite()
            {
                Animation = animation,
                Image = animation.FirstFrame,
                Scale = 1.0f,
                Speed = 0,
                Position = new Vector2(randomX, chosenY),
            };

            door.IsAlive = false;
            door.IsActive = false;

        }

        /// <summary>
        /// If the sprite collides with a door a new room
        /// is made. Every third room, a key mini game is 
        /// activated. If the player walks through the door before the mini game
        /// ends, the mini game ends
        /// </summary>
        public void DetectCollision(Sprite sprite)
        {
            if (door.HasCollided(sprite) && door.IsAlive)
            {
                door.IsActive = false;
                door.IsAlive = false;
                door.IsVisible = false;

                CreateDoor();

                game.playScreen.rocksController.RemoveRock();
                game.playScreen.rocksController.CreateRock();
                game.playScreen.keysController.CreateKey();
                game.playScreen.SetupEnemy();

                if (doorNumber % 3 == 0)
                {
                    game.playScreen.keysController.keyGameActive = true;
                }

                else
                {
                    game.playScreen.keysController.ClearKeys();
                    game.playScreen.keysController.CreateKey();
                    game.playScreen.keysController.keyGameActive = false;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            door.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            door.Draw(spriteBatch, gameTime);
        }
    }
}

