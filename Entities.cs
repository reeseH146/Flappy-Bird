using System.Reflection.Metadata.Ecma335;
using Raylib_cs;

namespace FlappyBird.Entities
{
    public class PlayerBird
    {
        // Hitbox - used for positioning and collision
        public Rectangle HitBox;
        // More detailed positioning and movement
        /* public int PosYSnap; // Snapshot of YPos when jump
        public int PosYDiff; // Difference of true YPos from snapshot
        public int PosYMove; // Tracks Y move
        public double CoolDown;*/
        public bool Jump; // True for up, false for down
        private float PosYMoveFall;// 360 / 60; // Rate of change of Y Move // Moves 360 pixels in 60 fps 
        private float PosYMoveJump;
        // Image/render
        private Image BirdImage;
        private Texture2D BirdTex;

        // Player class constructor
        public PlayerBird(int ScreenX, int ScreenY)
        {
            // Position and dimensions
            HitBox = new Rectangle
            (
                (float)((ScreenX * 0.075) - (ScreenX * 0.05 * 0.5)),
                (float)((ScreenY * 0.5) - (ScreenX * 0.05)), // No idea why this centres perfectly, I think my mind is going bonkers
                (float)(ScreenX * 0.1),
                (float)(ScreenX * 0.1)
            );
            // Movement
            Jump = false;
            PosYMoveFall = (float)(ScreenY * 0.004);
            PosYMoveJump = (float)(ScreenY * 0.25);
            /*PosYCurrent = Convert.ToInt16(ScreenY * 0.5);
            PosYSnap = Convert.ToInt16(ScreenY * 0.5);
            PosYMove = 0;
            MoveDirection = false;*/
            // Image
            BirdImage = Raylib.LoadImage("Assets/Bird.png"); // Loads custom image
            Raylib.ImageResizeNN(ref BirdImage, (int)(ScreenX * 0.1), (int)(ScreenX * 0.1)); // Resizes custom image to fix hitbox
            BirdTex = Raylib.LoadTextureFromImage(BirdImage); // Creates texture from image
            Raylib.UnloadImage(BirdImage);
        }

        public void Draw()
        {
            if (Raylib.IsTextureValid(BirdTex)) Raylib.DrawTexture(BirdTex, (int)HitBox.X, (int)HitBox.Y, Color.White); // Attempts to draw custom image
            else Raylib.DrawRectangleRec(HitBox, Color.Orange); // Draws default image if custom fails
        }

        // Called to update movement
        // Player changes state of moving up or down to affect method process

        // Depends on state to move up or down
        // False for down, variable takes snapshot of PosY when jump pressed (default pos at start up), another variable tracks unit of change which is parsed into a -(1/1.5 * x)^2 (wider parabola drifting down) allowing the bird to move down
        // True for up, var...snapshot..., another variable tracks unit of change which is parsed into a 9x^2 (steeper parabola going up) allowing the bird to move down, if the unit of change exceeds limit (300?) then flips state to decreasing, resets unit of change and snapshots YPos
        public void Move()
        {
            if (Jump)
            {
                HitBox.Y -= PosYMoveJump;
                Jump = false;
            }
            else HitBox.Y += PosYMoveFall;
            
            /*if (MoveDirection && ((Raylib.GetTime() - CoolDown) >= 0.4))
            {
                CoolDown = Raylib.GetTime();
                PosYCurrent -= 275;
                MoveDirection = false;
            }
            else
            {
                PosYCurrent += 6;
                HitBox[2] = PosYCurrent;
                HitBox[3] = PosYCurrent + 100;
            }*/

            /*if (MoveDirection) {
                PosYDiff = 9 * (PosYMove ^ 2); // Recalculates offset from snapshot
                if (PosYDiff >= (PosYMoveChange * 180)) { // Checks whether to move down. If offset > Rate of change (every change) * times changed for 3 seconds
                    PosYMove = 0;
                    MoveDirection = false;
                }
                else {
                    PosYCurrent = PosYSnap + PosYDiff; // Recalculates true position
                }
            }
            else {
                PosYDiff = Convert.ToInt16(1/1.5 * (PosYMove ^ 2)); // Recalculates offset from snapshot
                PosYCurrent = PosYSnap + PosYDiff; // Recalculates true position
            }
            PosYMove += PosYMoveChange;*/
        }

        // Player vertical collision, Bird pos centred so must account for actual dimensions
        public bool CeilingCollision(int ScreenY)
        {
            if ((0 <= HitBox.Y) && ((HitBox.Y + HitBox.Height) <= ScreenY)) return false;
            else return true;
        }

        // Unloads all information
        public void Unload()
        {
            Raylib.UnloadImage(BirdImage);
            Raylib.UnloadTexture(BirdTex);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
    // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- //
    // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### // --- ### --- // ### --- ### //
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Allows a pipe to be made                                - Pipe position and textures
    // Moves along screen                                      - Method to decrement X
    // Resets back when out of screen                          - Method to check X and reset
    // Checks if made contact with player and quits game if so - Hitbox requires rect
    public class PipeObject
    {
        // Position
        int PosX;
        int[] PosY;
        // Movement
        int Speed;
        // Texture
        Texture2D[] PipeTex = new Texture2D[2];
        // Collision
        int[] HitBox;

        public PipeObject(int ScreenPosX, int YEnd)
        {
            PosX = ScreenPosX;
            PosY = [0 - YEnd, 400 - YEnd]; // Transforms...
            Speed = 5;
            Image PipeImg = Raylib.LoadImage("C:/Users/Hi-bu/Reese/VSC/Flappy-Bird/Assets/LocalImages/Pipe.png");
            if (Raylib.IsImageValid(PipeImg))
            {
                PipeImg = Raylib.LoadImage("C:/Users/Hi-bu/Reese/VSC/Flappy-Bird/Assets/Pipe.png");
            }
            Raylib.ImageResize(ref PipeImg, 100, 400);
            PipeTex[0] = Raylib.LoadTextureFromImage(PipeImg);
            HitBox = [PosX, PosX + 100, PosY[0], PosY[1],/*Bottom Rect Y ->*/PosY[1] + 400, PosY[1] + 800];
        }

        // Literally draws the pipe textures onto the screen
        public void Draw()
        {
            Raylib.DrawTexture(PipeTex[0], PosX, PosY[0], Color.White); // Top pipe
            Raylib.DrawTexture(PipeTex[0], PosX, PosY[1], Color.White); // Bottom pipe
        }

        // Decrements x position of pipe to move, checks if they need to reset
        public void Move(int ScreenX)
        {
            PosX -= Speed;
            HitBox[0] = PosX;
            if (PosX < -290)
                PosX = Convert.ToInt16(ScreenX * 1.1);
            HitBox[0] = PosX;
        }

        // Collision check with player bird
        // Uses guard clauses to return early if there is overlap
        //
        // Pipe HitBox = [PosX, PosX + 0, PosY[0], PosY[0] + 0,/*Bottom Rect ->*/PosY[1], PosY[1] + 0];
        // Bird HitBox = [PosX, PosX + 100, PosYCurrent, PosYCurrent + 100];
        public bool Collision(int[] BirdHitBox)
        {
            // Compares bird LeftX and RightX within range of pipes 
            // If (Bird Left X within Pipe X range) or (Bird Right X within Pipe X range)
            if (((HitBox[0] <= BirdHitBox[0]) && (BirdHitBox[0] <= HitBox[1])) || ((HitBox[0] <= BirdHitBox[1]) && (BirdHitBox[1] <= HitBox[1])))
            {
                // Compares bird Top Y and Bottom Y within range of top pipe
                // If (Bird Top Y within Top Pipe Y range) or (Bird Bottom Y within Top Pipe Y range)
                if (((HitBox[2] <= BirdHitBox[2]) && (BirdHitBox[2] <= HitBox[3])) || ((HitBox[2] <= BirdHitBox[3]) && (BirdHitBox[3] <= HitBox[3])))
                {
                    return true;
                }
                // Compares bird Top Y and Bottom Y within range of bottom pipe
                // If (Bird Top Y within Top Pipe Y range) or (Bird Bottom Y within Top Pipe Y range)
                else if (((HitBox[4] <= BirdHitBox[2]) && (BirdHitBox[2] <= HitBox[5])) || ((HitBox[4] <= BirdHitBox[3]) && (BirdHitBox[3] <= HitBox[5])))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}