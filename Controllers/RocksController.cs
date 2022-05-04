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
    public class RocksController : IUpdateableInterface,
        IDrawableInterface, ICollideableInterface
    {
        private App05Game game;

        private Texture2D rockImage;

        private Sprite rock;

        /// <summary>
        /// Create a new list of keys
        /// </summary>
        public RocksController(App05Game game)
        {
            this.game = game;

            rockImage = game.Content.Load<Texture2D>("Actors/rock");

            CreateRock();
        }

        /// <summary>
        /// Create an animated sprite of a copper coin
        /// which could be collected by the player for a score
        /// </summary>
        public void CreateRock()
        {
            SoundController.PlaySoundEffect(Sounds.Coins);

            Random number = new Random();

            int randomX = number.Next(100, App05Game.Game_Width - 150);
            int randomY = number.Next(100, App05Game.Game_Height - 100);
            rock = new Sprite(rockImage, randomX, randomY);

        }

        public void RemoveRock()
        {
            rock.IsActive = false;
            rock.IsAlive = false;
            rock.IsVisible = false;
        }

        /// <summary>
        /// If the sprite collides with a coin the coin becomes
        /// invisible and inactive.  A sound is played
        /// </summary>
        public void DetectCollision(Sprite sprite)
        {
            if (rock.HasCollided(sprite) && rock.IsAlive)
            {
                sprite.Health.Decrease();
            }
        }

        public void Update(GameTime gameTime)
        {
            rock.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            rock.Draw(spriteBatch, gameTime);
        }
    }
}
