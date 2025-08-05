using Raylib_cs;
using FlappyBird.Entities;
using System.Numerics;
using NativeFileDialogNET; // For opening file explorer to select images for custom skins
using System.Diagnostics; // No idea how this got here

namespace Game
{
    public enum Scene { Menu, Game, Skin, Settings, Close } // Represent the current event loop to choose or quits
    public static class Scenes
    {
        // Initialises global resources
        private static Scene MenuState = Scene.Menu; // 0:Menu, 1:Game, 2:Skins, 3:Settings, 4:Close. Sets Menu as default
        private static int ScreenX = 16 * 70; // Values used to set max window size, reconfig separately later
        private static int ScreenY = 9 * 70;
        private static PlayerBird FlappyPlayer = new PlayerBird(100, Convert.ToInt16(ScreenY * 0.5)); // Creates bird object
        private static Pipe[] Pipes = new Pipe[5]; // Write code for max pipes dynamically generated based on screen size

        // Text informations

        private static Font MonospaceFont;
        private static string[] GameText = new string[]
        {
            "Flappy Bird Game", // 0
            "Start",
            "Skins",
            "Settings",
            "Close",
            "Return",           // 5 - Return button used in skins and settings menu
            "Skins",            // 6 - Skins menu
            "Settings"          // 7 - Settings menu
        };
        private static int[] TextSize = new int[]
        {
            95,                 // 0
            60,                 
            60,                 
            60,                 
            60,                 
            50,                 // 5 - Return button used in skins and settings menu
            95,                 // 6 - Skins menu
            95                  // 7 - Settings menu
        };
        private static double[,] TextRelPos = new double[,] // Used to calculate text rect pos relative to the centre of the rect
        {
            {0.5, 0.25 * 0.5},  // 0
            {0.5, 0.3}, 
            {0.5, 0.45},
            {0.5, 0.60},
            {0.5, 0.75},
            {0.90, 0.925},       // 5 - Return button used in skins and settings menu
            {0.5, 0.25 * 0.5},  // 6 - Skins menu
            {0.5, 0.25 * 0.5},  // 7 - Settings menu
        };
        private static Rectangle[] TextRect = new Rectangle[GameText.Count()]; // Used for collision and positioning when drawing, possibly also text background
        
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
        // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- //
        // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        private static void Main()
        {
            // Initialises game resources
            Console.WriteLine("Flappy Bird program loading...");
            Raylib.SetWindowMinSize(ScreenX, ScreenY);
            Raylib.InitWindow(ScreenX, ScreenY, "Flappy Bird"); // Creates a window based on max monitor size
            Raylib.SetTargetFPS(60);
            MonospaceFont = Raylib.LoadFontEx("Assets/Fonts/RobotoMono-Regular.ttf", 100, null, 0); // RL default font if not found
            Raylib.SetTextureFilter(MonospaceFont.Texture, TextureFilter.Bilinear);
            // Debug.WriteLine((MonospaceFont.Texture.Id == 0) ? "Font not loaded" : "Font loaded");
            // Pipe Pipes = new Pipe(1000, 580 + 300);
            int XPos = 500;
            int YEnd = 50;
            for (int i = 0; i < 5; i++)
            { // Creates each set of pipes
                Pipes[i] = new Pipe(XPos, YEnd);
                XPos += 350;
                YEnd += 50;
            }
            // Initialising text rect
            for (int i = 0; i < GameText.Count(); i++)
            {
                TextRect[i] = new Rectangle
                (
                    // Drawing rects start from the TL in RL_CS but I want to draw from middle point
                    // Calculate TL pos relative to rect centre pos : TL = RectCentre - HalfDimension (X/Y)
                    // Other wording : Shift XY of TL pos by half dimensions of rect XY to get centre pos
                    new Vector2 // TL pos
                    (
                        (int)((ScreenX * TextRelPos[i, 0]) - (Raylib.MeasureTextEx(MonospaceFont, GameText[i], TextSize[i], 4)[0] / 2)),
                        (int)((ScreenY * TextRelPos[i, 1]) - (Raylib.MeasureTextEx(MonospaceFont, GameText[i], TextSize[i], 4)[1] / 2))
                    ),
                    Raylib.MeasureTextEx(MonospaceFont, GameText[i], TextSize[i], 4) // Width, height
                );
            }
            // Game can start in main menu
            Console.WriteLine("All program components loaded.");

            // Runs game until closing
            EventManager();
            // Closes game
            Raylib.CloseWindow();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
        // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- //
        // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        private static void EventManager()
        {
            bool ExitPressed = false;
            while (!Raylib.WindowShouldClose() && !ExitPressed) // Runs scenes until window closed or user closes game
            {
                switch (MenuState) // Chooses a eventloop or quits based on the current scene
                {
                    case Scene.Menu:
                        MenuEvent();
                        break;
                    case Scene.Game:
                        GameEvent();
                        break;
                    case Scene.Skin:
                        SkinEvent();
                        break;
                    case Scene.Settings:
                        SettingsEvent();
                        break;
                    case Scene.Close:
                        Console.WriteLine(4);
                        ExitPressed = true;
                        break;
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
        // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- //
        // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Tried making arrays storing text positions and got runtime errors that don't sound too good so guess I'm putting off sustainable code for fonts/texts
        private static void MenuEvent()
        {
            // Updates the game events and graphics
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.SkyBlue);
            Raylib.DrawRectangle(0, (int)(ScreenY * 0.8), ScreenX, ScreenY, Color.Green);
            for (int i = 0; i < 5; i++)
            {
                Raylib.DrawTextEx(MonospaceFont, GameText[i], new Vector2(TextRect[i].X, TextRect[i].Y), TextSize[i], 4, Color.Black);
            }
            FlappyPlayer.Draw();
            /*foreach (Pipe in PipeContainer)
            {
                Pipe.Draw
            }*/
            Raylib.EndDrawing();

            // Check if player clicks on mouse button
            if (Raylib.IsMouseButtonReleased(0))
            {
                Vector2 MousePos = Raylib.GetMousePosition();
                for (int i = 1; i < 5; i++) // Looks through each text rect
                {
                    if (Raylib.CheckCollisionPointRec(MousePos, TextRect[i])) // Checks if mouse pos is within text rect
                    {
                        MenuState = (Scene)i; // Changes Menu state to respective state based button pressed
                    }
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
        // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- //
        // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Actual game
        private static void GameEvent()
        {
            Console.WriteLine(1);
            MenuState = Scene.Close;
            
            // Runs event loop


            // Main Loop
            /*bool GameOn = true;
            while (GameOn) {
                // Updates the game events and graphics
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.SkyBlue);
                Raylib.DrawRectangle(0, Convert.ToInt16(ScreenY * 0.8), ScreenX, Convert.ToInt16(ScreenY * 0.8), Color.Green);
                FlappyPlayer.Draw();
                foreach (Pipe Pip in Pipes) {
                        Pip.Draw();
                }
                //Pipes.Draw();
                Raylib.EndDrawing();

                // Checks for player movement
                if (Raylib.IsKeyPressed(KeyboardKey.W) || Raylib.IsKeyPressed(KeyboardKey.Space) || Raylib.IsMouseButtonPressed(MouseButton.Left)) {
                    //FlappyPlayer.PosYMove = 0;
                    FlappyPlayer.MoveDirection = true;
                }
                else {
                    FlappyPlayer.Move();
                    if (FlappyPlayer.Collision(ScreenY)) {
                        GameOn = false;
                    }
                }
                foreach (Pipe Pip in Pipes) {
                    Pip.Move(1700);
                    if (Pip.Collision(FlappyPlayer.HitBox)) {
                        GameOn = false;
                        //Raylib.CloseWindow();
                    }
                }*/

            //Pipes.Move(ScreenX);
            /*if (Pipes.Collision(FlappyPlayer.HitBox)) {
                    Console.WriteLine("Game over and closing");
                    GameOn = false;
                }*/
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
        // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- //
        // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Shows default skins, users can add skins to folder which shows up or import skins
        private static void SkinEvent()
        {
            // Updates the game events and graphics
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.SkyBlue);
            Raylib.DrawRectangle(0, (int)(ScreenY * 0.8), ScreenX, ScreenY, Color.Green);
            //for (int i = 0; i < GameText.Count(); i++) // Draw multiple text
            Raylib.DrawTextEx(MonospaceFont, GameText[6], new Vector2((int)TextRect[6].X, (int)TextRect[6].Y), TextSize[6], 4, Color.Black);
            Raylib.DrawTextEx(MonospaceFont, GameText[5], new Vector2((int)TextRect[5].X, (int)TextRect[5].Y), TextSize[5], 4, Color.Black);
            FlappyPlayer.Draw();
            /*foreach (Pipe in PipeContainer)
            {
                Pipe.Draw
            }*/
            Raylib.EndDrawing();

            // Check if player clicks on mouse button
            if (Raylib.IsMouseButtonReleased(0))
            {
                Vector2 MousePos = Raylib.GetMousePosition();
                //for (int i = 1; i < 5; i++) // Looks through each text rect
                if (Raylib.CheckCollisionPointRec(MousePos, TextRect[5])) // Checks if mouse pos is within text rect
                {
                    MenuState = Scene.Menu; // Changes Menu state to respective state based button pressed
                    Console.WriteLine($"Menu Button Pressed at {MousePos}");
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
        // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- //
        // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Allows changing on volumes/sounds and variables (spawn, pipe frequency and displacement)
        private static void SettingsEvent()
        {
            // Updates the game events and graphics
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.SkyBlue);
            Raylib.DrawRectangle(0, (int)(ScreenY * 0.8), ScreenX, ScreenY, Color.Green);
            //for (int i = 0; i < GameText.Count(); i++) // Draw multiple text
            Raylib.DrawTextEx(MonospaceFont, GameText[7], new Vector2((int)TextRect[7].X, (int)TextRect[7].Y), TextSize[7], 4, Color.Black);
            Raylib.DrawTextEx(MonospaceFont, GameText[5], new Vector2((int)TextRect[5].X, (int)TextRect[5].Y), TextSize[5], 4, Color.Black);
            FlappyPlayer.Draw();
            /*foreach (Pipe in PipeContainer)
            {
                Pipe.Draw
            }*/
            Raylib.EndDrawing();

            // Check if player clicks on mouse button
            if (Raylib.IsMouseButtonReleased(0))
            {
                Vector2 MousePos = Raylib.GetMousePosition();
                //for (int i = 1; i < 5; i++) // Looks through each text rect
                if (Raylib.CheckCollisionPointRec(MousePos, TextRect[5])) // Checks if mouse pos is within text rect
                {
                    MenuState = Scene.Menu; // Changes Menu state to respective state based button pressed
                }
            }
        }
    }
}

// TODO: Fix pipe creation class and handling in main
// TODO: Multiple pipes
// TODO: Implement clouds
// TODO: Implement score
// TODO: Menu
// TODO: Skins

// TODO: Line 16