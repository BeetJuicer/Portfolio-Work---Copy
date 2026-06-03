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

namespace FronkonGames.Artistic.OilPaint
{
  /// <summary> Kuwahara Basic Volume settings. </summary>
  [Serializable, VolumeComponentMenu("Fronkon Games/Artistic/Oil Paint/Kuwahara Basic"), HelpURL(Constants.Support.Documentation)]
  public sealed class KuwaharaBasicVolume : VolumeComponent, IPostProcessComponent
  {
    /// <summary> Controls the intensity of the effect [0, 1]. Default 1. </summary>
    [FloatSliderWithReset(1.0f, 0.0f, 1.0f)]
    public FloatSliderParameterLinear intensity = new(1.0f, 0.0f, 1.0f);

    /// <summary> Number of passes [1, 4]. Default 1. </summary>
    [IntSliderWithReset(1, 1, 4)]
    public IntSliderParameterNoInterpolation passes = new(1, 1, 4);

    /// <summary> Size of the kuwahara filter kernel [1, 20]. Default 10. </summary>
    [IntSliderWithReset(10, 1, 20)]
    public IntSliderParameterNoInterpolation radius = new(10, 1, 20);

    /// <summary> Detail algorithm used: None (default), Sharpen or Emboss. </summary>
    [EnumDropdown((int)OilPaint.Detail.None)]
    public EnumParameterNoInterpolation<OilPaint.Detail> detail = new(OilPaint.Detail.None);

    /// <summary> Strength of the detail [0, 1]. Default 1. Only valid if Detail it not None. </summary>
    [FloatSliderWithReset(1.0f, 0.0f, 1.0f)]
    public FloatSliderParameterNoInterpolation detailStrength = new(1.0f, 0.0f, 1.0f);

    /// <summary> Strength of the emboss effect [0, 20]. Default 5. Only valid if Detail it Emboss. </summary>
    [FloatSliderWithReset(5.0f, 0.0f, 20.0f)]
    public FloatSliderParameterNoInterpolation embossStrength = new(5.0f, 0.0f, 20.0f);

    /// <summary> Angle of the emboss effect [0, 360]. Default 0. Only valid if Detail it Emboss. </summary>
    [FloatSliderWithReset(0.0f, 0.0f, 360.0f)]
    public FloatSliderParameterNoInterpolation embossAngle = new(0.0f, 0.0f, 360.0f);

    /// <summary> Strength of the Water Color effect [0, 1]. Default 0. </summary>
    [FloatSliderWithReset(0.0f, 0.0f, 1.0f)]
    public FloatSliderParameterNoInterpolation waterColor = new(0.0f, 0.0f, 1.0f);

    /// <summary> Final render size, if it is less than 1 it will be faster but more blurred [0.1, 1]. Default 1. </summary>
    [FloatSliderWithReset(1.0f, 0.1f, 1.0f)]
    public FloatSliderParameterNoInterpolation renderSize = new(1.0f, 0.1f, 1.0f);

    /// <summary> Activate the use of the depth buffer to improve the effect. Default False. </summary>
    [ToggleWithReset(false)]
    public BoolParameterNoInterpolation processDepth = new(false);

    /// <summary> Change rate at which kernel sizes change between depths. Only valid if process depth is on. </summary>
    public CurveParameterNoInterpolation depthCurve = new(new AnimationCurve() { keys = OilPaint.DefaultDepthCurve.keys }, false);

    /// <summary> Factor to concentrate the depth curve [0.01, 1]. Default 1. </summary>
    [FloatSliderWithReset(1.0f, 0.01f, 1.0f)]
    public FloatSliderParameterNoInterpolation depthPower = new(1.0f, 0.01f, 1.0f);

    /// <summary> Affect the sky? Default True. </summary>
    [ToggleWithReset(true)]
    public BoolParameterNoInterpolation sampleSky = new(true);

    /// <summary> To view the depth curve in the Editor. </summary>
    [ToggleWithReset(false)]
    public BoolParameterNoInterpolation viewDepthCurve = new(false);

    /// <summary> Brightness [-1.0, 1.0]. Default 0. </summary>
    [FloatSliderWithReset(0.0f, -1.0f, 1.0f)]
    public FloatSliderParameterNoInterpolation brightness = new(0.0f, -1.0f, 1.0f);

    /// <summary> Contrast [0.0, 10.0]. Default 1. </summary>
    [FloatSliderWithReset(1.0f, 0.0f, 10.0f)]
    public FloatSliderParameterNoInterpolation contrast = new(1.0f, 0.0f, 10.0f);

    /// <summary> Gamma [0.1, 10.0]. Default 1. </summary>
    [FloatSliderWithReset(1.0f, 0.1f, 10.0f)]
    public FloatSliderParameterNoInterpolation gamma = new(1.0f, 0.1f, 10.0f);

    /// <summary> The color wheel [0.0, 1.0]. Default 0. </summary>
    [FloatSliderWithReset(0.0f, 0.0f, 1.0f)]
    public FloatSliderParameterNoInterpolation hue = new(0.0f, 0.0f, 1.0f);

    /// <summary> Intensity of a colors [0.0, 2.0]. Default 1. </summary>
    [FloatSliderWithReset(1.0f, 0.0f, 2.0f)]
    public FloatSliderParameterNoInterpolation saturation = new(1.0f, 0.0f, 2.0f);

    /// <summary> Does it affect the Scene View? </summary>
    [ToggleWithReset(false)]
    public BoolParameterNoInterpolation affectSceneView = new(false);

    /// <summary> Use scaled time? </summary>
    [ToggleWithReset(true)]
    public BoolParameterNoInterpolation useScaledTime = new(true);

    /// <summary> Reset to default values. </summary>
    public void Reset()
    {
      intensity.value = 1.0f;

      passes.value = 1;
      radius.value = 10;
      detail.value = OilPaint.Detail.None;
      detailStrength.value = 1.0f;
      embossStrength.value = 5.0f;
      embossAngle.value = 0.0f;
      waterColor.value = 0.0f;
      renderSize.value = 1.0f;

      brightness.value = 0.0f;
      contrast.value = 1.0f;
      gamma.value = 1.0f;
      hue.value = 0.0f;
      saturation.value = 1.0f;

      processDepth.value = false;

      useScaledTime.value = true;
    }

    /// <inheritdoc/>
    public bool IsActive() => intensity.overrideState == true && intensity.value > 0.0f;

    /// <inheritdoc/>
    public bool IsTileCompatible() => false;
  }
}
