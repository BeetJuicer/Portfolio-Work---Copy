-> start

=== start ===
I implemented the interactive farming system, allowing players to plant, grow, and harvest crops across modular farming plots.
Want me to go into more technical detail?
+ [Yes] -> technical_detail
+ [No] -> END

=== technical_detail ===
I programmed a script that manages a crop's life cycle using four simple states: Waiting, Empty, Growing, and Ripe.

When a player plants something, the system pulls the crop's stats from a data file, creates its visual model in the game, and saves the details to our local database.

To prevent players from cheating by changing their device's clock, the game calculates completion times using an online time server instead of the local device time.

To save processing power, the game only checks the growth timer twice per second instead of every single frame, while still accounting for any active speed boosts.

Player inputs change dynamically depending on the plot's current state, so clicking an empty plot opens the seed menu, while clicking a growing plot lets you speed up the timer.

Clicking a ripe plot harvests the crop, which calculates the final yield using active boosts, updates the player's inventory, and awards experience points.

Finally, the script deletes the crop's visual model from the world so the plot resets back to Empty.
-> END