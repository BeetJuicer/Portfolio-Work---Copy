->start
=== start ===
This is my bird flight simulation.
Each flight state hands off momentum to the next instead of resetting it.
A steep dive carries speed directly into your glide.
Fast dives launch into swift, weighty skims.
Lift and drag scale with entry speed.
Spline-defined wind paths make the birds feel like they're reading the air.
Want to see it in action?
+ [Yes] Let's fly! # onDialogueFinished:ChangeScene:Birds of a Feather
    -> END
+ [No] Maybe next time!
    -> END