->start
=== start ===
This is my time rewind system.
It's built on the Command pattern.
Every entity except the player is a TimeReversible object.
They snapshot state every N frames and lerp between snapshots on rewind.
Skipping frames instead of recording every tick keeps memory flat.
You can freeze the player and rewind everything else around you.
Want to bend time?
+ [Yes] Turn back the clock! # onDialogueFinished:ChangeScene:Time_ThirdPerson
    -> END
+ [No] Time stays put.
    -> END