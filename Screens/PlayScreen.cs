using App05MonoGame.Controllers;
using App05MonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace App05MonoGame.Screens
{
    public class PlayScreen : IUpdateableInterface, IDrawableInterface
    {
        #region Attributes

        private App05Game game;
        private Texture2D backgroundImage;
        private Button pauseButton;

        // Arial large font and calibri small font

        private SpriteFont arialFont;
        private SpriteFont calibriFont;

        public AnimatedPlayer playerSprite;
        public AnimatedSprite enemySprite;
        private Texture2D enemySpriteImage;
        public KeysController keysController;
        public DoorsController doorsController;
        public RocksController rocksController;

        private double timeElapsed;
        private const double delay = 2;
        private double remainingDelay = delay;

        private string[] keys;

        #endregion
        public PlayScreen(App05Game game)
        {
            this.game = game;
            keys = new string[] { "Down", "Left", "Right", "Up" };
            timeElapsed = 0;
            LoadContent();
        }

        public void LoadContent()
        {
            backgroundImage = game.Content.Load<Texture2D>(
                "backgrounds/background");

            arialFont = game.Content.Load<SpriteFont>("fonts/arial");
            calibriFont = game.Content.Load<SpriteFont>("fonts/calibri");

            pauseButton = new Button(arialFont,
                game.Content.Load<Texture2D>("Controls/button-icon-png-200"))
            {
                Position = new Vector2(App05Game.Game_Width - 150, App05Game.Game_Height - 100),
                Text = "Pause",
                Scale = 0.6f
            };

            pauseButton.Click += PauseGame;

            SetupKeys();
            SetupDoors();
            SetupAnimatedPlayer();
            SetupEnemy();
            SetupRocks();
        }

        /// <summary>
        /// Create a controller for keys with one key
        /// </summary>
        private void SetupKeys()
        {
            keysController = new KeysController(game);
        }

        /// <summary>
        /// Create a controller for doors with one door
        /// </summary>
        private void SetupDoors()
        {
            doorsController = new DoorsController(game);
        }

        /// <summary>
        /// Create a controller for rocks with one rock
        /// </summary>
        private void SetupRocks()
        {
            rocksController = new RocksController(game);
        }

        /// <summary>
        /// This is a Sprite with four animations for the four
        /// directions, up, down, left and right
        /// </summary>
        private void SetupAnimatedPlayer()
        {
            Texture2D sheet4x3 = game.Content.Load<Texture2D>("Actors/rsc-sprite-sheet1");

            AnimationController controller = new AnimationController(game.Graphics, sheet4x3, 4, 3);

            controller.CreateAnimationGroup(keys);

            playerSprite = new AnimatedPlayer()
            {
                CanWalk = true,
                Scale = 2.0f,

                Position = new Vector2(App05Game.Game_Width / 2, App05Game.Game_Height / 2),
                Speed = 200,
                Direction = new Vector2(0, 1),

                Rotation = MathHelper.ToRadians(0),
                RotationSpeed = 0f
            };

            controller.AppendAnimationsTo(playerSprite);
        }

        /// <summary>
        /// This is an enemy Sprite with four animations for the four
        /// directions, up, down, left and right.  Has no intelligence!
        /// </summary>
        public void SetupEnemy()
        {
            if (doorsController.doorNumber %3 == 0)
            {
                 enemySpriteImage = game.Content.Load<Texture2D>("Actors/rsc-sprite-sheet3");
            }
            
            else if (doorsController.doorNumber %2 == 0)
            {
                enemySpriteImage = game.Content.Load<Texture2D>("Actors/rsc-sprite-sheet2");
            }
            
            else
            {
                enemySpriteImage = game.Content.Load<Texture2D>("Actors/sprite-sheet1");
            }

            AnimationController manager = new AnimationController(game.Graphics, enemySpriteImage, 4, 3);

            manager.CreateAnimationGroup(keys);

            enemySprite = new AnimatedSprite()
            {
                Scale = 2.0f,

                Position = new Vector2(App05Game.Game_Width - 100, App05Game.Game_Height / 2),
                Direction = new Vector2(-1, 0),
                Speed = 50,

                Rotation = MathHelper.ToRadians(0),
            };

            manager.AppendAnimationsTo(enemySprite);
            RandomMovement(enemySprite);
            
        }

        /// <summary>
        /// Chooses a random direction for the sprite to walk
        /// in. Makes sure the sprite is facing the right direction
        /// </summary>
        /// <param name="movementsprite"></param>
        private void RandomMovement(AnimatedSprite movementsprite)
        {
            Random random = new Random();
            int index = random.Next(keys.Length);

            if (index == 0)
            {
                movementsprite.Direction = new Vector2(0, 1);
            }

            else if (index == 1)
            {
                movementsprite.Direction = new Vector2(-1, 0);
            }

            else if (index == 2)
            {
                movementsprite.Direction = new Vector2(1, 0);
            }

            else
            {
                movementsprite.Direction = new Vector2(0, -1);
            }

            movementsprite.PlayAnimation(keys[index]);
        }

        /// <summary>
        /// When game is paused, song pauses and text on button changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PauseGame(object sender, System.EventArgs e)
        {
             game.Paused = !(game.Paused);

            if (game.Paused)
            {
                SoundController.PauseSong();
                pauseButton.Text = "Play";
            }
            else
            {
                pauseButton.Text = "Pause";
                SoundController.ResumeSong();
            }
        }

        /// <summary>
        /// Update player, enemy, keys, doors and rocks
        /// When player walks into enemy or enemy walks into edge of screen,
        /// enemy walks in random direction
        /// </summary>
        public void Update(GameTime gameTime)
        {
            pauseButton.Update(gameTime);

            if(!game.Paused)
            {
                var timer = (float) gameTime.ElapsedGameTime.TotalSeconds;
                remainingDelay -= timer;
                timeElapsed = Math.Round(timeElapsed += gameTime.ElapsedGameTime.TotalSeconds, 2);
                if(playerSprite.Health.Value == 0)
                {
                    game.GameState = GameStates.Ending;

                }

                if(playerSprite.Score.Value == playerSprite.Score.MaximumValue)
                {
                    game.GameState = GameStates.Ending;
                }

                if(remainingDelay <= 0)
                {
                    RandomMovement(enemySprite);
                    remainingDelay = delay;
                }

                playerSprite.Update(gameTime);

                enemySprite.Update(gameTime);

                if (playerSprite.HasCollided(enemySprite))
                {
                    playerSprite.Health.Decrease();
                    RandomMovement(enemySprite);
                }

                if( enemySprite.ReachedEdge == true)
                {
                    RandomMovement(enemySprite);
                    enemySprite.ReachedEdge = false;
                }

                keysController.Update(gameTime);
                keysController.DetectCollision(playerSprite);

                doorsController.Update(gameTime);
                doorsController.DetectCollision(playerSprite);

                rocksController.Update(gameTime);
                rocksController.DetectCollision(playerSprite);
             
            }
        }

        /// <summary>
        /// Draw Player, enemy, keys, doors and rocks
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(backgroundImage, Vector2.Zero, Color.White);

            pauseButton.Draw(spriteBatch, gameTime);

            playerSprite.Draw(spriteBatch, gameTime);
            keysController.Draw(spriteBatch, gameTime);
            doorsController.Draw(spriteBatch, gameTime);
            rocksController.Draw(spriteBatch, gameTime);
            enemySprite.Draw(spriteBatch, gameTime);

            DrawGameStatus(spriteBatch);
            DrawGameFooter(spriteBatch);
        }
        /// <summary>
        /// Display the name of the game, the time, the current score
        /// and status of the player at the top of the screen
        /// </summary>
        public void DrawGameStatus(SpriteBatch spriteBatch)
        {
            
            int score = playerSprite.Score.Value;
            int health = playerSprite.Health.Value;

            int topMargin = 4;
            int sideMargin = 50;

            Vector2 topLeft = new Vector2(sideMargin, topMargin);
            string status = $"Score = {score:##0}";

            spriteBatch.DrawString(arialFont, status, topLeft, Color.White);

            Vector2 gameSize = arialFont.MeasureString(App05Game.GameName);
            Vector2 topCentreLeft = new Vector2((App05Game.Game_Width / 2 - gameSize.X / 2 - 100), topMargin);
            spriteBatch.DrawString(arialFont, App05Game.GameName, topCentreLeft, Color.White);

            string time =$"Time = { timeElapsed }";
            Vector2 topCentreRight = new Vector2((App05Game.Game_Width / 2 - gameSize.X / 2 + 50), topMargin);
            spriteBatch.DrawString(arialFont, time, topCentreRight, Color.White);

            string healthText = $"Health = {health:##0}%";
            Vector2 healthSize = arialFont.MeasureString(healthText);
            Vector2 topRight = new Vector2(
                App05Game.Game_Width - (healthSize.X + sideMargin), topMargin);
            
            spriteBatch.DrawString(arialFont, healthText, topRight, Color.White);

        }

        /// <summary>
        /// Display identifying information such as the the App,
        /// the Module, the authors at the bottom of the screen
        /// </summary>
        public void DrawGameFooter(SpriteBatch spriteBatch)
        {
            int bottomMargin = 30;

            Vector2 namesSize = calibriFont.MeasureString(App05Game.AuthorNames);
            Vector2 appSize = calibriFont.MeasureString(App05Game.AppName);

            Vector2 bottomCentre = new Vector2(
                (App05Game.Game_Width - namesSize.X) / 2, 
                App05Game.Game_Height - bottomMargin);

            Vector2 bottomLeft = new Vector2(
                bottomMargin, App05Game.Game_Height - bottomMargin);

            Vector2 bottomRight = new Vector2(
                App05Game.Game_Width - appSize.X - bottomMargin, 
                App05Game.Game_Height - bottomMargin);

            spriteBatch.DrawString(calibriFont, 
                App05Game.AuthorNames, bottomCentre, Color.Yellow);
            spriteBatch.DrawString(calibriFont, 
                App05Game.ModuleName, bottomLeft, Color.Yellow);
            spriteBatch.DrawString(calibriFont, 
                App05Game.AppName, bottomRight, Color.Yellow);
        }

    }
}
