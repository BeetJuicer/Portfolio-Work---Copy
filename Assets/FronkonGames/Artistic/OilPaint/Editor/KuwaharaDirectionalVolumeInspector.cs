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
using UnityEditor;

namespace FronkonGames.Artistic.OilPaint
{
  /// <summary> Kuwahara Directional Volume inspector. </summary>
  [CustomEditor(typeof(KuwaharaDirectionalVolume))]
  public sealed class KuwaharaDirectionalVolumeInspector : Inspector
  {
    protected override string Description() => "Orientation-adaptive smoothing that aligns with local image features";

    protected override void InspectorGUI()
    {
      KuwaharaDirectionalVolume volume = (KuwaharaDirectionalVolume)target;

      DrawFloatSliderWithReset("intensity", "Intensity");

      DrawIntSliderWithReset("passes", "Passes");
      DrawIntSliderWithReset("radius", "Radius");
      DrawFloatSliderWithReset("renderSize", "Render size");

      DrawEnumDropdownWithReset<OilPaint.Detail>("detail", "Improve details");
      if (volume.detail.value != OilPaint.Detail.None)
      {
        IndentLevel++;
        DrawFloatSliderWithReset("detailStrength", "Strength");

        if (volume.detail.value == OilPaint.Detail.Emboss)
        {
          DrawFloatSliderWithReset("embossStrength", "Emboss");
          DrawFloatSliderWithReset("embossAngle", "Angle");
        }

        IndentLevel--;
      }

      DrawFloatSliderWithReset("waterColor", "Water color");

      DrawToggleWithReset("processDepth", "Process depth");
      if (volume.processDepth.value == true)
      {
        IndentLevel++;
        DrawCurveWithReset("depthCurve", "Depth curve", OilPaint.DefaultDepthCurve);
        DrawFloatSliderWithReset("depthPower", "Depth power");
        DrawToggleWithReset("sampleSky", "Sample sky");
        DrawToggleWithReset("viewDepthCurve", "View depth curve");
        IndentLevel--;
      }
    }

    protected override void ResetValues()
    {
      if (EditorUtility.DisplayDialog("Reset", "Reset to default values?", "Yes", "No") == true)
      {
        KuwaharaDirectionalVolume volume = (KuwaharaDirectionalVolume)target;
        volume.Reset();

        SetTargetDirty();
      }
    }
  }
}
