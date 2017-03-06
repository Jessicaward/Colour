using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Colour
{
    class Player
    {
        //Objects
        GlobalVariables global = new GlobalVariables();

        private bool dead = false;
        private int secondsPassed = 0;

        //Player Position
        private int playerTileX = 1;
        private int playerTileY = 1;

        public bool Dead
        {
            get
            {
                return dead;
            }
        }

        public int returnXTile
        {
            get
            {
                return playerTileX;
            }    
        }

        public int returnYTile
        {
            get
            {
                return playerTileY;
            }
        }

        public int returnTime
        {
            get
            {
                return secondsPassed;
            }
        }

        private void PlayMoveSound()
        {
            global.ButtonClickSound.Play();
        }

        public void MoveLeft()
        {
            playerTileX--;
            //PlayMoveSound();
        }

        public void MoveRight()
        {
            playerTileX++;
            //PlayMoveSound();
        }

        public void MoveUp()
        {
            playerTileY--;
            //PlayMoveSound();
        }

        public void MoveDown()
        {
            playerTileY++;
            //PlayMoveSound();
        }

        private int step = 0;

        public void Update(GameTime gameTime)
        {
            if (step >= 60)
            {
                step = 0;
            }
            else
            {
                step++;
            }

            KeyboardState newKeyState = Keyboard.GetState();
            KeyboardState oldKeyState = newKeyState;

            Console.WriteLine("X: " + playerTileX);
            Console.WriteLine("Y: " + playerTileY);

            //4 steps per second
            if (step == 0 || step == 15 || step == 30 || step == 45)
            {
                if(!dead)
                {
                    if (playerTileX >= 0 && playerTileY >= 0)
                    {
                        if (newKeyState.IsKeyDown(Keys.A))
                        {
                            MoveLeft();
                        }
                        if (newKeyState.IsKeyDown(Keys.D))
                        {
                            MoveRight();
                        }
                        if (newKeyState.IsKeyDown(Keys.S))
                        {
                            MoveDown();
                        }
                        if (newKeyState.IsKeyDown(Keys.W))
                        {
                            MoveUp();
                        }
                    }
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //Don't draw game graphics in this function.

            //Function not to be used in production.
        }
    }
}