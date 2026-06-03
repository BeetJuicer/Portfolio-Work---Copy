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
using UnityEngine.Rendering;

namespace FronkonGames.Artistic.Neon
{
  /// <summary> Neon Volume. </summary>
  [Serializable, VolumeComponentMenu("Fronkon Games/Artistic/Neon")]
  public sealed class NeonVolume : VolumeComponent, IPostProcessComponent
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
    #region Neon settings.

    /// <summary> Neon power [0, 1]. Default 0.5. </summary>
    [FloatSliderWithReset(0.5f, 0.0f, 1.0f, "Neon power [0, 1]. Default 0.5.")]
    public FloatSliderParameterNoInterpolation strength = new(0.5f, 0.0f, 1.0f);

    /// <summary> Neon thickness [1, 20]. Default 1. </summary>
    [IntSliderWithReset(1, 1, 20, "Neon thickness [1, 20]. Default 1.")]
    public IntSliderParameterNoInterpolation radius = new(1, 1, 20);

    /// <summary> Color blend. Default Screen. </summary>
    [EnumDropdown((int)ColorBlends.Screen, "Color blend. Default Screen.")]
    public EnumParameterNoInterpolation<ColorBlends> blend = new(ColorBlends.Screen);

    /// <summary> Fisheye deformation effect [-1, 1]. Default 0. </summary>
    [FloatSliderWithReset(0.0f, -1.0f, 1.0f, "Fisheye deformation effect [-1, 1]. Default 0.")]
    public FloatSliderParameterNoInterpolation fisheye = new(0.0f, -1.0f, 1.0f);

    /// <summary> Speed of rotation [0, 5]. Default 0.25. </summary>
    [FloatSliderWithReset(0.25f, 0.0f, 5.0f, "Speed of rotation [0, 5]. Default 0.25.")]
    public FloatSliderParameterNoInterpolation speed = new(0.25f, 0.0f, 5.0f);

    /// <summary> Activate the use of the depth buffer to improve the effect. Default False. </summary>
    /// <remarks> The camera must have the 'Depth Texture' field set to 'On'. </remarks>
    [ToggleWithReset(false, "Activate the use of the depth buffer to improve the effect. Default False.")]
    public BoolParameterNoInterpolation processDepth = new(false);

    /// <summary> Factor to concentrate the depth [0, 1]. Default 1.0. </summary>
    [FloatSliderWithReset(1.0f, 0.0f, 1.0f, "Factor to concentrate the depth [0, 1]. Default 1.0.")]
    public FloatSliderParameterNoInterpolation depthPower = new(1.0f, 0.0f, 1.0f);

    /// <summary> Affect the sky? Default false. </summary>
    [ToggleWithReset(false, "Affect the sky? Default false.")]
    public BoolParameterNoInterpolation sampleSky = new(false);

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

    /// <summary> Reset to default values. </summary>
    public void Reset()
    {
      intensity.value = 1.0f;

      strength.value = 0.5f;
      radius.value = 1;
      blend.value = ColorBlends.Screen;
      fisheye.value = 0.0f;
      speed.value = 0.25f;
      processDepth.value = false;
      depthPower.value = 1.0f;
      sampleSky.value = false;

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
