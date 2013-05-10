using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LudumDareGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// b
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont hudFont, instructionsFont;
        Camera camera2D;
        Level level;
        float timeMs = 1000, timeSecs = 60, timeMinutes = 1, keyboardTimer = 0;
        Effect lightFx;
        RenderTarget2D normalrender;
        bool isMenuShown, isOptionsShown, isGameScreenShown, GameLost, GameWon;
        Human h;
        Menu menu;
        KeyboardState oldstate;
        SoundEffect sfx_press;
        Color timeCol = Color.White;
        SamplerState ClampSampleState;
        const float DELAY = 0.2f;
        const string instructions = 
            "Use W,A,S,D to move. \n" 
            +"Press the left mouse button to damage the survivors. \n"+ "Use number keys 1 to 4 to switch between new made allies \n" +
            "A Zombie kill team is sent out to rescue the survivors. \nYou must kill the survivors before the time runs out. \n\n" +
            "When the survivors are in the sanctuary they wont go out \nunless you take distance \n\n"+ "Good Luck!";

        const string gameLost = "Game Over! \nYou ran out of time";
        const string gameWon = "Congratulations you have killed the survivors\nbefore the team arrived!";

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "ZOMGbies";
            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            camera2D = new Camera(new Vector2(0,0));
         
            normalrender = new RenderTarget2D(graphics.GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            level = new Level(this);
            menu = new Menu(this);
            this.isMenuShown = true;
            this.isOptionsShown = false;
            this.isGameScreenShown = false;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            lightFx = Content.Load<Effect>("LightTest");
            hudFont = Content.Load<SpriteFont>("Fonts\\HudFont");
            instructionsFont = Content.Load<SpriteFont>("Fonts\\InstructionsFont");
            sfx_press = Content.Load<SoundEffect>("Sfx\\select_press");
            ClampSampleState = new SamplerState { 
                
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp
            };
           

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        /// 
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

        
            KeyboardState ks = Keyboard.GetState();
            GraphicsDevice.SamplerStates[0] = ClampSampleState;
            if (isMenuShown) {

                menu.Update(gameTime);

                int c = -1;
                if (keyboardTimer / 1000 >= DELAY)
                {
                    if (ks.IsKeyDown(Keys.Space) && !oldstate.IsKeyDown(Keys.Space))
                    {
                        sfx_press.Play();
                        keyboardTimer = 0;
                        c = menu.currentItem;
                    }
                }
                switch (c) { 
                    
                    case 0:
                        isMenuShown = false;
                        isGameScreenShown = true;
                        break;

                    case 1:
                        isMenuShown = false;
                        isOptionsShown = true;
                        break;

                    case 2:
                        Exit();
                        break;

                    default:
                        break;
                
                }
            }
            else if (isOptionsShown) {

                if (keyboardTimer/1000 >= DELAY)
                {

                    if (ks.IsKeyDown(Keys.Space) && !oldstate.IsKeyDown(Keys.Down))
                    {
                        sfx_press.Play();
                        keyboardTimer = 0;
                        isMenuShown = true;
                        isOptionsShown = false;
                    }
                }
                
            }
            else if (GameLost) {

                if (keyboardTimer / 1000 >= DELAY)
                {

                    if (ks.IsKeyDown(Keys.Space) && !oldstate.IsKeyDown(Keys.Down))
                    {
                        sfx_press.Play();
                        keyboardTimer = 0;
                        isMenuShown = true;
                        GameLost = false;
                        level = new Level(this);
                        ResetTimer();
                    }
                }
            }
            else if (GameWon)
            {

                if (keyboardTimer / 1000 >= DELAY)
                {

                    if (ks.IsKeyDown(Keys.Space) && !oldstate.IsKeyDown(Keys.Down))
                    {
                        sfx_press.Play();
                        keyboardTimer = 0;
                        isMenuShown = true;
                        GameWon = false;
                        level = new Level(this);
                        ResetTimer();
                    }
                }
            }
            else if (isGameScreenShown)
            {
                timeMs += gameTime.ElapsedGameTime.Milliseconds;




                if (timeMs / 1000 >= 1)
                {
                    timeSecs--;
                    timeMs = 0;
                }

                if (timeSecs <= 0)
                {
                    timeMinutes--;
                    if (timeMinutes < 0)
                        timeSecs = 0;
                    else
                        timeSecs = 60;
                }

                if (timeMinutes < 1)
                {
                    timeCol = Color.Red;
                }


                if (timeMinutes <= 0 && timeSecs <= 0)
                {
                    GameLost = true;
                    isGameScreenShown = false;

                }

                else if (level.getSurvivorCount() <= 0)
                {
                    GameWon = true;
                    isGameScreenShown = false;
                }


                camera2D.camera2DPos = level.currentActor.getPos();
                level.Update(gameTime, camera2D.transform_get(graphics));
            }

            oldstate = ks;
            if (keyboardTimer/1000 < DELAY) {

                keyboardTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            base.Update(gameTime);
        }

        public void ResetTimer() { 

            timeMs = 1000;
            timeSecs = 60;
            timeMinutes = 1;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (isMenuShown) {

                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin();
                menu.Draw(spriteBatch);
                spriteBatch.End();
            }
            else if (isOptionsShown) {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin();
                spriteBatch.DrawString(instructionsFont, instructions, new Vector2(10, 10), Color.Wheat);
                spriteBatch.DrawString(instructionsFont, "->Back", new Vector2(280, 400), Color.White);
                spriteBatch.End();
            }
            else if (isGameScreenShown)
            {
                GraphicsDevice.SetRenderTarget(normalrender);
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin();

                level.Draw(spriteBatch, gameTime);

                spriteBatch.End();


                GraphicsDevice.SetRenderTarget(null);
                GraphicsDevice.Clear(Color.Black);

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, lightFx, camera2D.transform_get(graphics));
                for (int l = 0; l < level.light.Count; ++l)
                {
                    
                    lightFx.Parameters["pos"].SetValue(level.light[l].getPos());
                    lightFx.Parameters["lightCol"].SetValue(timeCol.ToVector4());

                    spriteBatch.Draw(normalrender, Vector2.Zero, Color.White);
                }
                spriteBatch.End();


                spriteBatch.Begin();
                spriteBatch.DrawString(hudFont, "Time Left: " + timeMinutes + " : " + (timeSecs), new Vector2(250, 10), timeCol);
                spriteBatch.DrawString(hudFont, "Survivors left: " + level.getSurvivorCount(), new Vector2(10, 10), Color.White);
                spriteBatch.DrawString(hudFont, "Zombie team", new Vector2(10, 80), Color.White);
                for (int i = 0; i < level.getZombieCount(); ++i)
                    spriteBatch.DrawString(hudFont, (i + 1).ToString() + ", " + level.actors[i].getName(), new Vector2(10, 10 * i * 3 + 100), Color.White);
                spriteBatch.End();
            }
            else if (GameLost) {

                spriteBatch.Begin();
                spriteBatch.DrawString(instructionsFont, gameLost, new Vector2(250, 150), Color.Wheat);
                spriteBatch.DrawString(instructionsFont, "->To Menu", new Vector2(250, 400), Color.White);
                spriteBatch.End();
            }
            else if (GameWon) {

                spriteBatch.Begin();
                spriteBatch.DrawString(instructionsFont, gameWon, new Vector2(10, 10), Color.Wheat);
                spriteBatch.DrawString(instructionsFont, "->To Menu", new Vector2(280, 400), Color.White);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
