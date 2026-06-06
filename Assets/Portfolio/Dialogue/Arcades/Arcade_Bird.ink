->start
=== start ===
This is my Finite State Machine bird flight controller.
Each flight state hands off momentum to the next instead of resetting it.
A steep dive carries speed directly into your glide.
Fast dives launch into swift, weighty glides.
I'm particularly happy about how this controller feels! It feels like you're soaring through the air~
Spline-defined wind paths make the birds feel like they're following the air.
Want to see it in action?
+ [Yes] Let's fly! # onDialogueFinished:ChangeScene:Birds of a Feather
    -> END
+ [No] Maybe next time!
    -> END