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

namespace FronkonGames.Artistic.OneBit
{
  /// <summary> One Bit Volume. </summary>
  [Serializable, VolumeComponentMenu("Fronkon Games/Artistic/One Bit"), HelpURL(Constants.Support.Documentation)]
  public sealed class OneBitVolume : VolumeComponent, IPostProcessComponent
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
    #region One Bit settings.

    /// <summary> Edges strength [0, 10]. Default 0.5. </summary>
    [FloatSliderWithReset(0.5f, 0.0f, 10.0f, "Edges strength [0, 10]. Default 0.5.")]
    public FloatSliderParameterNoInterpolation edges = new(0.5f, 0.0f, 10.0f);

    /// <summary> Noise strength [0, 10]. Default 0.25. </summary>
    [FloatSliderWithReset(0.25f, 0.0f, 10.0f, "Noise strength [0, 10]. Default 0.25.")]
    public FloatSliderParameterNoInterpolation noiseStrength = new(0.25f, 0.0f, 10.0f);

    /// <summary> Noise seed [0, 5]. Default 1.7. </summary>
    [FloatSliderWithReset(1.7f, 0.0f, 5.0f, "Noise seed [0, 5]. Default 1.7.")]
    public FloatSliderParameterNoInterpolation noiseSeed = new(1.7f, 0.0f, 5.0f);

    /// <summary> Color blend. Default Multiply. </summary>
    [EnumDropdown((int)ColorBlends.Multiply, "Color blend. Default Multiply.")]
    public EnumParameterNoInterpolation<ColorBlends> blendMode = new(ColorBlends.Multiply);

    /// <summary> Sets color mode (Solid, Gradient, Horizontal, Vertical or Circular). Default Solid. </summary>
    [EnumDropdown((int)ColorModes.Solid, "Sets color mode (Solid, Gradient, Horizontal, Vertical or Circular). Default Solid.")]
    public EnumParameterNoInterpolation<ColorModes> colorMode = new(ColorModes.Solid);

    /// <summary> Tint color. Default white. </summary>
    [ColorWithReset(0xFFFFFFFF, "Tint color. Default white.")]
    public ColorParameterNoInterpolation color = new(Color.white);

    /// <summary> Tint color 1. Default white. </summary>
    [ColorWithReset(0xFFFFFFFF, "Tint color 1. Default white.")]
    public ColorParameterNoInterpolation color0 = new(Color.white);

    /// <summary> Tint color 2. Default white. </summary>
    [ColorWithReset(0xFFFFFFFF, "Tint color 2. Default white.")]
    public ColorParameterNoInterpolation color1 = new(Color.white);

    /// <summary> Color gradient for Gradient color mode. </summary>
    public GradientParameterNoInterpolation gradient = new(new Gradient() { colorKeys = new GradientColorKey[] { new(Color.white * 0.0f, 0.0f), new(Color.white * 0.33f, 0.2f), new(Color.white * 0.66f, 0.5f), new(Color.white * 1.0f, 1.0f) } });

    /// <summary> Minimum luminance value [0, 1]. Default 0. </summary>
    [FloatSliderWithReset(0.0f, 0.0f, 1.0f, "Minimum luminance value [0, 1]. Default 0.")]
    public FloatSliderParameterNoInterpolation luminanceMin = new(0.0f, 0.0f, 1.0f);

    /// <summary> Maximum luminance value [0, 1]. Default 1. </summary>
    [FloatSliderWithReset(1.0f, 0.0f, 1.0f, "Maximum luminance value [0, 1]. Default 1.")]
    public FloatSliderParameterNoInterpolation luminanceMax = new(1.0f, 0.0f, 1.0f);

    /// <summary> Gradient radius [0, 10]. Default 2. </summary>
    [FloatSliderWithReset(2.0f, 0.0f, 10.0f, "Gradient radius [0, 10]. Default 2.")]
    public FloatSliderParameterNoInterpolation circularRadius = new(2.0f, 0.0f, 10.0f);

    /// <summary> Horizontal offset [0, 2]. Default 1. </summary>
    [FloatSliderWithReset(1.0f, 0.0f, 2.0f, "Horizontal offset [0, 2]. Default 1.")]
    public FloatSliderParameterNoInterpolation horizontalOffset = new(1.0f, 0.0f, 2.0f);

    /// <summary> Vertical offset [0, 2]. Default 1. </summary>
    [FloatSliderWithReset(1.0f, 0.0f, 2.0f, "Vertical offset [0, 2]. Default 1.")]
    public FloatSliderParameterNoInterpolation verticalOffset = new(1.0f, 0.0f, 2.0f);

    /// <summary> Red channel color levels [0, 255]. Default 255. </summary>
    [IntSliderWithReset(255, 0, 255, "Red channel color levels [0, 255]. Default 255.")]
    public IntSliderParameterNoInterpolation redCount = new(255, 0, 255);

    /// <summary> Green channel color levels [0, 255]. Default 255. </summary>
    [IntSliderWithReset(255, 0, 255, "Green channel color levels [0, 255]. Default 255.")]
    public IntSliderParameterNoInterpolation greenCount = new(255, 0, 255);

    /// <summary> Blue channel color levels [0, 255]. Default 255. </summary>
    [IntSliderWithReset(255, 0, 255, "Blue channel color levels [0, 255]. Default 255.")]
    public IntSliderParameterNoInterpolation blueCount = new(255, 0, 255);

    /// <summary> Invert the color. Default false. </summary>
    [ToggleWithReset(false, "Invert the color. Default false.")]
    public BoolParameterNoInterpolation invertColor = new(false);

    #endregion
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Color settings.

    /// <summary> Brightness [-1.0, 1.0]. Default 0. </summary>
    [FloatSliderWithReset(0.0f, -1.0f, 1.0f, "Brightness [-1.0, 1.0]. Default 0.")]
    public FloatSliderParameterNoInterpolation brightness = new(0.0f, -1.0f, 1.0f);

    /// <summary> Contrast [0.0, 10.0]. Default 1. </summary>
    [FloatSliderWithReset(1.0f, 0.0f, 10.0f, "Contrast [0.0, 10.0]. Default 1.")]
    public FloatSliderParameterNoInterpolation contrast = new(1.0f, 0.0f, 10.0f);

    /// <summary> Gamma [0.1, 10.0]. Default 1. </summary>
    [FloatSliderWithReset(1.0f, 0.1f, 10.0f, "Gamma [0.1, 10.0]. Default 1.")]
    public FloatSliderParameterNoInterpolation gamma = new(1.0f, 0.1f, 10.0f);

    /// <summary> The color wheel [0.0, 1.0]. Default 0. </summary>
    [FloatSliderWithReset(0.0f, 0.0f, 1.0f, "The color wheel [0.0, 1.0]. Default 0.")]
    public FloatSliderParameterNoInterpolation hue = new(0.0f, 0.0f, 1.0f);

    /// <summary> Intensity of a colors [0.0, 2.0]. Default 1. </summary>
    [FloatSliderWithReset(1.0f, 0.0f, 2.0f, "Intensity of a colors [0.0, 2.0]. Default 1.")]
    public FloatSliderParameterNoInterpolation saturation = new(1.0f, 0.0f, 2.0f);

    #endregion
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Advanced settings.

    /// <summary> Does it affect the Scene View? Default false. </summary>
    [ToggleWithReset(false, "Does it affect the Scene View? Default false.")]
    public BoolParameterNoInterpolation affectSceneView = new(false);

    /// <summary> Use scaled time. </summary>
    [ToggleWithReset(true, "Use scaled time.")]
    public BoolParameterNoInterpolation useScaledTime = new(true);

    #endregion
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary> Reset to default values. </summary>
    public void Reset()
    {
      intensity.value = 1.0f;

      edges.value = 0.5f;
      noiseStrength.value = 0.25f;
      noiseSeed.value = 1.7f;
      blendMode.value = ColorBlends.Multiply;
      colorMode.value = ColorModes.Solid;
      color.value = Color.white;
      color0.value = Color.white;
      color1.value = Color.white;
      gradient.value = new Gradient() { colorKeys = new GradientColorKey[] { new(Color.white * 0.0f, 0.0f), new(Color.white * 0.33f, 0.2f), new(Color.white * 0.66f, 0.5f), new(Color.white * 1.0f, 1.0f) } };
      luminanceMin.value = 0.0f;
      luminanceMax.value = 1.0f;
      circularRadius.value = 2.0f;
      horizontalOffset.value = 1.0f;
      verticalOffset.value = 1.0f;
      redCount.value = 255;
      greenCount.value = 255;
      blueCount.value = 255;
      invertColor.value = false;

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
