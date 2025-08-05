# This is a flappy bird game

# Bird class
 - Rect
 - Moves method, if receives input then increases height, else goes down, should work like a parabola but the x position doesn't change

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
 - Loads in Bird and Pipes
 - Constantly looks for input for bird position
 - Updates display with all new pos

# Future features
 - [ ] Score
 - [ ] Settings
 - [ ] Realtime background change (day night cycle)
 - [ ] Skins
 - [ ] Multiplayer