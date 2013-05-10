using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LudumDareGame
{

    class AnimatedSprite
    {
        Texture2D animationTexture;
        public int time, picture;
        public int width, height;
        int animationDelay = 0, nPictures = 0;
        public int dir = 0;
        

        public AnimatedSprite(Game1 game, String sprite, int FrameWidth, int delay)
        {
            this.animationTexture = game.Content.Load<Texture2D>(sprite);
            this.width = FrameWidth;
            this.height = 32;
            this.nPictures = (animationTexture.Width / FrameWidth);
            this.animationDelay = delay;
        }

        public int getWidth() {
            return width;
        }

        public int getHeight() {
            return height;
        }

        public void Update(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.Milliseconds;

            if (time >= animationDelay)
            {
                time = 0;
                picture++;
                if (picture > nPictures - 1)
                {
                    picture = 0;
                }
            }
        }

     
        public bool IsFinished()
        {
            if (picture >= this.nPictures - 1)
            {
                picture = 0;
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos, Color color,float angle)
        {
                Rectangle tmp = new Rectangle((picture % nPictures) * width, 0, width, height);
                spriteBatch.Draw(animationTexture, pos, tmp, color, angle + 90, new Vector2(32 / 2, 32 / 2), 1f, SpriteEffects.None, 0);
        }



    }
}
