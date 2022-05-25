using App05MonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

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

        public List<Sprite> keys;

        private float keyDuration;
        private float keyCurrentTime;
        private double count;
        public bool keyGameActive;

        /// <summary>
        /// Create a new list of keys
        /// </summary>
        public KeysController(App05Game game)
        {
            this.game = game;

            keys = new List<Sprite>();

            keyImage = game.Content.Load<Texture2D>("Actors/key");

            keyDuration = 7;
            keyCurrentTime = 0;
            keyGameActive = false;
            CreateKey();
        }

        /// <summary>
        /// Create an animated sprite of a key
        /// which could be collected by the player for a score.
        /// Key created in a random position
        /// </summary>
        public void CreateKey()
        {
            SoundController.PlaySoundEffect(Sounds.Coins);

            Random number = new Random();

            int randomX = number.Next(100, App05Game.Game_Width - 150);
            int randomY = number.Next(100, App05Game.Game_Height - 100);
            Sprite key = new Sprite(keyImage, randomX, randomY);

            keys.Add(key);

        }

        /// <summary>
        /// If the sprite collides with a key the key becomes
        /// invisible and inactive.  A sound is played and the player's 
        /// score increases
        /// </summary>
        public void DetectCollision(Sprite sprite)
        {
            foreach (Sprite key in keys)
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
        }

        public void Update(GameTime gameTime)
        {
            foreach (Sprite key in keys)
            {
                key.Update(gameTime);
            }


            // Key mini game. Every half a second for 7 seconds a new key appears
            if (keyGameActive == true)
            {
                if (keyCurrentTime > keyDuration)
                {
                    keyCurrentTime = 0;
                    ClearKeys();
                    CreateKey();
                    keyGameActive = false;
                }

                count += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (keyCurrentTime <= keyDuration)
                {
                    if (count > 500)
                    {
                        CreateKey();
                        count = 0;
                    }

                    keyCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }

        /// <summary>
        /// Makes all the keys disappear
        /// </summary>
        public void ClearKeys()
        {
            foreach (Sprite key in keys)
            {
                key.IsActive = false;
                key.IsAlive = false;
                key.IsVisible = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (Sprite key in keys)
            {
                key.Draw(spriteBatch, gameTime);
                game.playScreen.doorsController.door.Draw(spriteBatch, gameTime);
            }
        }
    }
}
