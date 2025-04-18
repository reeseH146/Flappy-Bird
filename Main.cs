﻿using Raylib_cs;
using FlappyBird.Entities;
using System.Net.Sockets;

public class Program {
    public static void Main() {
        // Loads Variables And Assets
        Console.WriteLine("Flappy Bird program loading...");
        int ScreenX = 1700; //Convert.ToInt16(1920 * 0.3);//Convert.ToInt16(Raylib.GetMonitorWidth(Raylib.GetCurrentMonitor()) * WindowScale);
        int ScreenY = 900; //Convert.ToInt16(1080 * 0.3);//Convert.ToInt16(Raylib.GetMonitorHeight(Raylib.GetCurrentMonitor()) * WindowScale);
        Raylib.InitWindow(ScreenX, ScreenY, "Flappy Bird");

        PlayerBird FlappyPlayer = new PlayerBird(100, Convert.ToInt16(ScreenY * 0.5)); // Creates bird object
        //Pipe[] Pipes = new Pipe[1]; // Maxium of 10 pipes in game
        Raylib.SetTargetFPS(60);
        Pipe Pipes = new Pipe(1000, 580 + 300);
        /*int[] PipeX = [i for (i=0; )];
        for (int i = 0; i == 10; i++) { // Creates each set of pipes
        }*/
        Console.WriteLine("All program components loaded.");

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
        // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- //
        // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        // Main Loop
        bool GameOn = true;
        while (GameOn) {
            // Updates the game events and graphics
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.SkyBlue);
            Raylib.DrawRectangle(0, Convert.ToInt16(ScreenY * 0.8), ScreenX, Convert.ToInt16(ScreenY * 0.8), Color.Green);
            FlappyPlayer.Draw();
            Pipes.Draw();
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
            /*foreach (Pipe Pip in Pipes) {
                Pip.Move(ScreenX);
                if (Pip.Collision(FlappyPlayer.HitBox)) {
                    Raylib.CloseWindow();
                }
                else {
                    Pip.Draw();
                }
            }*/
            Pipes.Move(ScreenX);
            if (Pipes.Collision(FlappyPlayer.HitBox)) {
                    Console.WriteLine("Game over and closing");
                    GameOn = false;
                }
        }
        Raylib.CloseWindow();
    }
}

// TODO: Fix pipe creation class and handling in main
// TODO: Multiple pipes
// TODO: Implement clouds
// TODO: Implement score
// TODO: Menu
// TODO: Skins