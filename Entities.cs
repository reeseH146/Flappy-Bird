namespace FlappyBird.Entities{
    using Raylib_cs;
    public class PlayerBird {
        // Position
        public int PosX;
        // Movement
        public int PosYCurrent;
        public int PosYSnap; // Snapshot of YPos when jump
        public int PosYDiff; // Difference of true YPos from snapshot
        public int PosYMove; // Tracks Y move
        public const int PosYMoveChange = 360/60; // Rate of change of Y Move // Moves 360 pixels in 60 fps
        public bool MoveDirection; // True for up, false for down
        public double CoolDown;
        // Image
        public Image BirdTex;
        public Texture2D BirdImage;
        // Collision object
        public int[] HitBox;

        // Player class constructor
        public PlayerBird(int ScreenX, int ScreenY) {
            // Position
            PosX = ScreenX;
            // Movement
            PosYCurrent = Convert.ToInt16(ScreenY * 0.5);
            PosYSnap = Convert.ToInt16(ScreenY * 0.5);
            PosYMove = 0;
            MoveDirection = false;
            // Image
            BirdTex = Raylib.LoadImage("C:/Users/Hi-bu/Reese/VSC/Flappy-Bird/Assets/LocalImages/Bird.png");
            Raylib.ImageResizeNN(ref BirdTex, 100, 100);   
            BirdImage = Raylib.LoadTextureFromImage(BirdTex);
            // Collision
            HitBox = [PosX, PosX + 100, PosYCurrent, PosYCurrent + 100];
        }

        public void Draw() {
            Raylib.DrawTexture(BirdImage, PosX, PosYCurrent, Color.White);
        }

        // Called to update movement
        // Player changes state of moving up or down to affect method process

        // Depends on state to move up or down
        // False for down, variable takes snapshot of PosY when jump pressed (default pos at start up), another variable tracks unit of change which is parsed into a -(1/1.5 * x)^2 (wider parabola drifting down) allowing the bird to move down
        // True for up, var...snapshot..., another variable tracks unit of change which is parsed into a 9x^2 (steeper parabola going up) allowing the bird to move down, if the unit of change exceeds limit (300?) then flips state to decreasing, resets unit of change and snapshots YPos
        public void Move() {
            if (MoveDirection && ((Raylib.GetTime() - CoolDown) >= 0.4)) {
                CoolDown = Raylib.GetTime();
                PosYCurrent -= 275;
                MoveDirection = false;
            }
            else {
                PosYCurrent += 6;
                HitBox[2] = PosYCurrent;
                HitBox[3] = PosYCurrent + 100;
            }
            /*if (MoveDirection) {
                PosYDiff = 9 * (PosYMove ^ 2); // Recalculates offset from snapshot
                if (PosYDiff >= (PosYMoveChange * 180)) { // Checks whether to move dowd. If offset > Rate of change (every change) * times changed for 3 seconds
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

        // Player Vertical Collision
        public bool Collision(int ScreenY) {
            if ((PosYCurrent < 0) || (ScreenY < PosYCurrent)) {
                return true;
            }
            return false;
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
    public class Pipe {
        // Position
        int PosX;
        int[] PosY;
        // Movement
        int Speed;
        // Texture
        Texture2D[] PipeTex = new Texture2D[2];
        // Collision
        int[] HitBox;

        public Pipe(int ScreenPosX, int YEnd) {
            PosX = ScreenPosX;
            PosY = [0, YEnd]; // Transforms...
            Speed = 5;
            Image PipeImg = Raylib.LoadImage("C:/Users/Hi-bu/Reese/VSC/Flappy-Bird/Assets/LocalImages/Pipe.png");
            PipeTex[0] = Raylib.LoadTextureFromImage(PipeImg);
            HitBox = [PosX, PosX + 290, PosY[0], PosY[0] + 480,/*Bottom Rect ->*/PosY[1], PosY[1] + 580];
        }

        // Literally draws the pipe textures onto the screen
        public void Draw() {
            Raylib.DrawTexture(PipeTex[0], PosX, PosY[0], Color.White); // Top pipe
            Raylib.DrawTexture(PipeTex[0], PosX, PosY[1], Color.White); // Bottom pipe
        }

        // Decrements x position of pipe to move, checks if they need to reset
        public void Move(int ScreenX) {
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
        public bool Collision(int[] BirdHitBox) {
            // Compares bird LeftX and RightX within range of pipes 
            // If (Bird Left X within Pipe X range) or (Bird Right X within Pipe X range)
            if (((HitBox[0] <= BirdHitBox[0]) && (BirdHitBox[0] <= HitBox[1])) || ((HitBox[0] <= BirdHitBox[1]) && (BirdHitBox[1] <= HitBox[1]))) {
                // Compares bird Top Y and Bottom Y within range of top pipe
                // If (Bird Top Y within Top Pipe Y range) or (Bird Bottom Y within Top Pipe Y range)
                if (((HitBox[2] <= BirdHitBox[2]) && (BirdHitBox[2] <= HitBox[3])) || ((HitBox[2] <= BirdHitBox[3]) && (BirdHitBox[3] <= HitBox[3]))) {
                    return true;
                }
                // Compares bird Top Y and Bottom Y within range of bottom pipe
                // If (Bird Top Y within Top Pipe Y range) or (Bird Bottom Y within Top Pipe Y range)
                else if (((HitBox[4] <= BirdHitBox[2]) && (BirdHitBox[2] <= HitBox[5])) || ((HitBox[4] <= BirdHitBox[3]) && (BirdHitBox[3] <= HitBox[5]))) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }
    }
}