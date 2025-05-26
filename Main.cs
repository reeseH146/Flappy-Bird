using Raylib_cs;
using NativeFileDialogNET; // For opening file explorer to select images for custom skins
using FlappyBird.Entities;

public class Scenes {
    int ScreenX = 1700; //Convert.ToInt16(1920 * 0.3);//Convert.ToInt16(Raylib.GetMonitorWidth(Raylib.GetCurrentMonitor()) * WindowScale);
    int ScreenY = 900; //Convert.ToInt16(1080 * 0.3);//Convert.ToInt16(Raylib.GetMonitorHeight(Raylib.GetCurrentMonitor()) * WindowScale);
    public static void Main(int ScreenX, int ScreenY) {
        // Loads Variables And Assets
        Console.WriteLine("Flappy Bird program loading...");
        Raylib.InitWindow(ScreenX, ScreenY, "Flappy Bird");

        PlayerBird FlappyPlayer = new PlayerBird(100, Convert.ToInt16(ScreenY * 0.5)); // Creates bird object
        Pipe[] Pipes = new Pipe[5]; // Maximum of 10 pipes in game
        Raylib.SetTargetFPS(60);
        // Pipe Pipes = new Pipe(1000, 580 + 300);
        int XPos = 500;
        int YEnd = 50;
        for (int i = 0; i < 5; i++) { // Creates each set of pipes
            Pipes[i] = new Pipe(XPos, YEnd);
            XPos += 350;
            YEnd += 50;
        }

        int MenuState = 0; // 0:Menu, 1:Game, 2:Skins, 4:Settings

        Console.WriteLine("All program components loaded.");

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
        // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- //
        // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Main Loop
        //bool GameOn = true;
        while (!Raylib.WindowShouldClose())
        {
            switch (MenuState) {
                case 0:
                    Menu();
                    break;
                case 1:
                    Game();
                    break;
                case 2:
                    Skin();
                    break;
                case 3:
                    Settings();
                    break;
            }
            // Updates the game events and graphics
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.SkyBlue);
            Raylib.DrawRectangle(0, Convert.ToInt16(ScreenY * 0.8), ScreenX, Convert.ToInt16(ScreenY * 0.8), Color.Green);
            FlappyPlayer.Draw();
            foreach (Pipe Pip in Pipes)
            {
                Pip.Draw();
            }
            Raylib.EndDrawing();

            // Player input
            if (Raylib.IsKeyPressed(KeyboardKey.W) || Raylib.IsKeyPressed(KeyboardKey.Space) || Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                //FlappyPlayer.PosYMove = 0;
                FlappyPlayer.MoveDirection = true;
            }
            else
            {
                FlappyPlayer.Move();
                if (FlappyPlayer.Collision(ScreenY))
                {
                    //GameOn = false;
                    Raylib.CloseWindow();

                }
            }
            foreach (Pipe Pip in Pipes)
            {
                Pip.Move(1700);
                if (Pip.Collision(FlappyPlayer.HitBox))
                {
                    //GameOn = false;
                    Raylib.CloseWindow();
                }
            }
        }
        Raylib.CloseWindow();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
    // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- //
    // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static void Menu(int ScreenX, int ScreenY, ref PlayerBird FlappyPlayer)
    {
        bool MenuStay = true;
        while (MenuStay) { 
            // Updates the game events and graphics
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.SkyBlue);
            Raylib.DrawRectangle(0, Convert.ToInt16(ScreenY * 0.8), ScreenX, Convert.ToInt16(ScreenY * 0.8), Color.Green);
            FlappyPlayer.Draw();
            foreach (Pipe Pip in Pipes)
            {
                Pip.Draw();
            }
            Raylib.EndDrawing();
        } 
    }
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
    // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- //
    // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Actual game
    public static void Game()
    {
        Console.WriteLine(1);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
    // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- //
    // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Shows default skins, users can add skins to folder which shows up or import skins
    public static void Skin()
    {
        Console.WriteLine(1);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
    // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- //
    // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Allows changing on volumes/sounds and variables (spawn, pipe frequency and displacement)
    public static void Settings()
    {
        Console.WriteLine(1);
    }
}

// TODO: Fix pipe creation class and handling in main
// TODO: Multiple pipes
// TODO: Implement clouds
// TODO: Implement score
// TODO: Menu
// TODO: Skins