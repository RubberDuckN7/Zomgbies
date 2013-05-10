using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace LudumDareGame
{
    class Level
    {
        public Vector2[] lights;
        public List<Lights> light;
        Game1 game;
        byte[,] levelDesc;
        Ground[,] ground;
        public List<Actor> actors;
        List<Human> humans;
        int humansLeft;
        const byte RTL = 4, RTR = 5, RBL = 3, RBR = 2, BR = 6, BL = 7;
        public Actor currentActor;
        Vector2 sanctuaryPos;
        Color[] zombieColors;
        SoundEffect switchSfx;
        const int W = 21, H = 16, NUMBER_OF_HUMANS = 6  ;

        public Level(Game1 game) {
         
            actors = new List<Actor>();
            actors.Add(new Actor(game,new Vector2(50,50),"Actors\\zombie_sheet", "White"));
            actors.ElementAt(0).ControlCharacter();
            currentActor = actors.ElementAt(0);
            sanctuaryPos = new Vector2(279, 233);
            humans = new List<Human>();
            this.game = game;
            switchSfx = game.Content.Load<SoundEffect>("Sfx\\switch");
            zombieColors = new Color[] { Color.White, Color.Blue, Color.Red, Color.Green };
            
            
            levelDesc = new byte[H,W] { 
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0},
                                        {0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0},
                                        {0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0},
                                        {0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0},
                                        {0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0},
                                        {0,0,0,1,0,0,0,0,0,RTL,RTR,0,0,0,0,0,0,1,0,0,0},
                                        {0,0,0,1,0,0,0,0,0,RBL,RBR,0,0,0,0,0,0,1,0,0,0},
                                        {0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0},
                                        {0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0},
                                        {0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0},
                                        {0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0},
                                        {0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                      };

            ground = new Ground[H, W];

            this.Init();
            
        }

        private void Init() {

            light = new List<Lights>();
            light.Add(new Lights(new Vector2(0.2f, 0.2f),new Vector2(0.7f, 0.7f)));
            light.Add(new Lights(new Vector2(0.8f, 0.2f),new Vector2(0.2f,0.8f)));
            light.Add(new Lights(new Vector2(0.2f, 0.8f), new Vector2(0.8f, 0.2f)));

            for (int i = 0; i < H; ++i) {
                
                for (int j = 0; j < W; ++j) {
                    byte ident = levelDesc[i,j];
                    switch(ident){

                        case 1:
                            ground[i, j] = new Ground(game, "Tiles\\Stone", new Vector2(j * 32, i * 32), true);
                           break;

                        case 0:
                           ground[i, j] = new Ground(game, "Tiles\\Grass", new Vector2(j * 32, i * 32),true);
                           break;

                        case RTR:
                           ground[i, j] = new Ground(game, "Tiles\\Roof_top_right", new Vector2(j * 32, i * 32), false);
                           break;

                        case RTL:
                           ground[i, j] = new Ground(game, "Tiles\\Roof_top_left", new Vector2(j * 32, i * 32), false);
                            break;

                        case RBL:
                            ground[i, j] = new Ground(game, "Tiles\\Roof_bottom_left", new Vector2(j * 32, i * 32), false);
                            break;
                        case RBR:
                            ground[i, j] = new Ground(game, "Tiles\\Roof_bottom_right", new Vector2(j * 32, i * 32), false);
                            break;
                        case BR:
                            ground[i, j] = new Ground(game, "Tiles\\Bench-right", new Vector2(j * 32, i * 32), false);
                            break;

                        case BL:
                            ground[i, j] = new Ground(game, "Tiles\\Bench-left", new Vector2(j * 32, i * 32), false);
                            break;
                    }
                }
            }

            int seed = unchecked(Environment.TickCount);
            Random rand = new Random(1000);
            int ind = NUMBER_OF_HUMANS;
            while (ind != 0)
            {

                Human tmp = new Human(game, new Vector2(rand.Next(50, 600), rand.Next(50, 450)), "Actors\\human_male_sheet");
                if(CheckValidPos(tmp.getPos())){
                    
                    humans.Add(tmp);
                    ind--;
                }
                

            }
            humansLeft = humans.Count;
        }

        public int getSurvivorCount() {

            return this.humansLeft;
        }

        public int getZombieCount() {
            return actors.Count;
        }

        private void EnableSwitchCharacter() {

            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.D1))
            {

                if (!actors.ElementAt(0).isControlled)
                {
                    switchSfx.Play();
                    currentActor.ReleaseCharacter();
                    currentActor = actors.ElementAt(0);
                    currentActor.ControlCharacter();
                }
            }
            else if (ks.IsKeyDown(Keys.D2))
            {
                if (actors.Count >= 2)
                {
                    if (!actors.ElementAt(1).isControlled)
                    {
                        switchSfx.Play();
                        currentActor.ReleaseCharacter();
                        currentActor = actors.ElementAt(1);
                        currentActor.ControlCharacter();
                    }
                }
            }

            else if (ks.IsKeyDown(Keys.D3))
            {
                if (actors.Count >= 3)
                {
                    if (!actors.ElementAt(2).isControlled)
                    {
                        switchSfx.Play();
                        currentActor.ReleaseCharacter();
                        currentActor = actors.ElementAt(2);
                        currentActor.ControlCharacter();
                    }
                }
            }
            else if (ks.IsKeyDown(Keys.D4))
            {
                if (actors.Count >= 4)
                {
                    if (!actors.ElementAt(3).isControlled)
                    {
                        switchSfx.Play();
                        currentActor.ReleaseCharacter();
                        currentActor = actors.ElementAt(3);
                        currentActor.ControlCharacter();
                    }
                }
            }

        }

        public bool CheckValidPos(Vector2 pos) {

            int hx = (int)pos.X / 31;
            int hy = (int)pos.Y / 30;
            if (!ground[hy, hx].walkable) {

                return false;
            }

            return true;
        }

        public Actor getActiveZombie() {
            return this.currentActor;
        }

        public void Update(GameTime gameTime, Matrix camMatrix)
        {

            if (currentActor != null)
                currentActor.Update(gameTime, camMatrix);

            if(!CheckValidPos(currentActor.getPos())){

                currentActor.setOldPos();
            }

            for (int i = 0; i < humans.Count; ++i) {
                 if(!CheckValidPos(humans[i].getPos())){
                    humans.ElementAt(i).makeRandomDestination();
                }
            }

            for (int h = 0; h < humans.Count; ++h) {

                humans[h].Update(gameTime, this.currentActor, sanctuaryPos);
            }

            this.EnableSwitchCharacter();
            this.HandleColissions();
            this.UpdateLights();

        }

        public void UpdateLights() {

            for (int i = 0; i < light.Count; ++i) {

                light[i].Update();
            }
        }

        public void HandleColissions()
        {

            for (int i = 0; i < this.humans.Count; ++i)
            {

                if (this.currentActor.HasAttacked(humans[i]))
                {
                    
                    humans[i].takeDamage();
                    if (humans[i].checkIfDead() && !humans[i].checkIfSafe())
                    {

                        if (actors.Count < 4)
                        {
                            string [] tmpnames = new string[]{"White", "Blue", "Red", "Green"};
                            Actor tmpActor = new Actor(this.game, humans[i].getPos(), "Actors\\zombie_sheet", tmpnames[actors.Count]);
                            tmpActor.setColor(zombieColors[actors.Count]);
                            actors.Add(tmpActor);
                            humans.RemoveAt(i);
                            humansLeft--;
                            break;
                        }
                        else
                        {
                            humansLeft--;
                            break;
                        }
                    }
                    

                    break;

                }
            }

        }

        public void Draw(SpriteBatch batch, GameTime gameTime)
        {

            for (int i = 0; i < H; ++i)
            {

                for (int j = 0; j < W; ++j)
                {
                    if(ground[i,j] != null)
                        ground[i, j].Draw(batch);
                }
            }

            for (int a = 0; a < humans.Count; ++a){

                humans.ElementAt(a).Draw(batch);
            }

            for (int a = 0; a < actors.Count; ++a) {

                actors.ElementAt(a).Draw(gameTime, batch);
            }
        }


    }
}
