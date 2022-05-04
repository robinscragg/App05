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
    public class KeysController : IUpdateableInterface, 
        IDrawableInterface, ICollideableInterface
    {
        private App05Game game;

        private Texture2D keyImage;

        private Sprite key;

        /// <summary>
        /// Create a new list of keys
        /// </summary>
        public KeysController(App05Game game)
        {
            this.game = game;

            keyImage = game.Content.Load<Texture2D>("Actors/key");
            
            CreateKey();
        }

        /// <summary>
        /// Create an animated sprite of a copper coin
        /// which could be collected by the player for a score
        /// </summary>
        public void CreateKey()
        {
            SoundController.PlaySoundEffect(Sounds.Coins);

            Random number = new Random();

            int randomX = number.Next(100, App05Game.Game_Width - 150);
            int randomY = number.Next(100, App05Game.Game_Height - 100);
            key = new Sprite(keyImage, randomX, randomY);

        }

        /// <summary>
        /// If the sprite collides with a coin the coin becomes
        /// invisible and inactive.  A sound is played
        /// </summary>
        public void DetectCollision(Sprite sprite)
        {
            if (key.HasCollided(sprite) && key.IsAlive)
            {
                SoundController.PlaySoundEffect(Sounds.Coins);

                 key.IsActive = false;
                 key.IsAlive = false;
                 key.IsVisible = false;

                 sprite.Score.Increase();

                 game.playScreen.doorsController.door.IsAlive = true;
                 game.playScreen.doorsController.door.IsActive = true;

            }         
        }

        public void Update(GameTime gameTime)
        {
            key.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
             key.Draw(spriteBatch, gameTime);
        }
    }
}
