using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
namespace LudumDareGame
{
    
    class Menu
    {
        SpriteFont menuFnt;
        string [] menuItemText;
        string indicator, originalMenuText;
        public int currentItem;
        float timer;
        bool pressed;
        KeyboardState ks;
        SoundEffect menuSfx;
        Texture2D logo;
        public Menu(Game1 game) {

            menuFnt = game.Content.Load<SpriteFont>("Fonts\\MenuFont");
            menuItemText = new string[] {"Start", "Instructions", "Quit" };
            currentItem = 0;
            indicator = "->";
            originalMenuText = menuItemText[0];
            menuItemText[0] = indicator + menuItemText[0];
            timer = 0;
            menuSfx = game.Content.Load<SoundEffect>("Sfx\\select");
            logo = game.Content.Load<Texture2D>("Other\\Logo");

        }

        public void Update(GameTime gameTime) {
           ks  = Keyboard.GetState();

            if (timer/1000 >= 0.1f)
            {
                pressed = false;
                if (!pressed)
                {

                    if (ks.IsKeyDown(Keys.Down))
                    {
                        timer = 0;
                        pressed = true;
                        if (currentItem < menuItemText.Length-1)
                        {
                            menuSfx.Play();
                            menuItemText[currentItem] = originalMenuText;
                            currentItem++;
                            originalMenuText = menuItemText[currentItem];
                            menuItemText[currentItem] = indicator + menuItemText[currentItem];

                        }
                    }
                    else if (ks.IsKeyDown(Keys.Up))
                    {
                        timer = 0;
                        pressed = true;
                        if (currentItem > 0)
                        {
                            menuSfx.Play();
                            menuItemText[currentItem] = originalMenuText;
                            currentItem--;
                            originalMenuText = menuItemText[currentItem];
                            menuItemText[currentItem] = indicator + menuItemText[currentItem];
                        }
                    }
                }
            }
            else {

                timer += gameTime.ElapsedGameTime.Milliseconds;
            }     
            
        }

        public void Draw(SpriteBatch batch) {

            batch.Draw(logo, new Vector2(190, 10), Color.White);
            for(int i = 0; i < menuItemText.Length; ++i){

                batch.DrawString(menuFnt, menuItemText[i], new Vector2(100, 50 * i + 150), Color.White);
            }
        }

    }
}
