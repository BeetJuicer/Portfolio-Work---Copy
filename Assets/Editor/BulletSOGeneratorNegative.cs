using UnityEngine;
using UnityEditor;
using System.IO;

public class BulletSOGeneratorNegative
{
    [MenuItem("Tools/Generate Negative Curve Bullet SOs")]
    public static void GenerateAll()
    {
        string path = EditorUtility.SaveFolderPanel("Choose output folder", "Assets", "BulletData");
        if (string.IsNullOrEmpty(path)) return;

        string outputPath;
        if (path.StartsWith(Application.dataPath))
            outputPath = "Assets" + path.Substring(Application.dataPath.Length);
        else
        {
            Debug.LogError("Please choose a folder inside your Assets directory.");
            return;
        }

        string patternsPath = outputPath + "/Patterns";
        Directory.CreateDirectory(Application.dataPath + patternsPath.Substring("Assets".Length));
        AssetDatabase.Refresh();

        CreatePatterns(patternsPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"Negative curve SOs generated at {patternsPath}");
        EditorUtility.FocusProjectWindow();
    }

    static void CreatePatterns(string path)
    {
        // Boomerang — shoots out then comes back to origin
        // 0 → 1 → -1
        CreatePattern(path, "Pattern_Boomerang",          1,   0f,  0f,  3f,  8f, BoomerangCurve());

        // Wide boomerang — travels far before returning
        // 0 → 1 (held long) → -1
        CreatePattern(path, "Pattern_BoomerangFar",       1,   0f,  0f,  4f,  6f, BoomerangFarCurve());

        // Fast boomerang — snaps out and snaps back quickly
        // 0 → 1 → -1, compressed
        CreatePattern(path, "Pattern_BoomerangSnap",      1,   0f,  0f, 1.5f,14f, BoomerangCurve());

        // Boomerang spread — 3 bullets that all return
        CreatePattern(path, "Pattern_BoomerangSpread",    3,  30f,  0f,  3f,  8f, BoomerangCurve());

        // Boomerang ring — ring of bullets that all shoot out and come back
        CreatePattern(path, "Pattern_BoomerangRing",     12, 360f,  0f,  3f,  6f, BoomerangCurve());

        // Oscillating — reverses direction repeatedly, bullet ping-pongs in a line
        // 1 → -1 → 1 → -1 → 1 → -1
        CreatePattern(path, "Pattern_Oscillating",        1,   0f,  0f,  5f,  5f, OscillatingCurve());

        // Fast oscillating — rapid reversal, looks like vibrating/buzzing
        CreatePattern(path, "Pattern_OscillatingFast",    1,   0f,  0f,  3f,  8f, OscillatingFastCurve());

        // Oscillating spread — fan of bullets all ping-ponging
        CreatePattern(path, "Pattern_OscillatingSpread",  5,  50f,  0f,  5f,  5f, OscillatingCurve());

        // Oscillating ring — ring that pulses in and out
        CreatePattern(path, "Pattern_OscillatingRing",   12, 360f,  0f,  5f,  5f, OscillatingCurve());

        // Lunge — shoots forward fast, briefly reverses, then continues forward
        // 1 → -0.3 → 1
        CreatePattern(path, "Pattern_Lunge",              1,   0f,  0f,  2f, 10f, LungeCurve());

        // Stagger — moves forward, pauses/reverses slightly, lurches forward again, repeat
        // creates an uneven stumbling motion
        CreatePattern(path, "Pattern_Stagger",            1,   0f,  0f,  4f,  6f, StaggerCurve());

        // Orbit decay — starts going forward, reverses harder and harder
        // eventually ends up moving backward faster than it started
        CreatePattern(path, "Pattern_OrbitDecay",         1,   0f,  0f,  4f,  6f, OrbitDecayCurve());

        // Yoyo — shoots out, comes back past origin, shoots out again (double boomerang)
        // 1 → -1 → 1 (asymmetric, second launch weaker)
        CreatePattern(path, "Pattern_Yoyo",               1,   0f,  0f,  5f,  7f, YoyoCurve());

        // Twitch — mostly stationary with sudden violent lurches forward and back
        CreatePattern(path, "Pattern_Twitch",             1,   0f,  0f,  4f,  9f, TwitchCurve());

        // Creep — slow crawl forward, sudden snap back, slow crawl again
        CreatePattern(path, "Pattern_Creep",              1,   0f,  0f,  5f,  4f, CreepCurve());

        // Boomerang cross — 4-directional boomerangs
        CreatePattern(path, "Pattern_BoomerangCross",     4, 360f,  0f,  3f,  8f, BoomerangCurve());

        // Boomerang star — 6-directional boomerangs
        CreatePattern(path, "Pattern_BoomerangStar",      6, 360f,  0f,  3f,  7f, BoomerangFarCurve());

        // Oscillating cross
        CreatePattern(path, "Pattern_OscillatingCross",   4, 360f,  0f,  5f,  5f, OscillatingCurve());
    }

    static void CreatePattern(string path, string name, int count, float spread,
                               float startAngleOffset, float lifetime, float speed,
                               AnimationCurve speedCurve)
    {
        var so = ScriptableObject.CreateInstance<SO_BulletPattern>();
        so.data = new BulletPatternData
        {
            count            = count,
            spread           = spread,
            startAngleOffset = startAngleOffset,
            bulletData       = new BulletData
            {
                lifetime   = lifetime,
                speed      = speed,
                speedCurve = speedCurve
            }
        };
        AssetDatabase.CreateAsset(so, $"{path}/{name}.asset");
    }

    // ─── Curves ───────────────────────────────────────────────────────────────

    // 0 → 1 → -1  — shoots out then returns
    static AnimationCurve BoomerangCurve()
        => new AnimationCurve(
            new Keyframe(0f,    0f,  0f,  4f),
            new Keyframe(0.35f, 1f,  0f,  0f),
            new Keyframe(0.6f,  0f,  0f, -4f),
            new Keyframe(1f,   -1f,  0f,  0f)
        );

    // 0 → 1 (held) → -1  — travels far before returning
    static AnimationCurve BoomerangFarCurve()
        => new AnimationCurve(
            new Keyframe(0f,    0f,  0f,  3f),
            new Keyframe(0.2f,  1f,  0f,  0f),
            new Keyframe(0.7f,  1f,  0f,  0f),
            new Keyframe(0.85f, 0f,  0f, -4f),
            new Keyframe(1f,   -1f,  0f,  0f)
        );

    // 1 → -1 → 1 → -1 → 1 → -1  — ping-pongs
    static AnimationCurve OscillatingCurve()
        => new AnimationCurve(
            new Keyframe(0f,    1f,  0f,  0f),
            new Keyframe(0.17f,-1f,  0f,  0f),
            new Keyframe(0.33f, 1f,  0f,  0f),
            new Keyframe(0.5f, -1f,  0f,  0f),
            new Keyframe(0.67f, 1f,  0f,  0f),
            new Keyframe(0.83f,-1f,  0f,  0f),
            new Keyframe(1f,    1f,  0f,  0f)
        );

    // Rapid oscillation — high frequency reversal
    static AnimationCurve OscillatingFastCurve()
        => new AnimationCurve(
            new Keyframe(0f,     1f,  0f, 0f),
            new Keyframe(0.083f,-1f,  0f, 0f),
            new Keyframe(0.167f, 1f,  0f, 0f),
            new Keyframe(0.25f, -1f,  0f, 0f),
            new Keyframe(0.333f, 1f,  0f, 0f),
            new Keyframe(0.417f,-1f,  0f, 0f),
            new Keyframe(0.5f,   1f,  0f, 0f),
            new Keyframe(0.583f,-1f,  0f, 0f),
            new Keyframe(0.667f, 1f,  0f, 0f),
            new Keyframe(0.75f, -1f,  0f, 0f),
            new Keyframe(0.833f, 1f,  0f, 0f),
            new Keyframe(0.917f,-1f,  0f, 0f),
            new Keyframe(1f,     1f,  0f, 0f)
        );

    // 1 → -0.3 → 1  — lurches forward, flinches back, continues
    static AnimationCurve LungeCurve()
        => new AnimationCurve(
            new Keyframe(0f,    1f,   0f,  0f),
            new Keyframe(0.3f,  1f,   0f, -5f),
            new Keyframe(0.45f,-0.3f, 0f,  0f),
            new Keyframe(0.6f,  0f,   0f,  3f),
            new Keyframe(1f,    1f,   0f,  0f)
        );

    // Stumbling forward motion with small recoils
    static AnimationCurve StaggerCurve()
        => new AnimationCurve(
            new Keyframe(0f,    1f,   0f,  0f),
            new Keyframe(0.2f,  0.6f, 0f,  0f),
            new Keyframe(0.3f, -0.4f, 0f,  0f),
            new Keyframe(0.45f, 0.8f, 0f,  0f),
            new Keyframe(0.6f,  0.3f, 0f,  0f),
            new Keyframe(0.7f, -0.5f, 0f,  0f),
            new Keyframe(0.85f, 1f,   0f,  0f),
            new Keyframe(1f,    0.5f, 0f,  0f)
        );

    // Starts forward, reversal gets stronger over time
    static AnimationCurve OrbitDecayCurve()
        => new AnimationCurve(
            new Keyframe(0f,    1f,   0f,  0f),
            new Keyframe(0.25f,-0.2f, 0f,  0f),
            new Keyframe(0.5f,  0.6f, 0f,  0f),
            new Keyframe(0.75f,-0.6f, 0f,  0f),
            new Keyframe(1f,   -1.2f, 0f,  0f)
        );

    // 1 → -1 → 0.5 — goes out, comes back past origin, weakly goes out again
    static AnimationCurve YoyoCurve()
        => new AnimationCurve(
            new Keyframe(0f,    1f,   0f,  0f),
            new Keyframe(0.3f,  1f,   0f, -5f),
            new Keyframe(0.5f,  0f,   0f,  0f),
            new Keyframe(0.65f,-1f,   0f,  0f),
            new Keyframe(0.8f,  0f,   0f,  3f),
            new Keyframe(1f,    0.5f, 0f,  0f)
        );

    // Near zero with sudden violent spikes
    static AnimationCurve TwitchCurve()
        => new AnimationCurve(
            new Keyframe(0f,    0.1f,  0f,  0f),
            new Keyframe(0.15f, 1f,    0f,  0f),
            new Keyframe(0.2f, -0.8f,  0f,  0f),
            new Keyframe(0.25f, 0.1f,  0f,  0f),
            new Keyframe(0.5f,  0.1f,  0f,  0f),
            new Keyframe(0.6f, -1f,    0f,  0f),
            new Keyframe(0.65f, 0.8f,  0f,  0f),
            new Keyframe(0.7f,  0.1f,  0f,  0f),
            new Keyframe(0.85f, 1f,    0f,  0f),
            new Keyframe(0.9f, -0.5f,  0f,  0f),
            new Keyframe(1f,    0.1f,  0f,  0f)
        );

    // Slow forward → snap back → slow forward again
    static AnimationCurve CreepCurve()
        => new AnimationCurve(
            new Keyframe(0f,    0.2f,  0f,  0f),
            new Keyframe(0.3f,  0.2f,  0f, -4f),
            new Keyframe(0.45f,-0.8f,  0f,  0f),
            new Keyframe(0.55f, 0f,    0f,  3f),
            new Keyframe(0.7f,  0.2f,  0f,  0f),
            new Keyframe(1f,    0.2f,  0f,  0f)
        );
}
