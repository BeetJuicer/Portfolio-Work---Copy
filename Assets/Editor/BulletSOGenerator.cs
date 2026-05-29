using UnityEngine;
using UnityEditor;
using System.IO;

public class BulletSOGenerator
{
    [MenuItem("Tools/Generate Bullet SOs")]
    public static void GenerateAll()
    {
        string path = EditorUtility.SaveFolderPanel("Choose output folder", "Assets", "BulletData");
        if (string.IsNullOrEmpty(path)) return;

        if (!path.StartsWith(Application.dataPath)) { Debug.LogError("Choose a folder inside Assets."); return; }
        string outputPath = "Assets" + path.Substring(Application.dataPath.Length);

        string wp = outputPath + "/Waves";
        string pp = outputPath + "/Patterns";
        string pre = outputPath + "/Presets";
        Directory.CreateDirectory(Application.dataPath + wp.Substring("Assets".Length));
        Directory.CreateDirectory(Application.dataPath + pp.Substring("Assets".Length));
        Directory.CreateDirectory(Application.dataPath + pre.Substring("Assets".Length));
        AssetDatabase.Refresh();

        Waves(wp);
        Patterns(pp);
        Presets(pre, wp, pp);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"Generated at {outputPath}");
        EditorUtility.FocusProjectWindow();
    }

    // ─── Waves ────────────────────────────────────────────────────────────────

    static void Waves(string p)
    {
        // Single shots
        W(p, "Wave_Single", 1, 0f, 0f, 1.0f, Vector2.right);
        W(p, "Wave_SniperRhythm", 1, 0f, 0f, 3.0f, Vector2.right);
        W(p, "Wave_ChargedRelease", 1, 0f, 0f, 4.0f, Vector2.right);

        // Bursts
        W(p, "Wave_DoubleTap", 2, 0.08f, 0f, 2.0f, Vector2.right);
        W(p, "Wave_TripleTap", 3, 0.07f, 0f, 2.0f, Vector2.right);
        W(p, "Wave_SlowBurst", 4, 0.4f, 0f, 2.0f, Vector2.right);
        W(p, "Wave_Volley", 12, 0f, 30f, 3.0f, Vector2.right);

        // Streams
        W(p, "Wave_RapidFire", 10, 0.1f, 0f, 1.5f, Vector2.right);
        W(p, "Wave_MachineGun", 20, 0.03f, 0f, 2.0f, Vector2.right);
        W(p, "Wave_Continuous", 40, 0.05f, 0f, 0.3f, Vector2.right);
        W(p, "Wave_WallLayer", 8, 0.08f, 0f, 0.4f, Vector2.right);

        // Spirals
        W(p, "Wave_SlowSpiral", 20, 0.1f, 10f, 2.0f, Vector2.right);
        W(p, "Wave_FastSpiral", 20, 0.05f, 20f, 2.0f, Vector2.right);
        W(p, "Wave_TightSpiral", 30, 0.06f, 5f, 2.5f, Vector2.right);
        W(p, "Wave_ReverseSpiral", 20, 0.08f, -15f, 2.0f, Vector2.right);

        // Sweeps (angle increments that arc across a range)
        W(p, "Wave_WideSweep", 10, 0.12f, 8f, 2.5f, Vector2.up);
        W(p, "Wave_HalfSweep", 10, 0.1f, 18f, 2.0f, Vector2.up);
        W(p, "Wave_UpwardArc", 12, 0.1f, 15f, 2.5f, Vector2.left);

        // Geometric sequences (each shot jumps by a fixed notable angle)
        W(p, "Wave_CardinalSeq", 4, 0.2f, 90f, 2.0f, Vector2.right);  // 4 cardinal dirs
        W(p, "Wave_HexSeq", 6, 0.2f, 60f, 2.5f, Vector2.right);  // 6 dirs
        W(p, "Wave_GoldenAngle", 20, 0.1f, 137.5f, 3.0f, Vector2.right);  // non-repeating organic
        W(p, "Wave_Wobble", 8, 0.1f, 45f, 1.5f, Vector2.right);  // snaps 8 fixed angles

        // Directional variants
        W(p, "Wave_Downward", 1, 0f, 0f, 1.0f, Vector2.down);
        W(p, "Wave_DiagonalDR", 1, 0f, 0f, 1.0f, new Vector2(1f, -1f));
        W(p, "Wave_DiagonalDL", 1, 0f, 0f, 1.0f, new Vector2(-1f, -1f));
    }

    static void W(string p, string name, int shots, float cd, float inc, float rest, Vector2 dir)
    {
        var so = ScriptableObject.CreateInstance<SO_BulletWave>();
        so.data = new BulletWaveData { shotsPerWave = shots, cooldownPerShot = cd, angleIncrement = inc, restPerWave = rest, startDirection = dir };
        AssetDatabase.CreateAsset(so, $"{p}/{name}.asset");
    }

    // ─── Patterns ─────────────────────────────────────────────────────────────

    static void Patterns(string p)
    {
        // Single bullets — speed profiles
        P(p, "Pattern_Standard", 1, 0f, 0f, 2f, 6f, Linear());
        P(p, "Pattern_Fast", 1, 0f, 0f, 1.5f, 12f, Linear());
        P(p, "Pattern_Slow", 1, 0f, 0f, 3f, 3f, Linear());
        P(p, "Pattern_Accelerating", 1, 0f, 0f, 2.5f, 3f, EaseIn());
        P(p, "Pattern_Decelerating", 1, 0f, 0f, 2f, 12f, EaseOut());
        P(p, "Pattern_Punch", 1, 0f, 0f, 1f, 18f, SharpEaseOut());
        P(p, "Pattern_Ghost", 1, 0f, 0f, 8f, 1.5f, Linear());       // slow, lingers
        P(p, "Pattern_Sprinter", 1, 0f, 0f, 0.4f, 20f, Linear());         // fast, vanishes
        P(p, "Pattern_Bell", 1, 0f, 0f, 3f, 5f, Bell());           // surges mid-life
        P(p, "Pattern_Hesitant", 1, 0f, 0f, 2f, 8f, Hesitant());       // idles then launches
        P(p, "Pattern_Stutter", 1, 0f, 0f, 3f, 7f, Stutter());        // fast-slow-fast

        // Negative speed (return/reverse)
        P(p, "Pattern_Boomerang", 1, 0f, 0f, 3f, 8f, Boomerang());      // goes out, comes back
        P(p, "Pattern_BoomerangFar", 1, 0f, 0f, 4f, 6f, BoomerangFar());   // travels far first
        P(p, "Pattern_Yoyo", 1, 0f, 0f, 5f, 7f, Yoyo());           // out → back past origin → out again
        P(p, "Pattern_Oscillating", 1, 0f, 0f, 5f, 5f, Oscillating());    // ping-pongs
        P(p, "Pattern_OscFast", 1, 0f, 0f, 3f, 8f, OscillatingFast());// rapid vibrating
        P(p, "Pattern_Lunge", 1, 0f, 0f, 2f, 10f, Lunge());          // flinches back mid-flight
        P(p, "Pattern_Stagger", 1, 0f, 0f, 4f, 6f, Stagger());        // stumbling forward
        P(p, "Pattern_Twitch", 1, 0f, 0f, 4f, 9f, Twitch());         // still with violent spikes
        P(p, "Pattern_Creep", 1, 0f, 0f, 5f, 4f, Creep());          // slow → snap back → slow

        // Spreads — spread = degrees between each bullet, offset centers the fan on the firing direction
        // 2 bullets 8deg apart, centered:  offset = -4
        P(p, "Pattern_Split", 2, 8f, -4f, 2f, 7f, Linear());
        // 3 bullets 15deg apart, centered: offset = -15
        P(p, "Pattern_NarrowSpread", 3, 15f, -15f, 2f, 6f, Linear());
        // 5 bullets 20deg apart, centered: offset = -40
        P(p, "Pattern_WideSpread", 5, 20f, -40f, 2f, 6f, Linear());
        // 8 bullets ~14deg apart, centered: offset = -49  (shotgun, decelerating)
        P(p, "Pattern_Shotgun", 8, 14f, -49f, 1f, 10f, EaseOut());
        // 6 bullets 36deg apart, centered: offset = -90  (hemisphere = 180deg total arc)
        P(p, "Pattern_Hemisphere", 6, 36f, -90f, 2f, 6f, Linear());

        // Arcs — spread = step between bullets, offset positions start of arc
        // 90deg arc, 5 bullets: step = 90/4 = 22.5, centered offset = -45
        P(p, "Pattern_Arc90", 5, 22.5f, -45f, 2f, 6f, Linear());
        // 270deg arc, 10 bullets: step = 270/9 = 30, centered offset = -135
        P(p, "Pattern_Arc270", 10, 30f, -135f, 2f, 5f, Linear());
        // Rear 120deg arc, 5 bullets: step = 30, starts at 120 (behind-left)
        P(p, "Pattern_RearArc", 5, 30f, 120f, 2f, 6f, Linear());
        // Both sides: 2 bullets 180deg apart, first at 90
        P(p, "Pattern_SideBoth", 2, 180f, 90f, 2f, 6f, Linear());

        // Rings — spread = 360/count so bullets form a full evenly-spaced ring
        P(p, "Pattern_Triangle", 3, 120f, 0f, 2f, 5f, Linear());
        P(p, "Pattern_Square", 4, 90f, 0f, 2f, 5f, Linear());
        P(p, "Pattern_Pentagon", 5, 72f, 0f, 2f, 5f, Linear());
        P(p, "Pattern_Hexagon", 6, 60f, 0f, 2f, 5f, Linear());
        P(p, "Pattern_Octagon", 8, 45f, 0f, 2f, 5f, Linear());
        P(p, "Pattern_Ring12", 12, 30f, 0f, 2f, 5f, Linear());
        P(p, "Pattern_Ring24", 24, 15f, 0f, 2f, 5f, Linear());
        P(p, "Pattern_Ring32", 32, 11.25f, 0f, 2f, 4f, Linear());

        // Offset rings — same spread, offset by half a step to interleave with base ring
        P(p, "Pattern_DiagCross", 4, 90f, 45f, 2f, 6f, Linear());   // Square offset 45
        P(p, "Pattern_Star", 6, 60f, 30f, 2f, 5f, Linear());   // Hexagon offset 30
        P(p, "Pattern_Ring12Off", 12, 30f, 15f, 2f, 5f, Linear());   // Ring12 offset 15

        // Ring + speed combos
        P(p, "Pattern_ExpandRing", 12, 30f, 0f, 3f, 2f, EaseIn());
        P(p, "Pattern_DecelRing", 12, 30f, 0f, 2f, 10f, SharpEaseOut());
        P(p, "Pattern_CrawlRing", 12, 30f, 0f, 6f, 1.5f, Linear());
        P(p, "Pattern_HesitantRing", 12, 30f, 0f, 2.5f, 7f, Hesitant());
        P(p, "Pattern_BoomerangRing", 12, 30f, 0f, 3f, 6f, Boomerang());
        P(p, "Pattern_OscRing", 12, 30f, 0f, 5f, 5f, Oscillating());
    }

    static void P(string p, string name, int count, float spread, float offset, float lifetime, float speed, AnimationCurve curve)
    {
        var so = ScriptableObject.CreateInstance<SO_BulletPattern>();
        so.data = new BulletPatternData { count = count, spread = spread, startAngleOffset = offset, bulletData = new BulletData { lifetime = lifetime, speed = speed, speedCurve = curve } };
        AssetDatabase.CreateAsset(so, $"{p}/{name}.asset");
    }

    // ─── Curves ───────────────────────────────────────────────────────────────

    static AnimationCurve Linear() => AnimationCurve.Linear(0f, 1f, 1f, 1f);
    static AnimationCurve EaseIn() => AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    static AnimationCurve EaseOut() => new AnimationCurve(new Keyframe(0f, 1f, 0f, -2f), new Keyframe(1f, 0.1f));
    static AnimationCurve SharpEaseOut() => new AnimationCurve(new Keyframe(0f, 1f, 0f, -4f), new Keyframe(0.3f, 0.05f), new Keyframe(1f, 0.02f));

    // 0 → peak → 0 — surges mid-life
    static AnimationCurve Bell() => new AnimationCurve(new Keyframe(0f, 0.2f, 0f, 3f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0.2f, -3f, 0f));

    // Idles then launches at end of life
    static AnimationCurve Hesitant() => new AnimationCurve(new Keyframe(0f, 0.05f), new Keyframe(0.6f, 0.05f, 0f, 4f), new Keyframe(1f, 1f, 4f, 0f));

    // Fast → slow → fast → slow
    static AnimationCurve Stutter() => new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(0.25f, 0.2f), new Keyframe(0.5f, 1f), new Keyframe(0.75f, 0.2f), new Keyframe(1f, 1f));

    // 0 → 1 → -1 — goes out then returns
    static AnimationCurve Boomerang() => new AnimationCurve(new Keyframe(0f, 0f, 0f, 4f), new Keyframe(0.35f, 1f), new Keyframe(0.6f, 0f, 0f, -4f), new Keyframe(1f, -1f));

    // 0 → 1 (hold) → -1 — travels far before returning
    static AnimationCurve BoomerangFar() => new AnimationCurve(new Keyframe(0f, 0f, 0f, 3f), new Keyframe(0.2f, 1f), new Keyframe(0.7f, 1f), new Keyframe(0.85f, 0f, 0f, -4f), new Keyframe(1f, -1f));

    // 1 → -1 → 0.5 — out, back past origin, weakly re-launches
    static AnimationCurve Yoyo() => new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(0.3f, 1f, 0f, -5f), new Keyframe(0.5f, 0f), new Keyframe(0.65f, -1f), new Keyframe(0.8f, 0f, 0f, 3f), new Keyframe(1f, 0.5f));

    // 1,-1,1,-1,1,-1 — ping-pongs
    static AnimationCurve Oscillating() => new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(0.17f, -1f), new Keyframe(0.33f, 1f), new Keyframe(0.5f, -1f), new Keyframe(0.67f, 1f), new Keyframe(0.83f, -1f), new Keyframe(1f, 1f));

    // High-frequency ping-pong
    static AnimationCurve OscillatingFast() => new AnimationCurve(
        new Keyframe(0f, 1f), new Keyframe(0.083f, -1f), new Keyframe(0.167f, 1f), new Keyframe(0.25f, -1f),
        new Keyframe(0.333f, 1f), new Keyframe(0.417f, -1f), new Keyframe(0.5f, 1f), new Keyframe(0.583f, -1f),
        new Keyframe(0.667f, 1f), new Keyframe(0.75f, -1f), new Keyframe(0.833f, 1f), new Keyframe(0.917f, -1f), new Keyframe(1f, 1f));

    // Shoots forward, flinches back, continues
    static AnimationCurve Lunge() => new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(0.3f, 1f, 0f, -5f), new Keyframe(0.45f, -0.3f), new Keyframe(0.6f, 0f, 0f, 3f), new Keyframe(1f, 1f));

    // Irregular stumbling forward motion
    static AnimationCurve Stagger() => new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(0.2f, 0.6f), new Keyframe(0.3f, -0.4f), new Keyframe(0.45f, 0.8f), new Keyframe(0.6f, 0.3f), new Keyframe(0.7f, -0.5f), new Keyframe(0.85f, 1f), new Keyframe(1f, 0.5f));

    // Mostly still with sudden violent spikes
    static AnimationCurve Twitch() => new AnimationCurve(
        new Keyframe(0f, 0.1f), new Keyframe(0.15f, 1f), new Keyframe(0.2f, -0.8f), new Keyframe(0.25f, 0.1f),
        new Keyframe(0.5f, 0.1f), new Keyframe(0.6f, -1f), new Keyframe(0.65f, 0.8f), new Keyframe(0.7f, 0.1f),
        new Keyframe(0.85f, 1f), new Keyframe(0.9f, -0.5f), new Keyframe(1f, 0.1f));

    // Slow crawl → snap back → slow crawl again
    static AnimationCurve Creep() => new AnimationCurve(new Keyframe(0f, 0.2f), new Keyframe(0.3f, 0.2f, 0f, -4f), new Keyframe(0.45f, -0.8f), new Keyframe(0.55f, 0f, 0f, 3f), new Keyframe(1f, 0.2f));

    // ─── Presets ──────────────────────────────────────────────────────────────

    static void Presets(string presetsPath, string wavesPath, string patternsPath)
    {
        // Helper to load a just-created asset
        SO_BulletWave Wave(string n) => AssetDatabase.LoadAssetAtPath<SO_BulletWave>($"{wavesPath}/{n}.asset");
        SO_BulletPattern Pattern(string n) => AssetDatabase.LoadAssetAtPath<SO_BulletPattern>($"{patternsPath}/{n}.asset");

        Preset(presetsPath, "Preset_Flower",
            Wave("Wave_TightSpiral"), Pattern("Pattern_BoomerangRing"),
            "Tight spiral wave fires rings of 12 that shoot out and return. Petals bloom and collapse.");

        Preset(presetsPath, "Preset_Galaxy",
            Wave("Wave_GoldenAngle"), Pattern("Pattern_Standard"),
            "20 shots at 137.5deg golden angle increments. Never repeats, looks organic and swirling.");

        Preset(presetsPath, "Preset_Pinwheel",
            Wave("Wave_SlowSpiral"), Pattern("Pattern_Split"),
            "Spiraling pairs of bullets create a slow rotating pinwheel.");

        Preset(presetsPath, "Preset_Vortex",
            Wave("Wave_FastSpiral"), Pattern("Pattern_Double"),
            "Two opposing bullets spiral fast outward. Double helix.");

        Preset(presetsPath, "Preset_Sunburst",
            Wave("Wave_Volley"), Pattern("Pattern_Ring24"),
            "Instant 24-bullet ring. Static and dramatic.");

        Preset(presetsPath, "Preset_Ripple",
            Wave("Wave_SlowPulse"), Pattern("Pattern_Ring12"),
            "Slow rings fire one at a time. Like dropping a stone in water.");

        Preset(presetsPath, "Preset_Shockwave",
            Wave("Wave_Volley"), Pattern("Pattern_OscRing"),
            "Ring of 12 fires out then reverses back. Looks like a pulse.");

        Preset(presetsPath, "Preset_Bloom",
            Wave("Wave_SlowBurst"), Pattern("Pattern_BoomerangRing"),
            "Slow sequence of rings that return. Each expands then collapses.");

        Preset(presetsPath, "Preset_Blizzard",
            Wave("Wave_MachineGun"), Pattern("Pattern_WideSpread"),
            "Rapid fire 5-bullet fans. Fills the screen fast.");

        Preset(presetsPath, "Preset_Drill",
            Wave("Wave_TightSpiral"), Pattern("Pattern_Cone"),
            "Tight rotating stream of narrow cones. Looks like a drill bit.");

        Preset(presetsPath, "Preset_Clock",
            Wave("Wave_HexSeq"), Pattern("Pattern_Standard"),
            "Single bullet fires at each of 6 evenly spaced directions. Clean and mechanical.");

        Preset(presetsPath, "Preset_Crossfire",
            Wave("Wave_CardinalSeq"), Pattern("Pattern_NarrowSpread"),
            "3-bullet fan fires in each cardinal direction. Dense crossing streams.");

        Preset(presetsPath, "Preset_Pendulum",
            Wave("Wave_HalfSweep"), Pattern("Pattern_Shotgun"),
            "Wide shotgun blast sweeps 180 degrees then stops.");

        Preset(presetsPath, "Preset_Comet",
            Wave("Wave_SniperRhythm"), Pattern("Pattern_Punch"),
            "Single heavy punching shot with long pause. Slow and threatening.");

        Preset(presetsPath, "Preset_Firecracker",
            Wave("Wave_StutterBurst"), Pattern("Pattern_Triangle"),
            "Rapid triple-ring bursts with short pause. Pops like an explosion.");

        Preset(presetsPath, "Preset_Cage",
            Wave("Wave_Volley"), Pattern("Pattern_Ring32"),
            "32-bullet instant ring. Near-inescapable wall.");

        Preset(presetsPath, "Preset_GravityWell",
            Wave("Wave_Continuous"), Pattern("Pattern_HesitantRing"),
            "Constant stream of rings that idle then suddenly launch. Unpredictable wall.");

        Preset(presetsPath, "Preset_Mortar",
            Wave("Wave_DoubleTap"), Pattern("Pattern_Arc90"),
            "Two quick forward arcs then silence. Like lobbing two grenades.");

        Preset(presetsPath, "Preset_SpiralArms",
            Wave("Wave_SlowSpiral"), Pattern("Pattern_Double"),
            "Two opposing bullets spiral slowly. Galaxy spiral arms.");

        Preset(presetsPath, "Preset_Web",
            Wave("Wave_Wobble"), Pattern("Pattern_Hemisphere"),
            "6-bullet forward arc fires at 8 snapping angles. Wide overlapping coverage.");
    }

    static void Preset(string path, string name, SO_BulletWave wave, SO_BulletPattern pattern, string desc)
    {
        var so = ScriptableObject.CreateInstance<SO_BulletPreset>();
        so.wave = wave;
        so.pattern = pattern;
        so.description = desc;
        AssetDatabase.CreateAsset(so, $"{path}/{name}.asset");
    }
}