using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace LudumDareGame
{
    class Human
    {
        AnimatedSprite animateSpr, bloodAnim;
        Texture2D hpPieceSprite, bloodSplash;
        Vector2 position, destination;
        Rectangle bounds, healthBar;
        int health;
        const float SPEED = 1.5f;
        float angle;
        string name;
        bool isHit, isDead;
        Random rand;
        bool isSafe, isFleeing;
        SoundEffect hurtsfx;
        public Human(Game1 game, Vector2 pos, string sprPath) {

            int seed = unchecked(Environment.TickCount);
            rand = new Random(seed);
            this.position = pos;
            this.health = 20;
            animateSpr = new AnimatedSprite(game, sprPath, 32, 100);
            bloodAnim = new AnimatedSprite(game, "Other\\Blood", 32, 100);
            angle = 0;
            bounds = new Rectangle((int)position.X, (int)position.Y, animateSpr.getWidth()/2,animateSpr.getHeight()/2);
            healthBar = new Rectangle((int)position.X, (int)position.Y - 10, health, 2);
            isHit = false;
            isDead = false;
            isSafe = false;
            isFleeing = false;
            hpPieceSprite = game.Content.Load<Texture2D>("Other\\HealthBarPiece");
            bloodSplash = game.Content.Load<Texture2D>("Other\\bloodSplash");
            hurtsfx = game.Content.Load<SoundEffect>("Sfx\\hurt");
            

        }

        public void makeRandomDestination() {

            float rx = rand.Next(20, 500);
            float ry = rand.Next(20, 450);
            destination = new Vector2(rx, ry);
        }

        public Rectangle getBounds() {
            return this.bounds;
        }

        public Vector2 getPos() {
            return this.position;
        }
        public bool checkIfDead() {
            return this.isDead;
        }

        public bool checkIfSafe() {
            return this.isSafe;
        }
        public string getName() {
            return this.name;
        }

        public void takeDamage() {
            if (!isHit)
            {
                isHit = true;
                hurtsfx.Play();
                this.health -= 10;

                if (this.health <= 0)
                {
                    this.health = 0;
                    isDead = true;
                    this.bounds = Rectangle.Empty;
                }
            }
        }

        public void Update(GameTime gameTime, Actor zombie, Vector2 sanctuaryPos) {
            
            if (!isDead)
            {
                if (isHit)
                {

                    bloodAnim.Update(gameTime);

                    if (bloodAnim.IsFinished())
                    {
                        isHit = false;
                    }
                }

                Vector2 distance = (zombie.getPos() - this.position);
                float len = distance.Length();

                if (len <= 50 || isFleeing)
                {

                    isFleeing = true;
                    this.animateSpr.Update(gameTime);

                    Vector2 dir = sanctuaryPos - position;

                    if (dir.Length() < 20)
                    {
                        this.isSafe = true;
                        isFleeing = false;
                    }
                    dir.Normalize();
                    this.angle = (float)Math.Atan2((double)dir.Y, (double)dir.X);

                    this.position.X += SPEED * dir.X;
                    this.position.Y += SPEED * dir.Y;


                }
                else if (!isFleeing)
                {

                    if (len >= 100)
                        isSafe = false;
                    if (!isSafe)
                    {
                        this.animateSpr.Update(gameTime);
                        Vector2 dir = destination - position;

                        if (dir.Length() < 100)
                        {
                            makeRandomDestination();
                            dir = destination - position;
                        }
                        dir.Normalize();

                        this.angle = (float)Math.Atan2((double)dir.Y, (double)dir.X);

                        this.position.X += SPEED * dir.X;
                        this.position.Y += SPEED * dir.Y;
                    }
                }


                this.bounds.X = (int)this.position.X;
                this.bounds.Y = (int)this.position.Y;
                this.healthBar.X = (int)this.position.X - 10;
                this.healthBar.Y = (int)this.position.Y - 10;
                this.healthBar.Width = health;
            }
            
        }

        public void Draw(SpriteBatch batch) {

            if (!isDead)
            {
                if (!isSafe)
                {
                    animateSpr.Draw(batch, this.position, Color.White, angle);

                    if (isHit)
                    {

                        bloodAnim.Draw(batch, this.position, Color.White, -90);
                    }

                    batch.Draw(hpPieceSprite, healthBar, Color.Red);
                }
            }
            else {

                batch.Draw(bloodSplash, this.position, Color.White);
            }
        }
    }
}
