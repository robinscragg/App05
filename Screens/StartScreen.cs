using App05MonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace App05MonoGame.Screens
{
    public class StartScreen : IUpdateableInterface, IDrawableInterface
    {
        #region fields

        private App05Game game;
        private Texture2D backgroundImage;
        private SpriteFont arialFont;

        private Button coinsButton;

        private List<string> instructions;

        #endregion
        public StartScreen(App05Game game)
        {
            this.game = game;
            LoadContent();
        }

        public void LoadContent()
        {
            backgroundImage = game.Content.Load<Texture2D>(
                "backgrounds/Space6000x4000");

            CreateInstructions();

            arialFont = game.Content.Load<SpriteFont>("fonts/arial");
            
            SetupCoinsButton();
                
        }

        private void SetupCoinsButton()
        {
            coinsButton = new Button(arialFont,
                game.Content.Load<Texture2D>("Controls/button-icon-png-200"))
            {
                Position = new Vector2(App05Game.Game_Width - 150,App05Game.Game_Height - 100),
                Text = "Start",
                Scale = 0.6f
            };

            coinsButton.Click += StartCoinsGame;
        }

        /// <summary>
        /// A short summary explaining how to play the game
        /// </summary>
        private void CreateInstructions()
        {
            instructions = new List<string>
            {
                "The player can move around by using arrow keys or WASD",
                "Every time the player collides with a key their score increases",
                "A dog moves around and shoots at the player",
                "Every time the dog collides with the player they loose energy",
                "The player can use the space bar to shoot at the enemy",
                "Energy is also lost when walking into rocks",
                "The game is won when the player scores 50",
                "The game is lost when the player dies"
            };
        }

        private void StartCoinsGame(object sender, System.EventArgs e)
        {
            game.GameState = GameStates.PlayingLevel1;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(backgroundImage, Vector2.Zero, Color.White);

            int y = 100;
            foreach(string line in instructions)
            {
                y += 40;
                int x = 100;
                spriteBatch.DrawString(arialFont, line,
                    new Vector2(x, y), Color.White);
            }

            coinsButton.Draw(spriteBatch, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            coinsButton.Update(gameTime);
        }
    }
}
