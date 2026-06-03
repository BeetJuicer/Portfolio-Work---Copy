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
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;

namespace FronkonGames.Artistic.Comic.Editor
{
  /// <summary> Custom drawers. </summary>
  public abstract partial class Inspector : VolumeComponentEditor
  {
    /// <summary> Draws an IntSliderWithResetAttribute with slider and reset using attribute configuration. </summary>
    protected void DrawIntSliderWithReset(string name, GUIContent label = null)
    {
      SerializedDataParameter parameter = UnpackParameter(name);
      if (parameter == null)
        return;

      // Try to get the attribute
      var attr = parameter.GetAttribute<IntSliderWithResetAttribute>();

      // Fallback to default drawing if attribute not found
      if (attr == null)
      {
        EditorGUILayout.PropertyField(parameter.value, label ?? new GUIContent(parameter.displayName));
        return;
      }

      // Determine label content
      GUIContent displayLabel = label ?? new GUIContent(parameter.displayName, attr.tooltip);

      EditorGUILayout.BeginHorizontal();
      {
        // Override checkbox (16px standard width)
        EditorGUI.showMixedValue = parameter.overrideState.hasMultipleDifferentValues;
        EditorGUI.showMixedValue = false;

        bool isOverridden = parameter.overrideState.boolValue;
        EditorGUI.BeginDisabledGroup(!isOverridden);

        EditorGUILayout.BeginHorizontal();
        {
          // Get current value
          int value = parameter.value.intValue;

          // Draw slider using attribute values
          value = EditorGUILayout.IntSlider(displayLabel, value, attr.min, attr.max);

          EditorGUI.EndDisabledGroup();
          int oldIndentLevel = EditorGUI.indentLevel;
          EditorGUI.indentLevel = 0;
          EditorGUILayout.PropertyField(parameter.overrideState, GUIContent.none, GUILayout.Width(16));
          EditorGUI.indentLevel = oldIndentLevel;
          EditorGUI.BeginDisabledGroup(!isOverridden);

          // Reset button - enabled when value differs from default
          if (ResetButton(attr.value, value != attr.value) == true)
          {
            value = attr.value;
          }

          // Apply back
          parameter.value.intValue = value;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUI.EndDisabledGroup();
      }
      EditorGUILayout.EndHorizontal();
    }

    /// <summary> Draws an FloatSliderWithResetAttribute with slider and reset using attribute configuration. </summary>
    protected void DrawFloatSliderWithReset(string name, string label = null)
    {
      SerializedDataParameter parameter = UnpackParameter(name);
      if (parameter == null)
        return;

      // Try to get the attribute
      var attr = parameter.GetAttribute<FloatSliderWithResetAttribute>();

      // Fallback to default drawing if attribute not found
      if (attr == null)
      {
        EditorGUILayout.PropertyField(parameter.value, new GUIContent(label ?? parameter.displayName));
        return;
      }

      // Determine label content
      GUIContent displayLabel = new(label ?? parameter.displayName, attr.tooltip);

      EditorGUILayout.BeginHorizontal();
      {
        // Override checkbox (16px standard width)
        EditorGUI.showMixedValue = parameter.overrideState.hasMultipleDifferentValues;
        EditorGUI.showMixedValue = false;

        bool isOverridden = parameter.overrideState.boolValue;
        EditorGUI.BeginDisabledGroup(!isOverridden);

        EditorGUILayout.BeginHorizontal();
        {
          // Get current value
          float value = parameter.value.floatValue;

          // Draw slider using attribute values
          value = EditorGUILayout.Slider(displayLabel, value, attr.min, attr.max);

          EditorGUI.EndDisabledGroup();
          int oldIndentLevel = EditorGUI.indentLevel;
          EditorGUI.indentLevel = 0;
          EditorGUILayout.PropertyField(parameter.overrideState, GUIContent.none, GUILayout.Width(16));
          EditorGUI.indentLevel = oldIndentLevel;
          EditorGUI.BeginDisabledGroup(!isOverridden);

          // Reset button - enabled when value differs from default
          if (ResetButton(attr.value, value != attr.value) == true)
            value = attr.value;

          // Apply back
          parameter.value.floatValue = value;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUI.EndDisabledGroup();
      }
      EditorGUILayout.EndHorizontal();
    }

    protected void DrawToggleWithReset(string name, bool defaultValue = default)
    {
      SerializedDataParameter parameter = UnpackParameter(name);
      if (parameter == null)
        return;

      GUIContent displayLabel = new(parameter.displayName);

      EditorGUILayout.BeginHorizontal();
      {
        // Override checkbox
        EditorGUI.showMixedValue = parameter.overrideState.hasMultipleDifferentValues;
        EditorGUI.showMixedValue = false;

        bool isOverridden = parameter.overrideState.boolValue;
        EditorGUI.BeginDisabledGroup(!isOverridden);

        EditorGUILayout.BeginHorizontal();
        {
          // Get current value
          bool value = parameter.value.boolValue;

          // Draw color field
          EditorGUI.BeginChangeCheck();
          value = EditorGUILayout.Toggle(displayLabel, value);

          EditorGUI.EndDisabledGroup();
          int oldIndentLevel = EditorGUI.indentLevel;
          EditorGUI.indentLevel = 0;
          EditorGUILayout.PropertyField(parameter.overrideState, GUIContent.none, GUILayout.Width(16));
          EditorGUI.indentLevel = oldIndentLevel;
          EditorGUI.BeginDisabledGroup(!isOverridden);

          // Reset button
          bool isDefault = EqualityComparer<bool>.Default.Equals(value, defaultValue);
          if (ResetButton(defaultValue, !isDefault) == true)
            value = defaultValue;

          // Apply back
          parameter.value.boolValue = value;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUI.EndDisabledGroup();
      }
      EditorGUILayout.EndHorizontal();
    }

    /// <summary> Draws a Vector3SliderWithResetAttribute with slider and reset using attribute configuration. </summary>
    protected void DrawVector3SliderWithReset(string name, string label = null, Vector3 defaultValue = default)
    {
      SerializedDataParameter parameter = UnpackParameter(name);
      if (parameter == null)
        return;

      // Try to get the attribute
      var attr = parameter.GetAttribute<Vector3SliderWithResetAttribute>();

      // Fallback to default drawing if attribute not found
      if (attr == null)
      {
        EditorGUILayout.PropertyField(parameter.value, new GUIContent(label ?? parameter.displayName));
        return;
      }

      GUIContent displayLabel = new(label ?? parameter.displayName, attr.tooltip);

      EditorGUILayout.BeginVertical();
      {
        EditorGUILayout.BeginHorizontal();
        {
          // Override checkbox
          EditorGUI.showMixedValue = parameter.overrideState.hasMultipleDifferentValues;
          EditorGUI.showMixedValue = false;

          bool isOverridden = parameter.overrideState.boolValue;
          EditorGUI.BeginDisabledGroup(!isOverridden);

          EditorGUILayout.BeginHorizontal();
          {
            // Get current value
            Vector3 value = parameter.value.vector3Value;

            EditorGUILayout.LabelField(displayLabel);

            EditorGUI.EndDisabledGroup();
            int oldIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUILayout.PropertyField(parameter.overrideState, GUIContent.none, GUILayout.Width(16));
            EditorGUI.indentLevel = oldIndentLevel;
            EditorGUI.BeginDisabledGroup(!isOverridden);

            // Reset button
            bool isDefault = EqualityComparer<Vector3>.Default.Equals(value, defaultValue);
            if (ResetButton(defaultValue, !isDefault) == true)
              value = defaultValue;

            // Apply back
            parameter.value.vector3Value = value;
          }
          EditorGUILayout.EndHorizontal();

          EditorGUI.EndDisabledGroup();
        }
        EditorGUILayout.EndHorizontal();

        bool overridden = parameter.overrideState.boolValue;
        EditorGUI.BeginDisabledGroup(!overridden);
        IndentLevel++;
        Vector3 vecValue = parameter.value.vector3Value;
        EditorGUI.BeginChangeCheck();
        vecValue.x = EditorGUILayout.Slider("X", vecValue.x, attr.min, attr.max);
        vecValue.y = EditorGUILayout.Slider("Y", vecValue.y, attr.min, attr.max);
        vecValue.z = EditorGUILayout.Slider("Z", vecValue.z, attr.min, attr.max);
        if (EditorGUI.EndChangeCheck() == true)
          parameter.value.vector3Value = vecValue;
        IndentLevel--;
        EditorGUI.EndDisabledGroup();
      }
      EditorGUILayout.EndVertical();
    }

    /// <summary> Draws a Vector4SliderWithResetAttribute with slider and reset using attribute configuration. </summary>
    protected void DrawVector4SliderWithReset(string name, string label = null, Vector4 defaultValue = default)
    {
      SerializedDataParameter parameter = UnpackParameter(name);
      if (parameter == null)
        return;

      // Try to get the attribute
      var attr = parameter.GetAttribute<Vector4SliderWithResetAttribute>();

      // Fallback to default drawing if attribute not found
      if (attr == null)
      {
        EditorGUILayout.PropertyField(parameter.value, new GUIContent(label ?? parameter.displayName));
        return;
      }

      GUIContent displayLabel = new(label ?? parameter.displayName, attr.tooltip);

      EditorGUILayout.BeginVertical();
      {
        EditorGUILayout.BeginHorizontal();
        {
          // Override checkbox
          EditorGUI.showMixedValue = parameter.overrideState.hasMultipleDifferentValues;
          EditorGUI.showMixedValue = false;

          bool isOverridden = parameter.overrideState.boolValue;
          EditorGUI.BeginDisabledGroup(!isOverridden);

          EditorGUILayout.BeginHorizontal();
          {
            // Get current value
            Vector4 value = parameter.value.vector4Value;

            EditorGUILayout.LabelField(displayLabel);

            EditorGUI.EndDisabledGroup();
            int oldIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUILayout.PropertyField(parameter.overrideState, GUIContent.none, GUILayout.Width(16));
            EditorGUI.indentLevel = oldIndentLevel;
            EditorGUI.BeginDisabledGroup(!isOverridden);

            // Reset button
            bool isDefault = EqualityComparer<Vector4>.Default.Equals(value, defaultValue);
            if (ResetButton(defaultValue, !isDefault) == true)
              value = defaultValue;

            // Apply back
            parameter.value.vector4Value = value;
          }
          EditorGUILayout.EndHorizontal();

          EditorGUI.EndDisabledGroup();
        }
        EditorGUILayout.EndHorizontal();

        bool overridden = parameter.overrideState.boolValue;
        EditorGUI.BeginDisabledGroup(!overridden);
        IndentLevel++;
        Vector4 vecValue = parameter.value.vector4Value;
        EditorGUI.BeginChangeCheck();
        vecValue.x = EditorGUILayout.Slider("Cyan", vecValue.x, attr.min, attr.max);
        vecValue.y = EditorGUILayout.Slider("Magenta", vecValue.y, attr.min, attr.max);
        vecValue.z = EditorGUILayout.Slider("Yellow", vecValue.z, attr.min, attr.max);
        vecValue.w = EditorGUILayout.Slider("Black", vecValue.w, attr.min, attr.max);
        if (EditorGUI.EndChangeCheck() == true)
          parameter.value.vector4Value = vecValue;
        IndentLevel--;
        EditorGUI.EndDisabledGroup();
      }
      EditorGUILayout.EndVertical();
    }

    /// <summary> Draws a ColorFieldWithResetAttribute with color field and reset using attribute configuration. </summary>
    protected void DrawColorWithReset(string name, string label = null, Color defaultValue = default)
    {
      SerializedDataParameter parameter = UnpackParameter(name);
      if (parameter == null)
        return;

      GUIContent displayLabel = new(label ?? parameter.displayName);

      EditorGUILayout.BeginHorizontal();
      {
        // Override checkbox
        EditorGUI.showMixedValue = parameter.overrideState.hasMultipleDifferentValues;
        EditorGUI.showMixedValue = false;

        bool isOverridden = parameter.overrideState.boolValue;
        EditorGUI.BeginDisabledGroup(!isOverridden);

        EditorGUILayout.BeginHorizontal();
        {
          // Get current value
          Color value = parameter.value.colorValue;

          // Draw color field
          EditorGUI.BeginChangeCheck();
          value = EditorGUILayout.ColorField(displayLabel, value);

          EditorGUI.EndDisabledGroup();
          int oldIndentLevel = EditorGUI.indentLevel;
          EditorGUI.indentLevel = 0;
          EditorGUILayout.PropertyField(parameter.overrideState, GUIContent.none, GUILayout.Width(16));
          EditorGUI.indentLevel = oldIndentLevel;
          EditorGUI.BeginDisabledGroup(!isOverridden);

          // Reset button
          bool isDefault = EqualityComparer<Color>.Default.Equals(value, defaultValue);
          if (ResetButton(defaultValue, !isDefault) == true)
            value = defaultValue;

          // Apply back
          parameter.value.colorValue = value;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUI.EndDisabledGroup();
      }
      EditorGUILayout.EndHorizontal();
    }

    /// <summary> Draws an EnumParameter with dropdown and reset button using generics. </summary>
    /// <typeparam name="T">The enum type</typeparam>
    protected void DrawEnumDropdownWithReset<T>(string name, string label = null, T defaultValue = default) where T : Enum
    {
      SerializedDataParameter parameter = UnpackParameter(name);
      if (parameter == null)
        return;

      GUIContent displayLabel = new(label ?? parameter.displayName);

      EditorGUILayout.BeginHorizontal();
      {
        // Override checkbox
        EditorGUI.showMixedValue = parameter.overrideState.hasMultipleDifferentValues;
        EditorGUI.showMixedValue = false;

        bool isOverridden = parameter.overrideState.boolValue;
        EditorGUI.BeginDisabledGroup(!isOverridden);

        EditorGUILayout.BeginHorizontal();
        {
          // Get current value (stored as int)
          int currentInt = parameter.value.intValue;

          // Validate bounds (safety check)
          Array enumValues = Enum.GetValues(typeof(T));
          if (currentInt < 0 || currentInt >= enumValues.Length)
            currentInt = 0;

          // Convert int to enum
          T currentValue = (T)enumValues.GetValue(currentInt);

          // Draw enum popup
          EditorGUI.BeginChangeCheck();
          T newValue = (T)EditorGUILayout.EnumPopup(displayLabel, currentValue);

          if (EditorGUI.EndChangeCheck() == true)
          {
            // Convert back to int for serialization
            parameter.value.intValue = Convert.ToInt32(newValue);
          }

          EditorGUI.EndDisabledGroup();
          int oldIndentLevel = EditorGUI.indentLevel;
          EditorGUI.indentLevel = 0;
          EditorGUILayout.PropertyField(parameter.overrideState, GUIContent.none, GUILayout.Width(16));
          EditorGUI.indentLevel = oldIndentLevel;
          EditorGUI.BeginDisabledGroup(!isOverridden);

          // Reset button
          bool isDefault = EqualityComparer<T>.Default.Equals(newValue, defaultValue);
          if (ResetButton(defaultValue, !isDefault) == true)
            parameter.value.intValue = Convert.ToInt32(defaultValue);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUI.EndDisabledGroup();
      }
      EditorGUILayout.EndHorizontal();
    }
  }
}
