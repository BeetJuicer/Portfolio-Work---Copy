VAR player_name = "Stranger"
VAR has_key = false

-> start

=== start ===
# speaker: Old Man
# portrait: neutral
# layout: left
Ah, a traveler! I don't get many visitors out here.

What's your name?

* [My name is Alex.]
    ~ player_name = "Alex"
    # speaker: Old Man
    # portrait: happy
    {player_name}! What a fine name.
    -> give_key
    
* [None of your business.]
    # speaker: Old Man
    # portrait: angry
    Hmph. Rude.
    -> locked_door

=== give_key ===
# speaker: Old Man
# portrait: happy
Here, take this key. You might need it.
~ has_key = true
-> locked_door

=== locked_door ===
# speaker: Old Man
# portrait: neutral
There's a locked door to the north.

{ has_key:
    # speaker: Old Man
    # portrait: happy
    Good thing you have that key, {player_name}!
    -> END
- else:
    # speaker: Old Man
    # portrait: sad
    Too bad you don't have a key...
    -> END
}