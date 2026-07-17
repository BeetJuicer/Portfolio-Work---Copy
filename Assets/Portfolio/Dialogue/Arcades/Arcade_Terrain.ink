->start
=== start ===
This is my tile-based world generator.
It uses layered Perlin noise for biome and terrain classification.
You get wildly different layouts from a single seed.

Want to generate a world?
+ [Yes] # onDialogueFinished:ChangeScene:2DTerrain
    -> END
+ [No]
    -> END