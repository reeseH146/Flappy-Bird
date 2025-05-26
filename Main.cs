using Raylib_cs;
using NativeFileDialogNET; // For opening file explorer to select images for custom skins
using FlappyBird.Entities;
using System.Runtime.CompilerServices;

public class Scenes
{
    enum Scene { Menu, Game, Skin, Settings }
    int ScreenX = 1700; //Convert.ToInt16(1920 * 0.3);//Convert.ToInt16(Raylib.GetMonitorWidth(Raylib.GetCurrentMonitor()) * WindowScale);
    int ScreenY = 900; //Convert.ToInt16(1080 * 0.3);//Convert.ToInt16(Raylib.GetMonitorHeight(Raylib.GetCurrentMonitor()) * WindowScale);

    Scene MenuState = Scene.Menu; // 0:Menu, 1:Game, 2:Skins, 4:Settings

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
    // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- //
    // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static void Main(int ScreenX, int ScreenY, Scene MenuState)
    {
        // Loads Variables And Assets
        Console.WriteLine("Flappy Bird program loading...");
        Raylib.InitWindow(ScreenX, ScreenY, "Flappy Bird");

        PlayerBird FlappyPlayer = new PlayerBird(100, Convert.ToInt16(ScreenY * 0.5)); // Creates bird object
        Pipe[] Pipes = new Pipe[5]; // Maximum of 10 pipes in game
        Raylib.SetTargetFPS(60);
        // Pipe Pipes = new Pipe(1000, 580 + 300);
        int XPos = 500;
        int YEnd = 50;
        for (int i = 0; i < 5; i++)
        { // Creates each set of pipes
            Pipes[i] = new Pipe(XPos, YEnd);
            XPos += 350;
            YEnd += 50;
        }

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
            switch (MenuState)
            {
                case Scene.Menu:
                    Menu(ref ScreenX, ref ScreenY, ref MenuState);
                    break;
                case Scene.Game:
                    Game(ref ScreenX, ref ScreenY, ref FlappyPlayer, ref MenuState);
                    break;
                case Scene.Skin:
                    Skin(ref FlappyPlayer, ref MenuState);
                    break;
                case Scene.Settings:
                    Settings(ref ScreenX, ref ScreenY, ref MenuState);
                    break;
            }
            Raylib.CloseWindow();
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
    // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- //
    // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static void Menu(ref int ScreenX, ref int ScreenY, ref Scene MenuState)
    {
        int[,] HitBox = { { ScreenX }, { 0 }, { 0 }, { 0 } }; // Contains XY coordinate on TL, BR corner of font rect
        bool MenuStay = true;
        while (MenuStay)
        {
            // Updates the game events and graphics
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.SkyBlue); // Sky background
            Raylib.DrawRectangle(0, Convert.ToInt16(ScreenY * 0.8), ScreenX, Convert.ToInt16(ScreenY * 0.8), Color.Green); // Grass floor
            /*Raylib.DrawText(char("Start"), ScreenX/2, ScreenY/3/4, 25, [45, 45, 45]);
            Draw options to start, skin, settings, quit
            */
            Raylib.EndDrawing();
            // Player input, selecting option in menu
            System.Numerics.Vector2 MousePos = Raylib.GetMousePosition();
            if (Raylib.IsMouseButtonPressed(0))
            {
                if (HitBox[0, 0] > MousePos[0])
                {
                    Console.WriteLine(2);
                }
            }
        }
        MenuState = Scene.Menu;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
    // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- //
    // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Actual game
    public static void Game(ref int ScreenX, ref int Screen, ref PlayerBird FlappyBird, ref Scene MenuState)
    {
        MenuState = Scene.Menu;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
    // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- //
    // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Shows default skins, users can add skins to folder which shows up or import skins
    public static void Skin(ref PlayerBird FlappyBird, ref Scene MenuState)
    {
        MenuState = Scene.Menu;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
    // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- //
    // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Allows changing on volumes/sounds/window size and variables (spawn, pipe frequency and displacement)
    public static void Settings(ref int ScreenX, ref int ScreenY, ref Scene MenuState)
    {
        MenuState = Scene.Menu;
    }
}

// TODO: Fix pipe creation class and handling in main
// TODO: Multiple pipes
// TODO: Implement clouds
// TODO: Implement score
// TODO: Menu
// TODO: Skins