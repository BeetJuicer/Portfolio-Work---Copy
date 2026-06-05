-> start

=== start ===
I handled the game's data persistence!
Want me to go into more technical detail?
+ [Yes] -> technical_detail
+ [No] -> END

=== technical_detail ===
I set up the infrastructure to link the mobile game directly to SQLite, making sure all player data and game states are saved locally and persist across sessions.

In particular, I used the sqlite-net library for this.

I also set it up to connect to Unity Cloud Save to make sure that data is transferred between devices! More on this on the next floor.
    -> END