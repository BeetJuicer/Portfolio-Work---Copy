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

namespace FronkonGames.Artistic.Neon.Editor
{
  /// <summary> Neon inspector. </summary>
  [CustomEditor(typeof(NeonVolume))]
  public class NeonVolumeInspector : Inspector
  {
    public override void OnEnable()
    {
      if (serializedObject == null)
        return;
    }

    protected override void InspectorGUI()
    {
      /////////////////////////////////////////////////
      // Common.
      /////////////////////////////////////////////////
      DrawFloatSliderWithReset("intensity");

      /////////////////////////////////////////////////
      // Neon.
      /////////////////////////////////////////////////
      Separator();

      DrawFloatSliderWithReset("strength");
      DrawIntSliderWithReset("radius");
      DrawEnumDropdownWithReset("blend", "Blend", ColorBlends.Screen);
      DrawFloatSliderWithReset("fisheye");
      DrawFloatSliderWithReset("speed");

      Separator();

      DrawToggleWithReset("processDepth");
      IndentLevel++;
      DrawFloatSliderWithReset("depthPower", "Power");
      DrawToggleWithReset("sampleSky", "Sample sky");
      IndentLevel--;
    }

    protected override void ResetValues() => ((NeonVolume)target).Reset();

    protected override void CheckForErrors()
    {
      if (Neon.IsInAnyRenderFeatures() == false)
      {
        Separator();

        EditorGUILayout.HelpBox($"Renderer Feature '{Constants.Asset.Name}' not found. You must add it as a Render Feature.", MessageType.Error);
      }
      else
      {
        Neon[] effects = Neon.Instances;

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
