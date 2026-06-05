->start
=== start ===
This is my procedural mesh generator.
It builds a smooth 3D mesh along any Bezier curve.
It samples points along the curve and extrudes a cross-section at each one.
The cross-section is swappable — roads, rivers, pipes, rails, whatever shape you feed it.
The geometry stitches itself together automatically.
Want to see it build something?
+ [Yes] Let's generate! # onDialogueFinished:ChangeScene:MeshGen
    -> END
+ [No] Maybe next time!
    -> END