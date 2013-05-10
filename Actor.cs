using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

/*
 * This comment is added by Troll Mastah Techno Ganstah
 */

namespace LudumDareGame
{
    class Actor
    {
        int temp_ = 190;
        #region variables
        AnimatedSprite animatedSpr;
        Rectangle bounds;
        Vector2 position, oldPos;
        Game game;
        KeyboardState ks;
        MouseState ms;
        const float SPEED = 1.0f;
        public bool isControlled;
        float angle;
        int health;
        string name;
        Color color;
        
        #endregion
        #region class creation
        public Actor(Game1 game, Vector2 start, string spritepath, string name) {

            this.name = name;
            this.game = game;
            this.position = start;
            this.bounds = new Rectangle((int)start.X, (int)start.Y, 32/2, 32/2);
            this.animatedSpr = new AnimatedSprite(game, spritepath, 32, 100);
            this.isControlled = false;
            this.health = 100;
            ks = Keyboard.GetState();
            color = Color.White;
        }
        #endregion
        #region functions
        public void Update(GameTime gameTime, Matrix camTrans){
            if (this.isControlled)
            {
                ms = Mouse.GetState();
                Vector2 mousePos = new Vector2(ms.X, ms.Y);
                mousePos = Vector2.Transform(mousePos, Matrix.Invert(camTrans));
                Vector2 direction = mousePos - this.position;
                ks = Keyboard.GetState();
                direction.Normalize();
                this.angle = (float)Math.Atan2((double)direction.Y, (double)direction.X);
                oldPos = position;

                if (ks.IsKeyDown(Keys.W))
                {

                    this.Move(0, (SPEED * -1));
                    this.animatedSpr.Update(gameTime);
                }
                if (ks.IsKeyDown(Keys.S))
                {
                    this.Move(0, SPEED);
                    this.animatedSpr.Update(gameTime);
                }

                if (ks.IsKeyDown(Keys.A))
                {
                    this.Move((SPEED * -1), 0);
                    this.animatedSpr.Update(gameTime);
                }

                if (ks.IsKeyDown(Keys.D))
                {
                    this.Move(SPEED, 0);
                    this.animatedSpr.Update(gameTime);
                }

                this.bounds.X = (int)this.position.X;
                this.bounds.Y = (int)this.position.Y;

                if (this.position.X <= 20 || this.position.X >= 600) {

                    setOldPos();
                }

                if (this.position.Y <= 20 || this.position.Y >= 450) {
                    setOldPos();
                }
                
            }

        }

        public void setColor(Color col) {
            this.color = col;
        }
        
        public void setOldPos() {

            this.position = oldPos;
            this.bounds.X = (int)this.position.X;
            this.bounds.Y = (int)this.position.Y;
        }

        public bool HasAttacked(Human human) { 
        
            if(this.bounds.Intersects(human.getBounds())){
                if (ms.LeftButton == ButtonState.Pressed && !human.checkIfSafe()) {

                    return true;
                }
                
            }

            return false;
        }

        public void ControlCharacter() {

            this.isControlled = true;

        }
        public void ReleaseCharacter() {
            this.isControlled = false;
        }
        public Vector2 getPos() {

            return this.position;
        }

        public Color getColor() {
            return this.color;
        }

        public void Move(float x, float y) {
           
            this.position.X += x;
            this.position.Y += y;
        }

        public string getName() {

            return this.name;
            
        }

        public void Draw(GameTime gameTime, SpriteBatch batch){

            this.animatedSpr.Draw(batch, this.position, color, angle);
        }

        #endregion
    }
}
