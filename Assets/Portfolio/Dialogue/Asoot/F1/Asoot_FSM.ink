-> start

=== start ===
I built the main character using a 2D finite state machine character controller.
Want to hear about how the code architecture works?
+ [Yes] -> technical_detail
+ [No]
    -> END

=== technical_detail ===
I designed the controller around a finite state machine pattern.
It's a nice architectural pattern for clean separation of concerns, and it helps keep the movement code modular and tidy.

I implemented several distinct states, including idle, walk, jump, fall, land, climb, dash, and death. 

Under the hood, the system uses modular sub-components like a Core, Health, Vision, and an IDamageable interface, 

These are modular enough to be reused for the enemy AI as well!
-> END