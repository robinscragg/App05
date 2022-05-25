using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace App05MonoGame.Sprites
{
    /// <summary>
    /// This method will update any game object that
    /// is updatable
    /// </summary>
    public interface IUpdateableInterface
    {
        public void Update(GameTime gameTime);

    }
}
