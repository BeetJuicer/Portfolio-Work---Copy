-> start

=== start ===
Of course, a game like this can't fit into the standard 100MB limit for a Play Store release. To make it fit, I had to set up remote downloadable content!
Want me to go into more technical detail?
+ [Yes] -> technical_detail
+ [No] -> END

=== technical_detail ===
I integrated Unity's Cloud Content Delivery service so we could host our heavy game assets on the cloud instead of stuffing them directly into the initial app download. 
When a player installs the game through the Play Store, they get a lightweight launcher app. 

Once they open it, my system securely connects to the cloud backend and lets the player download the rest of the game assets directly to their device, keeping our store file size perfectly within Google's limits.
-> END