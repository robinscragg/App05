using App05MonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace App05MonoGame.Controllers
{

    /// <summary>
    /// This class creates a list of keys which
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

        private Texture2D doorImage;

        public Sprite door;

        private int doorNumber;

        private int chosenY;

        /// <summary>
        /// Create a new list of keys
        /// </summary>
        public DoorsController(App05Game game)
        {
            this.game = game;

            doorImage = game.Content.Load<Texture2D>("Actors/door");

            CreateDoor();
        }

        /// <summary>
        /// Create an animated sprite of a copper coin
        /// which could be collected by the player for a score
        /// </summary>
        public void CreateDoor()
        {
            doorNumber++;

            Random number = new Random();

            if(doorNumber % 2 == 0 )
            {
                chosenY = 670;
            }

            else
            {
                chosenY = 30;
            }

            int randomX = number.Next(100, App05Game.Game_Width - 100);

            door = new Sprite(doorImage, randomX, chosenY);

            door.IsAlive = false;
            door.IsActive = false;

        }

        /// <summary>
        /// If the sprite collides with a coin the coin becomes
        /// invisible and inactive.  A sound is played
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

