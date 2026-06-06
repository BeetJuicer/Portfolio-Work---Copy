->start
=== start ===
This is my flocking simulation.
Separation, alignment, cohesion. The classic three.
The tricky part was obstacle avoidance for thick objects.
Comparing boid-to-obstacle center distances doesn't account for the object's width, boids just clip right through.
I ended up making a custom solution that uses raycasts and makes weighted decisions based on free areas.
I used compute shaders, so thousands of boids can run at 60fps.
Wanna check out the demo?
+ [Yes] Cool, let's go! # onDialogueFinished:ChangeScene:Boids
    -> END
+ [No] No worries, feel free to look around!
    -> END