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

namespace Colour
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Objects
        Player player = new Player();
        GlobalVariables global = new GlobalVariables();
        
#region MapDeclaration
        /*
         * Map declaration
         * 0 = dirt
         * 1 = grass
         * 2 = sand
         * 3 = snow
         * 4 = stone
         * 5 = water
         */
        int[,] map = new int[,]
        {
            {1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 5, 5, 5, 5},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 5, 5, 5},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 5, 5},
            {0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 5},
            {1, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 2, 5, 5},
            {0, 1, 1, 1, 0, 0, 0, 1, 1, 0, 0, 2, 2, 5},
            {0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 5},
            {1, 1, 1, 0, 4, 4, 0, 0, 0, 0, 0, 0, 0, 2},
            {0, 0, 0, 0, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        };

        //0 - Unexplored
        //1 - Explored
        int[,] colouredMap = new int[,]
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        };
#endregion

#region GameStates
        /*
         * ----Game States----
         * MainMenu
         * LevelOne
         * GameMenu
         * Objectives
         */

        public enum GameStates
        {
            MainMenu,
            LevelOne,
            GameMenu,
            Objectives
        }

        //Sets current GameState to MainMenu for game startup.
        //This can be changed for testing features ((cont.))
        //without breaking the gamestate feature
        //TEMPORARY SOLUTION UNTIL I GET ENUMERATION WORKING
        //Current List:
        //MainMenu
        //Game
        //Help
        public string currentGameState = "Game";
#endregion

#region textures
        //Declaring Texture2D variables

        //Game Textures
        Texture2D dirtTexture;
        Texture2D grassTexture;
        Texture2D sandTexture;
        Texture2D snowTexture;
        Texture2D stoneTexture;
        Texture2D waterTexture;
        Texture2D playerTexture;

        //Menu Textures
        Texture2D logoTexture;
        Texture2D gradientTexture;

        //Text Textures
        Texture2D HelpInfoTexture;
        Texture2D playTextTexture;
        Texture2D helpTextTexture;
        Texture2D quitTextTexture;

        //Misc Textures
        Texture2D blankTexture;
#endregion

#region inputstates
        //GetStates
        KeyboardState keyState = Keyboard.GetState();
        MouseState mouseState = Mouse.GetState();
        MouseState previousMouseState;
#endregion

#region rectangles
        //1500
        //750
        Rectangle playButtonRect = new Rectangle(287, 200, 125, 61);
        Rectangle helpButtonRect = new Rectangle(287, 261, 125, 61);
        Rectangle quitButtonRect = new Rectangle(287, 322, 125, 61);
#endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //This makes the mouse visible
            IsMouseVisible = true;

            //Changing Screen Resolution
            graphics.PreferredBackBufferHeight = 700;
            graphics.PreferredBackBufferWidth = 700;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

#region loadtextures
            //Initializing Textures
            dirtTexture = Content.Load<Texture2D>(@"textures\dirt");
            grassTexture = Content.Load<Texture2D>(@"textures\grass");
            sandTexture = Content.Load<Texture2D>(@"textures\sand");
            snowTexture = Content.Load<Texture2D>(@"textures\Snow");
            stoneTexture = Content.Load<Texture2D>(@"textures\stone");
            waterTexture = Content.Load<Texture2D>(@"textures\water");
            playerTexture = Content.Load <Texture2D>(@"textures\player");
            logoTexture = Content.Load<Texture2D>("Logo");

            blankTexture = Content.Load<Texture2D>(@"textures\blank");

            playTextTexture = Content.Load<Texture2D>(@"text\PlayText");
            helpTextTexture = Content.Load<Texture2D>(@"text\HelpText");
            quitTextTexture = Content.Load<Texture2D>(@"text\QuitText");
            HelpInfoTexture = Content.Load<Texture2D>(@"text\HelpInformation");

            gradientTexture = Content.Load<Texture2D>(@"gradients\yelloworange");
#endregion

#region loadsound
            global.ButtonClickSound = Content.Load<SoundEffect>(@"sounds\ButtonClick");
#endregion
        }


        protected override void UnloadContent()
        {

        }

        public void PlayMoveSound()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;

            if (currentGameState == "Game")
            {
                //Game Update Logic
                player.Update(gameTime);

                //This for statement checks whether the player has explored the 3x3 grid of tiles around the player, if not, it explores it.
                for (int x = (player.returnYTile - 1); x < (player.returnYTile + 2); x++)
                {
                    //Y Version
                    for (int y = (player.returnXTile - 1); y < (player.returnXTile + 2); y++)
                    {
                        if (colouredMap[player.returnXTile, player.returnYTile] == 0)
                        {
                            //Checks if the player is out of bounds
                            if ((x >= 0 && y >= 0) && x != map.GetLength(0) && y != map.GetLength(1))
                            {
                                colouredMap[x, y] = 1;
                            }
                        }
                    }
                }
            }
            else if (currentGameState == "MainMenu")
            {
                MouseState UmouseState = Mouse.GetState();
                if (UmouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                {
                    MouseClicked(mouseState.X, mouseState.Y);
                }
                previousMouseState = UmouseState;
            }
            else if (currentGameState == "Help")
            {

            }

            base.Update(gameTime);
        }

        void MouseClicked(int x, int y)
        {
            //Creates a 10x10 rectangle around the mouse click area.
            Rectangle MouseClickRect = new Rectangle(x, y, 10, 10);

            //Start Menu
            if (currentGameState == "MainMenu")
            {
                Rectangle playclickrect = new Rectangle((int)playButtonRect.X, (int)playButtonRect.Y, 125, 61);
                Rectangle helpclickrect = new Rectangle((int)playButtonRect.X, (int)playButtonRect.Y, 125, 61);
                Rectangle quitclickrect = new Rectangle((int)playButtonRect.X, (int)playButtonRect.Y, 125, 61);

                if(MouseClickRect.Intersects(playButtonRect))
                {
                    currentGameState = "Game";
                }
                else if(MouseClickRect.Intersects(helpButtonRect))
                {
                    currentGameState = "Help";
                }
                else if (MouseClickRect.Intersects(quitButtonRect))
                {
                    QuitGame();
                }
            }
        }

        void QuitGame()
        {
            Environment.Exit(0);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            

            if (currentGameState == "MainMenu")
            {
                //Todo list
                //---------
                //Implement scrolling background
                //Draw text (after background to make sure it draws correctly)

                //Drawing background colour
                spriteBatch.Draw(blankTexture, new Rectangle(0,0,700,700), Color.GreenYellow);

                //Drawing logo
                spriteBatch.Draw(logoTexture, new Rectangle(179, 0, 341, 240), Color.White);

                //Drawing menu buttons
                spriteBatch.Draw(playTextTexture, playButtonRect, Color.White);
                spriteBatch.Draw(helpTextTexture, helpButtonRect, Color.White);
                spriteBatch.Draw(quitTextTexture, quitButtonRect, Color.White);
            }
            else if (currentGameState == "Game")
            {
                //map.GetLength(1) gets the x length
                //map.GetLength(0) gets the y length
                //This for loop draws the grid
                for (int xAxis = 0; xAxis < map.GetLength(0); xAxis++)
                {
                    for (int yAxis = 0; yAxis < map.GetLength(1); yAxis++)
                    {
                        switch (map[xAxis, yAxis])
                        {
                            case 0:
                                if(colouredMap[xAxis, yAxis] == 1)
                                    spriteBatch.Draw(dirtTexture, new Rectangle(yAxis * 50, xAxis * 50, 50, 50), Color.Black);
                                else
                                    spriteBatch.Draw(dirtTexture, new Rectangle(yAxis * 50, xAxis * 50, 50, 50), Color.White);
                                break;
                            case 1:
                                if (colouredMap[xAxis, yAxis] == 1)
                                    spriteBatch.Draw(grassTexture, new Rectangle(yAxis * 50, xAxis * 50, 50, 50), Color.Black);
                                else
                                    spriteBatch.Draw(grassTexture, new Rectangle(yAxis * 50, xAxis * 50, 50, 50), Color.White);
                                break;
                            case 2:
                                if (colouredMap[xAxis, yAxis] == 1)
                                    spriteBatch.Draw(sandTexture, new Rectangle(yAxis * 50, xAxis * 50, 50, 50), Color.Black);
                                else
                                    spriteBatch.Draw(sandTexture, new Rectangle(yAxis * 50, xAxis * 50, 50, 50), Color.White);
                                break;
                            case 3:
                                if (colouredMap[xAxis, yAxis] == 1)
                                    spriteBatch.Draw(snowTexture, new Rectangle(yAxis * 50, xAxis * 50, 50, 50), Color.Black);
                                else
                                    spriteBatch.Draw(snowTexture, new Rectangle(yAxis * 50, xAxis * 50, 50, 50), Color.White);
                                break;
                            case 4:
                                if (colouredMap[xAxis, yAxis] == 1)
                                    spriteBatch.Draw(stoneTexture, new Rectangle(yAxis * 50, xAxis * 50, 50, 50), Color.Black);
                                else
                                    spriteBatch.Draw(stoneTexture, new Rectangle(yAxis * 50, xAxis * 50, 50, 50), Color.White);
                                break;
                            case 5:
                                if (colouredMap[xAxis, yAxis] == 1)
                                    spriteBatch.Draw(waterTexture, new Rectangle(yAxis * 50, xAxis * 50, 50, 50), Color.Black);
                                else
                                    spriteBatch.Draw(waterTexture, new Rectangle(yAxis * 50, xAxis * 50, 50, 50), Color.White);
                                break;
                        }
                    }
                }
                //Drawing player
                spriteBatch.Draw(playerTexture, new Rectangle(player.returnXTile * 50, player.returnYTile * 50, 50, 50), Color.White);
            }
            else if(currentGameState == "Help")
            {
                spriteBatch.Draw(blankTexture, new Rectangle(0, 0, 700, 700), Color.Aqua);
                spriteBatch.Draw(HelpInfoTexture, new Rectangle(0, 0, 700, 700), Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
        public static GameStates gameState { get; set; }
    }
}