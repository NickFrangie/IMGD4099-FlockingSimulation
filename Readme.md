![image](https://github.com/NickFrangie/IMGD4099-FlockingSimulation/assets/51765298/ecbee6f9-f049-40f8-8551-3b5a8dd69c77)

 # Controls
A panel of interactable controls and sliders is located on the right side of the screen, during the simulation.
Use your mouse to click and drag the sliders to change the values of the simulation.

In addition, there is a button to reset the simulation. There is also a button to toggle the display of the boids
between a GPU render shader, and a CPU implementation that uses Unity Game Objects. This can be performed in real time.

**Press Q** at any time to toggle the display of this panel.

**Press Escape** at any time to quit the simulation.

# About
Because WebGL does not support Unity Compute Shaders, I was unable to port this project to be playable in browser.
However, included in this GitHub repository is a final build for this project, under the Releases section.
You are free to download the executable and play the simulation, which should support any Windows device.

# References
A variety of resources and references used in the development of this project.

[Flocking Simulation Coding Challenge](https://www.youtube.com/watch?v=mhjuuHl6qHM&t=21s) 

[Unity Compute Shader: Cellular Automate](https://www.youtube.com/watch?v=W07TrTq3a4o&t=128s)

[Unity Compute Shader: 3D Boids](https://www.artstation.com/blogs/degged/Ow6W/compute-shaders-in-unity-boids-simulation-on-gpu-shared-memory)

[Boids Algorithm Pseudocode](http://www.kfish.org/boids/pseudocode.html)

[Boids by Craig Reynolds](https://www.red3d.com/cwr/boids/)
