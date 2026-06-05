->start
=== start ===
This is my tile-based world generator.
It uses layered Perlin noise for biome and terrain classification.
Octaves, persistence, and lacunarity are all configurable.
Biome thresholds drive tile selection.
You get wildly different layouts from a single seed.
Want to generate a world?
+ [Yes] Let's see it! # onDialogueFinished:ChangeScene:2DTerrain
    -> END
+ [No] Maybe next time!
    -> END