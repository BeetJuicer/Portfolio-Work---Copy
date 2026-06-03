////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Martin Bustos @FronkonGames <fronkongames@gmail.com>. All rights reserved.
//
// THIS FILE CAN NOT BE HOSTED IN PUBLIC REPOSITORIES.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace FronkonGames.Artistic.Comic
{
  /// <summary> Comic Volume. </summary>
  [Serializable, VolumeComponentMenu("Fronkon Games/Artistic/Comic")]
  public sealed class ComicVolume : VolumeComponent, IPostProcessComponent
  {
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Common settings.

    /// <summary> Controls the intensity of the effect [0, 1]. Default 1. </summary>
    /// <remarks> An effect with Intensity equal to 0 will not be executed. </remarks>
    [FloatSliderWithReset(1.0f, 0.0f, 1.0f, "Controls the intensity of the effect [0, 1]. Default 1.")]
    public FloatSliderParameterLinear intensity = new(1.0f, 0.0f, 1.0f);

    #endregion
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Comic settings.

    /// <summary> Scale of dots [0.01, 1]. Default 0.2. </summary>
    [FloatSliderWithReset(0.2f, 0.01f, 1.0f, "Scale of dots [0.01, 1]. Default 0.2.")]
    public FloatSliderParameterNoInterpolation scale = new(0.2f, 0.01f, 1.0f);

    /// <summary> Color blend operation. Default Solid. </summary>
    [EnumDropdown((int)ColorBlends.Solid, "Color blend operation. Default Solid.")]
    public EnumParameterNoInterpolation<ColorBlends> colorBlend = new(ColorBlends.Solid);

    /// <summary> Edge enhancement intensity [0, 10]. Default 3. </summary>
    [FloatSliderWithReset(3.0f, 0.0f, 10.0f, "Edge enhancement intensity [0, 10]. Default 3.")]
    public FloatSliderParameterNoInterpolation edge = new(3.0f, 0.0f, 10.0f);

    /// <summary> Edge color tint. Default Black. </summary>
    [ColorWithReset(0x000000FF, "Edge color tint. Default Black.")]
    public ColorParameterNoInterpolation edgeColor = new(Color.black);

    /// <summary> Edge color blend operation. Default Solid. </summary>
    [EnumDropdown((int)ColorBlends.Solid, "Edge color blend operation. Default Solid.")]
    public EnumParameterNoInterpolation<ColorBlends> edgeColorBlend = new(ColorBlends.Solid);

    /// <summary> Intensity of each channel (cyan, magenta, yellow, black) [0, 90]. </summary>
    [Vector4SliderWithReset(new float[] { 15.0f, 75.0f, 0.0f, 45.0f }, 0.0f, 90.0f, "Intensity of each channel (cyan, magenta, yellow, black) [0, 90].")]
    public Vector4Parameter cmykPattern = new(DefaultCMYKPattern);

    #endregion
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Color settings.

    /// <summary> Brightness [-1, 1]. Default 0. </summary>
    [FloatSliderWithReset(0.0f, -1.0f, 1.0f, "Brightness [-1, 1]. Default 0.")]
    public FloatSliderParameterNoInterpolation brightness = new(0.0f, -1.0f, 1.0f);

    /// <summary> Contrast [0, 10]. Default 1. </summary>
    [FloatSliderWithReset(1.0f, 0.0f, 10.0f, "Contrast [0, 10]. Default 1.")]
    public FloatSliderParameterNoInterpolation contrast = new(1.0f, 0.0f, 10.0f);

    /// <summary> Gamma [0.1, 10]. Default 1. </summary>
    [FloatSliderWithReset(1.0f, 0.1f, 10.0f, "Gamma [0.1, 10]. Default 1.")]
    public FloatSliderParameterNoInterpolation gamma = new(1.0f, 0.1f, 10.0f);

    /// <summary> The color wheel [0, 1]. Default 0. </summary>
    [FloatSliderWithReset(0.0f, 0.0f, 1.0f, "The color wheel [0, 1]. Default 0.")]
    public FloatSliderParameterNoInterpolation hue = new(0.0f, 0.0f, 1.0f);

    /// <summary> Intensity of a colors [0, 2]. Default 1. </summary>
    [FloatSliderWithReset(1.0f, 0.0f, 2.0f, "Intensity of a colors [0, 2]. Default 1.")]
    public FloatSliderParameterNoInterpolation saturation = new(1.0f, 0.0f, 2.0f);

    #endregion
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Advanced settings.

    /// <summary> Does it affect the Scene View? </summary>
    [ToggleWithReset(false, "Does it affect the Scene View?")]
    public BoolParameterNoInterpolation affectSceneView = new(false);

    /// <summary> Use scaled time. </summary>
    [ToggleWithReset(true, "Use scaled time.")]
    public BoolParameterNoInterpolation useScaledTime = new(true);

    #endregion
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static Vector4 DefaultCMYKPattern = new Vector4(15.0f, 75.0f, 0.0f, 45.0f);

    /// <summary> Reset to default values. </summary>
    public void Reset()
    {
      intensity.value = 1.0f;

      scale.value = 0.2f;
      colorBlend.value = ColorBlends.Solid;
      edge.value = 3.0f;
      edgeColor.value = Color.black;
      edgeColorBlend.value = ColorBlends.Solid;
      cmykPattern.value = DefaultCMYKPattern;

      brightness.value = 0.0f;
      contrast.value = 1.0f;
      gamma.value = 1.0f;
      hue.value = 0.0f;
      saturation.value = 1.0f;

      affectSceneView.value = false;
      useScaledTime.value = true;
    }

    /// <summary> Is the effect active? </summary>
    public bool IsActive() => intensity.overrideState == true && intensity.value > 0.0f;

    /// <summary> Is the effect tile compatible? </summary>
    public bool IsTileCompatible() => false;
  }
}
