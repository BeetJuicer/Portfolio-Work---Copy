->start
=== start ===
This is my flocking simulation.
Separation, alignment, cohesion — the classic three.
The tricky part was obstacle avoidance for thick objects.
Comparing boid-to-obstacle center distances doesn't account for the object's width — boids just clip right through.
I ended up repelling based on the closest surface point instead of the center.
Runs on compute shaders — 10,000+ boids at 60fps.
Wanna check out the demo?
+ [Yes] Cool, let's go! # onDialogueFinished:ChangeScene:Boids
    -> END
+ [No] No worries, feel free to look around!
    -> END