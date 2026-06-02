->start
=== start ===
This is my actor-controller architecture demo.
The actor owns all state — health, movement, animation.
The controller just feeds inputs.
Swapping a player controller for an AI controller at runtime is one line.
An FSM drives the AI — patrol, alert, idle.
You can possess any actor in the scene mid-game.
Want to try it?
+ [Yes] Take control! # onDialogueFinished:ChangeScene:Possession
    -> END
+ [No] I'll leave the possessing to you.
    -> END