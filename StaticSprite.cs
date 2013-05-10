using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LudumDareGame
{
    class StaticSprite
    {
        Texture2D sprite;

        public StaticSprite(Game1 game, string sprite) {

            this.sprite = game.Content.Load<Texture2D>(sprite);
        }

        public void Draw(SpriteBatch batch, Vector2 pos, Color col, float angle) {

            batch.Draw(this.sprite,pos,new Rectangle(0,0,32,32),col,angle, new Vector2( 32 /2, 32 /2), 1f, SpriteEffects.None,0);
        }

    }
}
