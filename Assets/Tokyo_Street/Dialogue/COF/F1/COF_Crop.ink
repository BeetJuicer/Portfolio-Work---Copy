-> start

=== start ===
I implemented the interactive farming system, allowing players to plant, grow, and harvest crops across modular farming plots.
Want me to go into more technical detail?
+ [Yes] -> technical_detail
+ [No] -> END

=== technical_detail ===
I programmed a `Plot` script that manages a crop's lifecycle using four simple states: Waiting, Empty, Growing, and Ripe.

When a player plants something, the system pulls data directly from a data file containing the crop's stats, spawns the visual model in the game, and saves the details to our local SQLite database.

To stop players from cheating by altering their mobile device's clock, the game calculates the completion time using an online network server time. To save processing power, the game only checks the timer twice per second instead of every single frame, calculating how much time is left and factoring in if a player has activated a speed boost.

Player inputs change dynamically depending on the current state of the plot. Clicking an empty plot opens up the seed selection menu, clicking a growing plot brings up a button to speed up the timer, and clicking a ripe plot lets the player harvest the crop.

When harvesting, the script calculates the final crop yield through a boost manager to check for buffs, updates the player's inventory, awards experience points, and deletes the crop's visual model from the world so the plot resets to Empty.
-> END