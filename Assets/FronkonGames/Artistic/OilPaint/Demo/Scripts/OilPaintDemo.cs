using System;
using UnityEngine;
using UnityEngine.Rendering;
using FronkonGames.Artistic.OilPaint;

/// <summary> Artistic: Oil Paint demo. </summary>
/// <remarks>
/// This code is designed for a simple demo, not for production environments.
/// </remarks>
public class OilPaintDemo : MonoBehaviour
{
  [Header("This code is only for the demo, not for production environments.")]

  [Space(20.0f), SerializeField]
  private VolumeProfile volumeProfile;

  private enum Effects
  {
    KuwaharaBasic,
    KuwaharaGeneralized,
    KuwaharaDirectional,
    KuwaharaAnisotropic,
    TomitaTsuji,
    SymmetricNearestNeighbor
  }

  private Effects effect;

  private KuwaharaBasicVolume volumeBasic;
  private KuwaharaGeneralizedVolume volumeGeneralized;
  private KuwaharaDirectionalVolume volumeDirectional;
  private KuwaharaAnisotropicVolume volumeAnisotropic;
  private TomitaTsujiVolume volumeTomitaTsuji;
  private SymmetricNearestNeighborVolume volumeSNN;

  private GUIStyle styleTitle;
  private GUIStyle styleLabel;
  private GUIStyle styleButton;

  private void ResetEffects()
  {
    UpdateEffect(Effects.TomitaTsuji);

    if (volumeBasic != null)
    {
      volumeBasic.intensity.value = 1.0f;
      volumeBasic.radius.value = (int)Mathf.Clamp(Screen.width / 3840.0f * 15.0f, 4.0f, 20.0f);
      volumeBasic.detail.value = OilPaint.Detail.Sharpen;
      volumeBasic.detailStrength.value = 1.0f;
      volumeBasic.waterColor.value = 0.25f;
    }

    if (volumeGeneralized != null)
    {
      volumeGeneralized.intensity.value = 1.0f;
      volumeGeneralized.radius.value = (int)Mathf.Clamp(Screen.width / 3840.0f * 15.0f, 4.0f, 20.0f);
      volumeGeneralized.detail.value = OilPaint.Detail.Sharpen;
      volumeGeneralized.detailStrength.value = 1.0f;
      volumeGeneralized.waterColor.value = 0.25f;
    }

    if (volumeDirectional != null)
    {
      volumeDirectional.intensity.value = 1.0f;
      volumeDirectional.radius.value = (int)Mathf.Clamp(Screen.width / 3840.0f * 15.0f, 4.0f, 20.0f);
      volumeDirectional.detail.value = OilPaint.Detail.Sharpen;
      volumeDirectional.detailStrength.value = 1.0f;
      volumeDirectional.waterColor.value = 0.25f;
    }

    if (volumeAnisotropic != null)
    {
      volumeAnisotropic.intensity.value = 1.0f;
      volumeAnisotropic.radius.value = (int)Mathf.Clamp(Screen.width / 3840.0f * 15.0f, 4.0f, 20.0f);
      volumeAnisotropic.detail.value = OilPaint.Detail.Sharpen;
      volumeAnisotropic.detailStrength.value = 1.0f;
      volumeAnisotropic.waterColor.value = 0.25f;
    }

    if (volumeTomitaTsuji != null)
    {
      volumeTomitaTsuji.intensity.value = 1.0f;
      volumeTomitaTsuji.radius.value = (int)Mathf.Clamp(Screen.width / 3840.0f * 15.0f, 4.0f, 20.0f);
      volumeTomitaTsuji.detail.value = OilPaint.Detail.Sharpen;
      volumeTomitaTsuji.detailStrength.value = 1.0f;
      volumeTomitaTsuji.waterColor.value = 0.25f;
    }

    if (volumeSNN != null)
    {
      volumeSNN.intensity.value = 1.0f;
      volumeSNN.radius.value = (int)Mathf.Clamp(Screen.width / 3840.0f * 15.0f, 4.0f, 20.0f);
      volumeSNN.detail.value = OilPaint.Detail.Sharpen;
      volumeSNN.detailStrength.value = 1.0f;
      volumeSNN.waterColor.value = 0.25f;
    }
  }

  private void UpdateEffect(Effects effect)
  {
    this.effect = effect;

    if (volumeBasic != null) volumeBasic.active = effect == Effects.KuwaharaBasic;
    if (volumeGeneralized != null) volumeGeneralized.active = effect == Effects.KuwaharaGeneralized;
    if (volumeDirectional != null) volumeDirectional.active = effect == Effects.KuwaharaDirectional;
    if (volumeAnisotropic != null) volumeAnisotropic.active = effect == Effects.KuwaharaAnisotropic;
    if (volumeTomitaTsuji != null) volumeTomitaTsuji.active = effect == Effects.TomitaTsuji;
    if (volumeSNN != null) volumeSNN.active = effect == Effects.SymmetricNearestNeighbor;
  }

  private void Awake()
  {
    styleTitle = styleLabel = styleButton = null;

    volumeBasic = volumeProfile != null && volumeProfile.TryGet(out KuwaharaBasicVolume volume0) ? volume0 : null;
    volumeGeneralized = volumeProfile != null && volumeProfile.TryGet(out KuwaharaGeneralizedVolume volume1) ? volume1 : null;
    volumeDirectional = volumeProfile != null && volumeProfile.TryGet(out KuwaharaDirectionalVolume volume2) ? volume2 : null;
    volumeAnisotropic = volumeProfile != null && volumeProfile.TryGet(out KuwaharaAnisotropicVolume volume3) ? volume3 : null;
    volumeTomitaTsuji = volumeProfile != null && volumeProfile.TryGet(out TomitaTsujiVolume volume4) ? volume4 : null;
    volumeSNN = volumeProfile != null && volumeProfile.TryGet(out SymmetricNearestNeighborVolume volume5) ? volume5 : null;

    if (volumeBasic == null ||
        volumeGeneralized == null ||
        volumeDirectional == null ||
        volumeAnisotropic == null ||
        volumeTomitaTsuji == null ||
        volumeSNN == null)
    {
      Debug.LogWarning($"Effect '{Constants.Asset.Name}' not found. You must add it to a Volume Profile.");
      this.enabled = false;
    }
    else
      ResetEffects();
  }

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

    GUILayout.BeginHorizontal();
    {
      GUILayout.BeginVertical("box", GUILayout.Width(450.0f), GUILayout.Height(Screen.height));
      {
        const float space = 10.0f;

        GUILayout.Space(space);

        GUILayout.Label("OIL PAINT DEMO", styleTitle);

        Effects newEffect = EnumField("", effect);
        if (newEffect != effect)
          UpdateEffect(newEffect);

        GUILayout.Space(space);

        switch (effect)
        {
          case Effects.KuwaharaBasic:
            if (volumeBasic != null)
            {
              volumeBasic.intensity.value = SliderField("Intensity", volumeBasic.intensity.value);
              volumeBasic.passes.value = SliderField("Passes", volumeBasic.passes.value, 1, 4);
              volumeBasic.radius.value = SliderField("Radius", volumeBasic.radius.value, 1, 20);
              volumeBasic.detail.value = EnumField("Detail", volumeBasic.detail.value);
              volumeBasic.waterColor.value = SliderField("Water color", volumeBasic.waterColor.value);
            }
            break;
          case Effects.KuwaharaGeneralized:
            if (volumeGeneralized != null)
            {
              volumeGeneralized.intensity.value = SliderField("Intensity", volumeGeneralized.intensity.value);
              volumeGeneralized.passes.value = SliderField("Passes", volumeGeneralized.passes.value, 1, 4);
              volumeGeneralized.radius.value = SliderField("Radius", volumeGeneralized.radius.value, 1, 20);
              volumeGeneralized.sharpness.value = SliderField("Sharpness", volumeGeneralized.sharpness.value, 0.0f, 18.0f);
              volumeGeneralized.hardness.value = SliderField("Hardness", volumeGeneralized.hardness.value, 1.0f, 100.0f);
              volumeGeneralized.detail.value = EnumField("Detail", volumeGeneralized.detail.value);
              volumeGeneralized.waterColor.value = SliderField("Water color", volumeGeneralized.waterColor.value);
            }
            break;
          case Effects.KuwaharaDirectional:
            if (volumeDirectional != null)
            {
              volumeDirectional.intensity.value = SliderField("Intensity", volumeDirectional.intensity.value);
              volumeDirectional.passes.value = SliderField("Passes", volumeDirectional.passes.value, 1, 4);
              volumeDirectional.radius.value = SliderField("Radius", volumeDirectional.radius.value, 1, 20);
              volumeDirectional.detail.value = EnumField("Detail", volumeDirectional.detail.value);
              volumeDirectional.waterColor.value = SliderField("Water color", volumeDirectional.waterColor.value);
            }
            break;
          case Effects.KuwaharaAnisotropic:
            if (volumeAnisotropic != null)
            {
              volumeAnisotropic.intensity.value = SliderField("Intensity", volumeAnisotropic.intensity.value);
              volumeAnisotropic.passes.value = SliderField("Passes", volumeAnisotropic.passes.value, 1, 4);
              volumeAnisotropic.radius.value = SliderField("Radius", volumeAnisotropic.radius.value, 1, 20);
              volumeAnisotropic.sharpness.value = SliderField("Sharpness", volumeAnisotropic.sharpness.value, 0.0f, 18.0f);
              volumeAnisotropic.hardness.value = SliderField("Hardness", volumeAnisotropic.hardness.value, 1.0f, 100.0f);
              volumeAnisotropic.alpha.value = SliderField("Alpha", volumeAnisotropic.alpha.value, 0.01f, 2.0f);
              volumeAnisotropic.zeroCrossing.value = SliderField("Zero Crossing", volumeAnisotropic.zeroCrossing.value, 0.25f, 2.0f);
              volumeAnisotropic.detail.value = EnumField("Detail", volumeAnisotropic.detail.value);
              volumeAnisotropic.waterColor.value = SliderField("Water color", volumeAnisotropic.waterColor.value);
            }
            break;
          case Effects.TomitaTsuji:
            if (volumeTomitaTsuji != null)
            {
              volumeTomitaTsuji.intensity.value = SliderField("Intensity", volumeTomitaTsuji.intensity.value);
              volumeTomitaTsuji.passes.value = SliderField("Passes", volumeTomitaTsuji.passes.value, 1, 4);
              volumeTomitaTsuji.radius.value = SliderField("Radius", volumeTomitaTsuji.radius.value, 1, 20);
              volumeTomitaTsuji.detail.value = EnumField("Detail", volumeTomitaTsuji.detail.value);
              volumeTomitaTsuji.waterColor.value = SliderField("Water color", volumeTomitaTsuji.waterColor.value);
            }
            break;
          case Effects.SymmetricNearestNeighbor:
            if (volumeSNN != null)
            {
              volumeSNN.intensity.value = SliderField("Intensity", volumeSNN.intensity.value);
              volumeSNN.passes.value = SliderField("Passes", volumeSNN.passes.value, 1, 4);
              volumeSNN.radius.value = SliderField("Radius", volumeSNN.radius.value, 1, 20);
              volumeSNN.detail.value = EnumField("Detail", volumeSNN.detail.value);
              volumeSNN.waterColor.value = SliderField("Water color", volumeSNN.waterColor.value);
            }
            break;
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("RESET", styleButton) == true)
          ResetEffects();

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

  private void OnDestroy() => UpdateEffect(Effects.TomitaTsuji);

  private bool ToggleField(string label, bool value)
  {
    GUILayout.BeginHorizontal();
    {
      GUILayout.Label(label, styleLabel);

      value = GUILayout.Toggle(value, string.Empty);
    }
    GUILayout.EndHorizontal();

    return value;
  }

  private float SliderField(string label, float value, float min = 0.0f, float max = 1.0f)
  {
    GUILayout.BeginHorizontal();
    {
      GUILayout.Label(label, styleLabel);

      value = GUILayout.HorizontalSlider(value, min, max);
    }
    GUILayout.EndHorizontal();

    return value;
  }

  private int SliderField(string label, int value, int min, int max)
  {
    GUILayout.BeginHorizontal();
    {
      GUILayout.Label(label, styleLabel);

      value = (int)GUILayout.HorizontalSlider(value, min, max);
    }
    GUILayout.EndHorizontal();

    return value;
  }

  private Color ColorField(string label, Color value, bool alpha = true)
  {
    GUILayout.BeginHorizontal();
    {
      GUILayout.Label(label, styleLabel);

      float originalAlpha = value.a;

      UnityEngine.Color.RGBToHSV(value, out float h, out float s, out float v);
      h = GUILayout.HorizontalSlider(h, 0.0f, 1.0f);
      value = UnityEngine.Color.HSVToRGB(h, s, v);

      if (alpha == false)
        value.a = originalAlpha;
    }
    GUILayout.EndHorizontal();

    return value;
  }

  private Vector3 Vector3Field(string label, Vector3 value, string x = "X", string y = "Y", string z = "Z", float min = 0.0f, float max = 1.0f)
  {
    GUILayout.Label(label, styleLabel);

    value.x = SliderField($"   {x}", value.x, min, max);
    value.y = SliderField($"   {y}", value.y, min, max);
    value.z = SliderField($"   {z}", value.z, min, max);

    return value;
  }

  private T EnumField<T>(string label, T value) where T : Enum
  {
    string[] names = System.Enum.GetNames(typeof(T));
    Array values = System.Enum.GetValues(typeof(T));
    int index = Array.IndexOf(values, value);

    GUILayout.BeginHorizontal();
    {
      if (string.IsNullOrEmpty(label) == false)
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
}
