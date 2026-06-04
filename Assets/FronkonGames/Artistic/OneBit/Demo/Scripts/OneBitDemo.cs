using System;
using UnityEngine;
using UnityEngine.Rendering;
using FronkonGames.Artistic.OneBit;

/// <summary> Artistic: One bit demo. </summary>
/// <remarks> This code is designed for a simple demo, not for production environments. </remarks>
public class OneBitDemo : MonoBehaviour
{
  [Header("This code is only for the demo, not for production environments.")]

  [Space(20.0f), SerializeField]
  private VolumeProfile volumeProfile;

  private OneBitVolume volume;

  private GUIStyle styleTitle;
  private GUIStyle styleLabel;
  private GUIStyle styleButton;
  private GUIStyle styleColor;

  private void Awake()
  {
    if (OneBit.IsInRenderFeatures() == false)
    {
      Debug.LogWarning($"Effect '{Constants.Asset.Name}' not found. You must add it as a Render Feature.");
#if UNITY_EDITOR
      if (UnityEditor.EditorUtility.DisplayDialog($"Effect '{Constants.Asset.Name}' not found", $"You must add '{Constants.Asset.Name}' as a Render Feature.", "Quit") == true)
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    styleTitle = styleLabel = styleButton = null;

    volume = volumeProfile != null && volumeProfile.TryGet(out OneBitVolume vol) ? vol : null;
    this.enabled = OneBit.IsInRenderFeatures();
  }

  private void OnEnable() => ResetDemo();

  private void OnDestroy() => volume?.Reset();

  private void OnGUI()
  {
    styleTitle ??= new GUIStyle(GUI.skin.label)
    {
      alignment = TextAnchor.LowerCenter,
      fontSize = 32,
      fontStyle = FontStyle.Bold
    };

    styleLabel ??= new GUIStyle(GUI.skin.label)
    {
      alignment = TextAnchor.UpperLeft,
      fontSize = 24
    };

    styleButton ??= new GUIStyle(GUI.skin.button)
    {
      fontSize = 24
    };

    styleColor = new GUIStyle(GUI.skin.button);
    styleColor.normal.background = styleColor.hover.background = Texture2D.whiteTexture;

    GUILayout.BeginHorizontal();
    {
      GUILayout.BeginVertical("box", GUILayout.Width(400.0f), GUILayout.Height(Screen.height));
      {
        const float space = 10.0f;

        GUILayout.Space(space);

        GUILayout.Label("ONE BIT DEMO", styleTitle);

        GUILayout.Space(space);

        volume.intensity.value = Slider("Intensity", volume.intensity.value);

        GUILayout.Space(space);

        volume.edges.value = Slider("Edges", volume.edges.value, 0.0f, 10.0f);
        volume.noiseStrength.value = Slider("Noise", volume.noiseStrength.value, 0.0f, 10.0f);
        volume.blendMode.value = Enum("Blend", volume.blendMode.value);

        volume.colorMode.value = Enum("Mode", volume.colorMode.value);
        switch (volume.colorMode.value)
        {
          case ColorModes.Solid:
            volume.color.value = Color("  Color", volume.color.value);
            break;
          case ColorModes.Gradient:
            break;
          case ColorModes.Horizontal:
            volume.horizontalOffset.value = Slider("  Offset", volume.horizontalOffset.value, 0.0f, 2.0f);
            volume.color0.value = Color("  Color #0", volume.color0.value);
            volume.color1.value = Color("  Color #1", volume.color1.value);
            break;
          case ColorModes.Vertical:
            volume.verticalOffset.value = Slider("  Offset", volume.verticalOffset.value, 0.0f, 2.0f);
            volume.color0.value = Color("  Color #0", volume.color0.value);
            volume.color1.value = Color("  Color #1", volume.color1.value);
            break;
          case ColorModes.Circular:
            volume.circularRadius.value = Slider("  Radius", volume.circularRadius.value, 0.0f, 10.0f);
            volume.color0.value = Color("  Color #0", volume.color0.value);
            volume.color1.value = Color("  Color #1", volume.color1.value);
            break;
        }

        volume.invertColor.value = Toogle("Invert", volume.invertColor.value);

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("RESET", styleButton) == true)
          ResetDemo();

        GUILayout.Space(4.0f);

        if (GUILayout.Button("ONLINE DOCUMENTATION", styleButton) == true)
          Application.OpenURL(Constants.Support.Documentation);

        GUILayout.Space(4.0f);

        if (GUILayout.Button("❤️ LEAVE A REVIEW ❤️", styleButton) == true)
          Application.OpenURL(Constants.Support.Store);

        GUILayout.Space(space * 2.0f);
      }
      GUILayout.EndVertical();

      GUILayout.FlexibleSpace();
    }
    GUILayout.EndHorizontal();
  }

  private void ResetDemo()
  {
    volume.Reset();
    volume.luminanceMax.value = 0.7f;
  }

  private bool Toogle(string label, bool value)
  {
    GUILayout.BeginHorizontal();
    {
      GUILayout.Label(label, styleLabel);

      value = GUILayout.Toggle(value, string.Empty);
    }
    GUILayout.EndHorizontal();

    return value;
  }

  private float Slider(string label, float value, float min = 0.0f, float max = 1.0f)
  {
    GUILayout.BeginHorizontal();
    {
      GUILayout.Label(label, styleLabel);

      value = GUILayout.HorizontalSlider(value, min, max);
    }
    GUILayout.EndHorizontal();

    return value;
  }

  private int Slider(string label, int value, int min, int max)
  {
    GUILayout.BeginHorizontal();
    {
      GUILayout.Label(label, styleLabel);

      value = (int)GUILayout.HorizontalSlider(value, min, max);
    }
    GUILayout.EndHorizontal();

    return value;
  }

  private Color Color(string label, Color value, bool alpha = true)
  {
    GUILayout.BeginHorizontal();
    {
      GUILayout.Label(label, styleLabel);
      float originalAlpha = value.a;

      GUI.backgroundColor = value;

      if (GUILayout.Button(string.Empty, styleColor, GUILayout.Height(24.0f)) == true)
        value = NextColor(value);

      GUI.backgroundColor = UnityEngine.Color.white;

      if (alpha == false)
        value.a = originalAlpha;
    }
    GUILayout.EndHorizontal();

    return value;
  }

  private Vector3 Vector3(string label, Vector3 value, string x = "X", string y = "Y", string z = "Z", float min = 0.0f, float max = 1.0f)
  {
    GUILayout.Label(label, styleLabel);

    value.x = Slider($"   {x}", value.x, min, max);
    value.y = Slider($"   {y}", value.y, min, max);
    value.z = Slider($"   {z}", value.z, min, max);

    return value;
  }

  private T Enum<T>(string label, T value) where T : Enum
  {
    string[] names = System.Enum.GetNames(typeof(T));
    Array values = System.Enum.GetValues(typeof(T));
    int index = Array.IndexOf(values, value);

    GUILayout.BeginHorizontal();
    {
      GUILayout.Label(label, styleLabel);

      if (GUILayout.Button("<", styleButton) == true)
        index = index > 0 ? index - 1 : values.Length - 1;

      GUILayout.Label(names[index], styleLabel);

      if (GUILayout.Button(">", styleButton) == true)
        index = index < values.Length - 1 ? index + 1 : 0;
    }
    GUILayout.EndHorizontal();

    return (T)(object)index;
  }

  private Color NextColor(Color value)
  {
    if (value == UnityEngine.Color.white)
      return UnityEngine.Color.red;
    if (value == UnityEngine.Color.red)
      return UnityEngine.Color.magenta;
    if (value == UnityEngine.Color.magenta)
      return UnityEngine.Color.blue;
    if (value == UnityEngine.Color.blue)
      return UnityEngine.Color.cyan;
    if (value == UnityEngine.Color.cyan)
      return UnityEngine.Color.green;
    if (value == UnityEngine.Color.green)
      return UnityEngine.Color.yellow;
    if (value == UnityEngine.Color.yellow)
      return UnityEngine.Color.black;

    return UnityEngine.Color.white;
  }
}
