using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LudumDareGame
{
    class Ground
    {
        StaticSprite Ssprite;
        Vector2 position;
        public bool walkable;
        public Ground(Game1 game, string sprPath, Vector2 pos, bool walkable){

            this.position = pos;
            Ssprite = new StaticSprite(game, sprPath);
            this.walkable = walkable;

        }

        public void Draw(SpriteBatch batch) {

            Ssprite.Draw(batch, position, Color.White, 0);
        }
    }
}
