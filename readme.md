# This is a flappy bird game
Dependencies :
 - .NET 9 runtime (unless published)
 - STD libraries
 - Raylib (included in code, not sure if building/publishing auto includes but debugging does)
Conventions : 
 - Allmann braces
 - PascalCase

# Pipe class
 - Creates rect, heights are passed in
 - Move method which moves it across the screen and reset back when goes off
 - Allows height to be changed
 - As game plays, distance can be decreased between the pipes

# Main
 - Initialise main resources (Bird, pipes)
 - Runs event loop manager

# Event loop manager
 - Checks which loop

# Menu event
 - Background
 - Buttons to other events

# Game event
 - Updates display (bird and pipes)
 - Checks for collision
 - Constantly looks for input for bird jump
 - Updates all positions

# Skins
 - Loads skins (.png, .jpeg, .jpg) and their names (name.extension)
 - Players can choose a skin
 - Players can press return to return to main menu

# Settings
 - Loads sliders/checkboxes and text to configure settings (display size, bird config, pipe config)
 - Players can press return to return to main menu

# Bird
 - Rect defines hitbox, position
 - A variable tracks whether its jumped or not and used to
 - Image and texture to utilise custom images
 
 - Draw - Draws custom image at current location, if image fails to load draws an orange rect in its place
 - Move - Updates position based on whether player jumped or not
 - CeilingCollision - Quick built in method to check collision with vertical bounds
 - Unload - Called when closing program to unload image and textures

# Pipe

# Future features
 - [ ] Score and high score
 - [ ] Advanced bird movement (tweening => dynamic positioning and image/hitbox rotation)
 - [ ] Advanced pipe positioning (gaps get smaller between pipes (vertically and horizontally), pipes shift up and down)
 - [ ] Settings
 - [ ] Default skins and custom skins
 - [ ] Realtime background change (day night cycle, IRL time and weather dependant (requires location and API), themes (apocalypse))
 - [ ] Multiplayer
 - [ ] Platform ports (WASM, Linux)