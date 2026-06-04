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
using UnityEditor.Rendering;
using UnityEngine;

namespace FronkonGames.Artistic.OneBit.Editor
{
  /// <summary> One Bit inspector. </summary>
  [CustomEditor(typeof(OneBitVolume))]
  public class OneBitVolumeInspector : Inspector
  {
    public override void OnEnable()
    {
      if (serializedObject == null)
        return;
    }

    protected override void InspectorGUI()
    {
      OneBitVolume volume = (OneBitVolume)target;

      /////////////////////////////////////////////////
      // Common.
      /////////////////////////////////////////////////
      DrawFloatSliderWithReset("intensity");

      /////////////////////////////////////////////////
      // One Bit.
      /////////////////////////////////////////////////
      Separator();

      DrawFloatSliderWithReset("edges");

      DrawFloatSliderWithReset("noiseStrength", "Noise");
      IndentLevel++;
      DrawFloatSliderWithReset("noiseSeed", "Seed");
      IndentLevel--;

      DrawEnumDropdownWithReset<ColorBlends>("blendMode", "Blend", ColorBlends.Multiply);
      DrawEnumDropdownWithReset<ColorModes>("colorMode", "Color", ColorModes.Solid);

      IndentLevel++;

      switch (volume.colorMode.value)
      {
        case ColorModes.Solid:
          DrawColorWithReset("color", "Tint", Color.white);
          break;
        case ColorModes.Gradient:
          DrawMinMaxSliderWithReset("luminanceMin", "luminanceMax", "Luminance", 0.0f, 1.0f, 0.0f, 1.0f);
          // Gradient field with palette tool button
          EditorGUILayout.BeginHorizontal();
          {
            EditorGUILayout.PrefixLabel("Gradient");
            EditorGUILayout.BeginHorizontal();
            {
              SerializedDataParameter gradientParam = UnpackParameter("gradient");
              if (gradientParam != null)
              {
                EditorGUI.BeginChangeCheck();
                Gradient newGradient = EditorGUILayout.GradientField(gradientParam.value.gradientValue);
                if (EditorGUI.EndChangeCheck())
                  gradientParam.value.gradientValue = newGradient;
              }
              if (GUILayout.Button(EditorGUIUtility.IconContent("d_Search Icon"), EditorStyles.miniLabel, GUILayout.Width(20.0f), GUILayout.Height(20.0f)) == true)
                PaletteTool.ShowTool(volume);
            }
            EditorGUILayout.EndHorizontal();
          }
          EditorGUILayout.EndHorizontal();
          break;
        case ColorModes.Horizontal:
          DrawFloatSliderWithReset("horizontalOffset", "Offset");
          DrawColorWithReset("color0", "Color #1");
          DrawColorWithReset("color1", "Color #2");
          break;
        case ColorModes.Vertical:
          DrawFloatSliderWithReset("verticalOffset", "Offset");
          DrawColorWithReset("color0", "Color #1");
          DrawColorWithReset("color1", "Color #2");
          break;
        case ColorModes.Circular:
          DrawFloatSliderWithReset("circularRadius", "Radius");
          DrawColorWithReset("color0", "Color #1");
          DrawColorWithReset("color1", "Color #2");
          break;
      }

      IndentLevel--;

      DrawIntSliderWithReset("redCount", "Red count");
      DrawIntSliderWithReset("greenCount", "Green count");
      DrawIntSliderWithReset("blueCount", "Blue count");

      DrawToggleWithReset("invertColor", "Invert");
    }

    protected override void ResetValues() => ((OneBitVolume)target).Reset();

    protected override void CheckForErrors()
    {
      if (OneBit.IsInAnyRenderFeatures() == false)
      {
        Separator();

        EditorGUILayout.HelpBox($"Renderer Feature '{Constants.Asset.Name}' not found. You must add it as a Render Feature.", MessageType.Error);
      }
      else
      {
        OneBit[] effects = OneBit.Instances;

        bool anyEnabled = false;
        for (int i = 0; i < effects.Length; i++)
        {
          if (effects[i].isActive == true)
          {
            anyEnabled = true;
            break;
          }
        }

        if (anyEnabled == false)
        {
          Separator();

          EditorGUILayout.HelpBox($"No Renderer Feature '{Constants.Asset.Name}' is active. You must activate it in the Render Features.", MessageType.Warning);
        }
      }
    }
  }
}
