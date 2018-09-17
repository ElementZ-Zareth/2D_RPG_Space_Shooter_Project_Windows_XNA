# 2D_RPG_Space_Shooter_Project_Windows_XNA

One of my first Learning projects. Using C# and Windows Xbox XNA 4.0 library in Visual Studio.

Following this tutorial: http://xbox.create.msdn.com/en-US/education/tutorial/2dgame/getting_started

After completing the tutorial, I decided to expand on it and create something unique while learning to develop my own code as well as create sprites and animations and then implement them into the game.

Noted Features:
3D Parallaxing script:
Using the center of the screen as the starting point to draw out from, rather than the top left of the screen. Then adding a variable, called "depth" for all objects including the camera. This allows for perfect-to-scale 3D parallaxing inside a 2D game. 

Requires a reference point (ex: how tall is a pixel?).
"Main character is about 5.5 feet or 150 pixels"

Then using your reference, extrapolating distances (ex: what do the "depth" numbers represent? 
"27.24026274303030303030303030303 pixels in a foot
143828.5872832 pixels in a mile

10 depth = 1 mile
1 depth = .1 mile
10 depth = 5280 feet
1 depth = 528.0 feet
.1 depth = 52.80 feet
.01 depth = 5.280 feet
.001 depth = .5280 feet
0.00189393939393939393939393939394 = 1 foot
0.00189393939393939393939393939394 = 27.24026274303030303030303030303 pixels

textures can be 4096 pixels high (Texture2D)

Depth layer ratio to screen position ok at 4096 pixel height with .005depth variance."

Using this data, setting the scale (using depth) and x,y positions of all objects based on their relative position to the Camera. Objects will also need their scale adjusted (ex: the Moon at real life distances will need to change it's scale based on the initial pixel:height ratio).

3D manipulation of 2D objects: (Walls.cs, as an Object)
Takes a 2D image and allows for x,y,z rotation of that object within a 2D landscape, with use of the Parallaxing script.
Creates a List of Sprites, cuts the image from left to right (single pixel width, entire height) as new sprites and adds them to the list.
