using App05MonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace App05MonoGame.Screens
{
    public class EndScreen : IUpdateableInterface, IDrawableInterface
    {
        private App05Game game;
        private Texture2D backgroundImage;
        private Button replayButton;
        private SpriteFont arialFont;


        public EndScreen(App05Game game)
        {
            this.game = game;
            LoadContent();
        }

        /// <summary>
        /// Loads two different backgrounds depending on whether 
        /// win or loose
        /// </summary>
        public void LoadContent()
        {
            if (game.playScreen.playerSprite.Health.Value == 0)
            {
                backgroundImage = game.Content.Load<Texture2D>(
                "backgrounds/loose_screen");
            }

            else
            {
                backgroundImage = game.Content.Load<Texture2D>("backgrounds/win_screen");
            }

            arialFont = game.Content.Load<SpriteFont>("fonts/arial");

            SetUpButton();
        }

        /// <summary>
        /// Replay button
        /// </summary>
        private void SetUpButton()
        {
            replayButton = new Button(arialFont,
               game.Content.Load<Texture2D>("Controls/button-icon-png-200"))
            {
                Position = new Vector2(App05Game.Game_Width / 2, App05Game.Game_Height / 2),
                Text = "Play again",
                Scale = 0.6f
            };

            replayButton.Click += ReplayCoinsGame;
        }

        private void ReplayCoinsGame(object sender, System.EventArgs e)
        {
            game.playScreen = null;
            game.GameState = GameStates.PlayingLevel1;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(backgroundImage, Vector2.Zero, Color.White);

            replayButton.Draw(spriteBatch, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            replayButton.Update(gameTime);
        }
    }
    
}
