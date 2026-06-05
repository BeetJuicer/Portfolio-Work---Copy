-> start

=== start ===
I created a donation system where players can submit daily requests, allowing other users to visit their towns and cook for them to earn resources.
Want me to go into more technical detail?
+ [Yes] -> technical_detail
+ [No] -> END

=== technical_detail ===
This is a serverless cloud function in Unity.
Players save food requests on their phones, calling a cloud function to ensure that all requests are filtered through proper access.
Only cloud functions can affect private player data.
I built a system where player requests are stored and reset every day by an automated scheduler. 
To make sure no player ever runs out of towns to visit and cook for, I created 10 bot accounts that are completely exempted from this daily cleanup, guaranteeing there are always active requests available.
    -> END