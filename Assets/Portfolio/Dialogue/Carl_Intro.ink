INCLUDE globals.ink

-> intro_router

=== intro_router ===
{ has_done_intro:
    -> second
- else:
    -> start
}

=== start ===
# speaker: Carl
Hey, what's up!
I'm Carl.
Welcome to my interactive portfolio!
If you look around, you'll see doors to my published projects.
And in front of us is the demos room!
Feel free to look around!
~ has_done_intro = true
-> END

=== second ===
# speaker: Carl
Go ahead and look at my rooms!
-> END