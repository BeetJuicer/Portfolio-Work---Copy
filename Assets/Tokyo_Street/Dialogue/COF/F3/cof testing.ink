-> start

=== start ===
Publishing on mobile forced us to design around strict hardware limitations. I spearheaded our mobile debugging and performance optimization efforts.
Want to hear more?
+ [Yes] -> technical_detail
+ [No] -> END

=== technical_detail ===
Testing on low-to-mid-tier Android devices meant dealing with unexpected frame drops. I monitored runtime logs using Android Logcat, Unity Remote, and rendering passes using RenderDoc. 

Through the Unity Profiler and RenderDoc frame analysis, I discovered that our 3D models just had too many vertices and triangles for standard mobile GPUs. 

To solve this, we reduced our 3D polygon counts and shifted to lightweight 2D art (fake 3d) sprites for recurring elements, like our walking characters and the animals inside the coops. This drastically lowered draw calls and stabilized our frame rate.
-> END