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
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;

namespace FronkonGames.Artistic.OneBit
{
  ///------------------------------------------------------------------------------------------------------------------
  /// <summary> Render Pass. </summary>
  /// <remarks> Only available for Universal Render Pipeline. </remarks>
  ///------------------------------------------------------------------------------------------------------------------
  public sealed partial class OneBit
  {
    [DisallowMultipleRendererFeature]
    private sealed class RenderPass : ScriptableRenderPass
    {
      // Internal use only.
      internal Material material { get; set; }

      private OneBitVolume volume;

      private bool forceGradientTextureUpdate;

      private Gradient gradient = null;
      private Texture2D gradientTexture;

      private static class ShaderIDs
      {
        internal static readonly int Intensity = Shader.PropertyToID("_Intensity");

        internal static readonly int Edges = Shader.PropertyToID("_Edges");
        internal static readonly int NoiseStrength = Shader.PropertyToID("_NoiseStrength");
        internal static readonly int NoiseSeed = Shader.PropertyToID("_NoiseSeed");
        internal static readonly int BlendMode = Shader.PropertyToID("_Blend");
        internal static readonly int Color = Shader.PropertyToID("_Color");
        internal static readonly int Color0 = Shader.PropertyToID("_Color0");
        internal static readonly int Color1 = Shader.PropertyToID("_Color1");
        internal static readonly int LuminanceMin = Shader.PropertyToID("_LumRangeMin");
        internal static readonly int LuminanceMax = Shader.PropertyToID("_LumRangeMax");
        internal static readonly int CircularRadius = Shader.PropertyToID("_GradientRadius");
        internal static readonly int HorizontalOffset = Shader.PropertyToID("_GradientHorizontalOffset");
        internal static readonly int VerticalOffset = Shader.PropertyToID("_GradientVerticalOffset");
        internal static readonly int RedCount = Shader.PropertyToID("_RedCount");
        internal static readonly int GreenCount = Shader.PropertyToID("_GreenCount");
        internal static readonly int BlueCount = Shader.PropertyToID("_BlueCount");
        internal static readonly int InvertColor = Shader.PropertyToID("_InvertColor");

        internal static readonly int Brightness = Shader.PropertyToID("_Brightness");
        internal static readonly int Contrast = Shader.PropertyToID("_Contrast");
        internal static readonly int Gamma = Shader.PropertyToID("_Gamma");
        internal static readonly int Hue = Shader.PropertyToID("_Hue");
        internal static readonly int Saturation = Shader.PropertyToID("_Saturation");
      }

      private static class Keywords
      {
        internal static readonly string ColorModeSolid = "COLORMODE_SOLID";
        internal static readonly string ColorModeGradient = "COLORMODE_GRADIENT";
        internal static readonly string ColorModeHorizontal = "COLORMODE_HORIZONTAL";
        internal static readonly string ColorModeVertical = "COLORMODE_VERTICAL";
        internal static readonly string ColorModeCircular = "COLORMODE_CIRCULAR";
      }

      private static class Textures
      {
        internal static readonly int GradientTexture = Shader.PropertyToID("_GradientTex");
      }

      /// <summary> Render pass constructor. </summary>
      public RenderPass() : base() => profilingSampler = new ProfilingSampler(Constants.Asset.AssemblyName);

      /// <summary> Destroy the render pass. </summary>
      ~RenderPass() => material = null;

      /// <summary> Update gradient texture. </summary>
      public void UpdateGradientTexture()
      {
        gradient = volume.gradient.value;

        const int width = 256;
        const int height = 4;
        gradientTexture = new Texture2D(width, height, TextureFormat.RGB24, false) { filterMode = FilterMode.Point, wrapMode = TextureWrapMode.Clamp };

        const float inv = 1.0f / (width - 1);
        for (int y = 0; y < height; ++y)
        {
          for (int x = 0; x < width; ++x)
            gradientTexture.SetPixel(x, y, gradient.Evaluate(x * inv));
        }

        gradientTexture.Apply();

        forceGradientTextureUpdate = false;
      }

      private void UpdateMaterial()
      {
        material.shaderKeywords = null;
        material.SetFloat(ShaderIDs.Intensity, volume.intensity.value);

        material.SetFloat(ShaderIDs.Edges, volume.edges.value * 100.0f);
        material.SetFloat(ShaderIDs.NoiseStrength, volume.noiseStrength.value);
        material.SetFloat(ShaderIDs.NoiseSeed, volume.noiseSeed.value);

        switch (volume.colorMode.value)
        {
          case ColorModes.Solid:
            material.EnableKeyword(Keywords.ColorModeSolid);
            material.SetColor(ShaderIDs.Color, volume.color.value);
            break;
          case ColorModes.Gradient:
            if (forceGradientTextureUpdate == true || gradient == null || gradient != volume.gradient.value)
              UpdateGradientTexture();

            material.EnableKeyword(Keywords.ColorModeGradient);
            material.SetTexture(Textures.GradientTexture, gradientTexture);
            material.SetFloat(ShaderIDs.LuminanceMin, volume.luminanceMin.value);
            material.SetFloat(ShaderIDs.LuminanceMax, volume.luminanceMax.value);
            break;
          case ColorModes.Horizontal:
            material.EnableKeyword(Keywords.ColorModeHorizontal);
            material.SetColor(ShaderIDs.Color0, volume.color0.value);
            material.SetColor(ShaderIDs.Color1, volume.color1.value);
            material.SetFloat(ShaderIDs.HorizontalOffset, volume.horizontalOffset.value - 1.0f);
            break;
          case ColorModes.Vertical:
            material.EnableKeyword(Keywords.ColorModeVertical);
            material.SetColor(ShaderIDs.Color0, volume.color0.value);
            material.SetColor(ShaderIDs.Color1, volume.color1.value);
            material.SetFloat(ShaderIDs.VerticalOffset, volume.verticalOffset.value - 1.0f);
            break;
          case ColorModes.Circular:
            material.EnableKeyword(Keywords.ColorModeCircular);
            material.SetColor(ShaderIDs.Color0, volume.color0.value);
            material.SetColor(ShaderIDs.Color1, volume.color1.value);
            material.SetFloat(ShaderIDs.CircularRadius, volume.circularRadius.value);
            break;
        }

        material.SetInt(ShaderIDs.RedCount, volume.redCount.value + 1);
        material.SetInt(ShaderIDs.GreenCount, volume.greenCount.value + 1);
        material.SetInt(ShaderIDs.BlueCount, volume.blueCount.value + 1);

        material.SetInt(ShaderIDs.InvertColor, volume.invertColor.value == true ? 1 : 0);

        material.SetInt(ShaderIDs.BlendMode, (int)volume.blendMode.value);

        material.SetFloat(ShaderIDs.Brightness, volume.brightness.value);
        material.SetFloat(ShaderIDs.Contrast, volume.contrast.value);
        material.SetFloat(ShaderIDs.Gamma, 1.0f / volume.gamma.value);
        material.SetFloat(ShaderIDs.Hue, volume.hue.value);
        material.SetFloat(ShaderIDs.Saturation, volume.saturation.value);
      }

      /// <inheritdoc/>
      public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
      {
        volume = VolumeManager.instance.stack.GetComponent<OneBitVolume>();
        if (volume == null || volume.IsActive() == false || material == null)
          return;

        UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
        if (resourceData.isActiveTargetBackBuffer == true)
          return;

        UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
        if (cameraData.camera.cameraType == CameraType.SceneView && volume.affectSceneView.value == false || cameraData.postProcessEnabled == false)
          return;

        TextureHandle source = resourceData.activeColorTexture;
        TextureHandle destination = renderGraph.CreateTexture(source.GetDescriptor(renderGraph));

        UpdateMaterial();

        RenderGraphUtils.BlitMaterialParameters pass = new(source, destination, material, 0);
        renderGraph.AddBlitPass(pass, $"{Constants.Asset.AssemblyName}.Pass");

        resourceData.cameraColor = destination;
      }
    }
  }
}
