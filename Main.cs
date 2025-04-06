using System;
using Raylib_cs;

namespace Program {
    public class Program {
        public static void Main() { // Main program where event loop runs
            Console.WriteLine("Flappy Bird program loading...");
            // Initialises vairables
            int ScreenX = 640;
            int ScreenY = 360;
            // Initialises assets and objects

            // Initialises Raylib stuff
            Raylib.InitWindow(ScreenX, ScreenY, "Flappy Bird");
            Raylib.SetTargetFPS(120);
            Console.WriteLine("All program components loaded.");
            while (Raylib.WindowShouldClose()) {
                //if
            }
            Raylib.CloseWindow();
        }
    }
}