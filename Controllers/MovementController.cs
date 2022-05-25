using App05MonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace App05MonoGame.Controllers
{
    public class MovementController
    {
        public InputKeys InputKeys { get; set; }

        public MovementController()
        {
            InputKeys = new InputKeys()
            {
                Up = Keys.Up,
                Down = Keys.Down,
                Left = Keys.Left,
                Right = Keys.Right,

                UpLetter = Keys.W,
                DownLetter = Keys.S,
                LeftLetter = Keys.A,
                RightLetter = Keys.D,

            };
        }

        public Vector2 ChangeDirection(KeyboardState keyState)
        {
            Vector2 Direction = Vector2.Zero;

            if (keyState.IsKeyDown(InputKeys.Right) || keyState.IsKeyDown(InputKeys.RightLetter))
            {
                Direction = new Vector2(1, 0);
            }

            if (keyState.IsKeyDown(InputKeys.Left) || keyState.IsKeyDown(InputKeys.LeftLetter))
            {
                Direction = new Vector2(-1, 0);
            }

            if (keyState.IsKeyDown(InputKeys.Up) || keyState.IsKeyDown(InputKeys.UpLetter))
            {
                Direction = new Vector2(0, -1);
            }

            if (keyState.IsKeyDown(InputKeys.Down) || keyState.IsKeyDown(InputKeys.DownLetter))
            {
                Direction = new Vector2(0, 1);
            }

            return Direction;
        }

    }
}
