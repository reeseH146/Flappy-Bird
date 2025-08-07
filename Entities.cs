using Raylib_cs;
using System.Numerics;

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
                new Vector2((float)(ScreenX * 0.075), (float)(ScreenX * 0.075))
            );
            // Movement
            Jump = false;
            PosYMoveFall = (float)(ScreenY * 0.0028);
            PosYMoveJump = (float)(ScreenY * 0.17);
            /*PosYCurrent = Convert.ToInt16(ScreenY * 0.5);
            PosYSnap = Convert.ToInt16(ScreenY * 0.5);
            PosYMove = 0;
            MoveDirection = false;*/
            // Image
            BirdImage = Raylib.LoadImage("Assets/Bird.png"); // Loads custom image
            Raylib.ImageResizeNN(ref BirdImage, (int)HitBox.Width, (int)HitBox.Height); // Resizes custom image to fix hitbox
            BirdTex = Raylib.LoadTextureFromImage(BirdImage); // Creates texture from image
            if (Raylib.IsImageValid(BirdImage)) Raylib.UnloadImage(BirdImage);
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
            // if (Raylib.IsImageValid(BirdImage)) Raylib.UnloadImage(BirdImage); // Was causing performance issues, possibly .IsImageValid might be expensive
            if (Raylib.IsTextureValid(BirdTex)) Raylib.UnloadTexture(BirdTex);
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
        // Hitbox - used for positioning and collision
        private Rectangle[] HitBox;
        private Vector2 PipeSize;
        private float MovementSpeed;
        // Image/render
        private Image[] PipeImages;
        private Texture2D[] PipeTexs;

        public PipeObject(int ScreenX, int ScreenY, float PosX, float PosY, int BirdSize) // Expects position as centre
        {
            PipeSize = new Vector2((float)(ScreenX * 0.05), ScreenY);
            Random Rand = new Random();
            HitBox = new Rectangle[]
            {
                new Rectangle(PosX - (PipeSize[0] / 2), (float)(PosY - PipeSize[1]), PipeSize), // PosY does contradict normal centre positioning but PosY in this context is the end of the top pipe, bottom pipe uses PosY and adds a difference
                new Rectangle(PosX - (PipeSize[0] / 2), (float)(PosY + (BirdSize * 1.6 * Rand.Next(15, 18) * 0.1)), PipeSize)
            };
            MovementSpeed = (float)(ScreenX * 0.002);
            // Creates image and textures
            // Loads images
            PipeImages = new Image[]
            {
                Raylib.LoadImage("Assets/LocalImages/Pipe.png"),
                Raylib.LoadImage("Assets/LocalImages/Pipe.png"),
            };
            // Sanitises images
            Raylib.ImageResize(ref PipeImages[0], (int)HitBox[0].Width, (int)HitBox[0].Height);
            Raylib.ImageResize(ref PipeImages[1], (int)HitBox[1].Width, (int)HitBox[1].Height);
            Raylib.ImageRotate(ref PipeImages[1], 180);
            // Creates textures from images
            PipeTexs = new Texture2D[]
            {
                Raylib.LoadTextureFromImage(PipeImages[0]),
                Raylib.LoadTextureFromImage(PipeImages[1])
            };
            // Frees images from RAM
            if (Raylib.IsImageValid(PipeImages[0])) Raylib.UnloadImage(PipeImages[0]);
            if (Raylib.IsImageValid(PipeImages[1])) Raylib.UnloadImage(PipeImages[1]);
        }

        // Literally draws the pipe textures onto the screen
        public void Draw()
        {
            if (Raylib.IsTextureValid(PipeTexs[0]) && Raylib.IsTextureValid(PipeTexs[1])) // Confirms both textures are able to load
            {
                Raylib.DrawTexture(PipeTexs[0], (int)HitBox[0].X, (int)HitBox[0].Y, Color.White); // Top pipe
                Raylib.DrawTexture(PipeTexs[1], (int)HitBox[1].X, (int)HitBox[1].Y, Color.White); // Bottom pipe
            }
            else // Loads default rectangle if custom textures aren't available
            {
                Raylib.DrawRectangleRec(HitBox[0], Color.DarkGreen);
                Raylib.DrawRectangleRec(HitBox[1], Color.DarkGreen);
            }
        }

        // Decrements x position of pipe to move, checks if they need to reset
        public void Move(int ScreenX)
        {
            HitBox[0].X -= MovementSpeed;
            HitBox[1].X -= MovementSpeed;

            if ((HitBox[0].X + (HitBox[0].Width / 2)) < (ScreenX * -0.1))
            {
                HitBox[0].X = (float)(ScreenX * 1.1);
                HitBox[1].X = (float)(ScreenX * 1.1);
            }
        }

        // Collision check with player bird
        public bool Collision(Rectangle CollideObject)
        {
            if (Raylib.CheckCollisionRecs(CollideObject, HitBox[0]) || Raylib.CheckCollisionRecs(CollideObject, HitBox[1])) return true;
            else return false;
        }

        // Unloads all information
        public void Unload()
        {
            // if (Raylib.IsImageValid(PipeImages[0])) Raylib.UnloadImage(PipeImages[0]); // Was causing performance issues, possibly .IsImageValid might be expensive
            // if (Raylib.IsImageValid(PipeImages[1])) Raylib.UnloadImage(PipeImages[1]);
            if (Raylib.IsTextureValid(PipeTexs[0])) Raylib.UnloadTexture(PipeTexs[0]);
            if (Raylib.IsTextureValid(PipeTexs[1])) Raylib.UnloadTexture(PipeTexs[1]);
        }
    }
}