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

namespace FronkonGames.Artistic.OilPaint
{
  ///------------------------------------------------------------------------------------------------------------------
  /// <summary>
  ///
  /// Symmetric Nearest Neighbor.
  ///
  /// 🕹️ Documentation: https://fronkongames.github.io/store/artistic.html
  /// 📄 Demo:          https://fronkongames.github.io/demos-artistic/oilpaint/
  /// 📧 Support:       fronkongames@gmail.com
  /// ❤️ More assets:   https://assetstore.unity.com/publishers/62716
  ///
  /// 💡 Do you want to report an error? Please, send the log file along with the mail.
  ///
  /// ❤️ Leave a review if you found this asset useful, thanks! ❤️
  ///
  /// </summary>
  /// <remarks> Only available for Universal Render Pipeline. </remarks>
  ///------------------------------------------------------------------------------------------------------------------
  [DisallowMultipleRendererFeature("Oil Paint: Symmetric Nearest Neighbor")]
  public sealed class SymmetricNearestNeighbor : ScriptableRendererFeature
  {
    private RenderPass renderPass;

    private Material material;

    /// <summary> Initializes this feature's resources. </summary>
    public override void Create() => renderPass ??= new RenderPass();

    /// <summary> Injects one or multiple ScriptableRenderPass in the renderer. Called every frame once per camera. </summary>
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
      if (renderingData.cameraData.cameraType == CameraType.Preview || renderingData.cameraData.cameraType == CameraType.Reflection)
        return;

      if (material == null)
      {
        string shaderPath = "Shaders/SymmetricNearestNeighbor_URP";
        Shader shader = Resources.Load<Shader>(shaderPath);
        if (shader != null)
        {
          if (shader.isSupported == true)
            material = CoreUtils.CreateEngineMaterial(shader);
          else
            Log.Warning($"'{shaderPath}.shader' not supported");
        }
      }

      renderPass.material = material;
      renderer.EnqueuePass(renderPass);
    }

    protected override void Dispose(bool disposing) => CoreUtils.Destroy(material);

    [DisallowMultipleRendererFeature]
    private sealed class RenderPass : ScriptableRenderPass
    {
      internal Material material { get; set; }

      private SymmetricNearestNeighborVolume volume;

      private readonly TextureHandle[] renderTextureHandles = new TextureHandle[4];
      private TextureHandle renderTextureHandlesDetail;

      private AnimationCurve curve = null;
      private Texture2D curveTexture;
      private int curveHash = int.MinValue;

      private static class ShaderIDs
      {
        internal static readonly int Intensity = Shader.PropertyToID("_Intensity");
        internal static readonly int Radius = Shader.PropertyToID("_Radius");
        internal static readonly int DetailStrength = Shader.PropertyToID("_DetailStrength");
        internal static readonly int EmbossStrength = Shader.PropertyToID("_EmbossStrength");
        internal static readonly int EmbossAngle = Shader.PropertyToID("_EmbossAngle");
        internal static readonly int WaterColor = Shader.PropertyToID("_WaterColorStrength");
        internal static readonly int SampleSky = Shader.PropertyToID("_SampleSky");
        internal static readonly int DepthPower = Shader.PropertyToID("_DepthPower");
        internal static readonly int Brightness = Shader.PropertyToID("_Brightness");
        internal static readonly int Contrast = Shader.PropertyToID("_Contrast");
        internal static readonly int Gamma = Shader.PropertyToID("_Gamma");
        internal static readonly int Hue = Shader.PropertyToID("_Hue");
        internal static readonly int Saturation = Shader.PropertyToID("_Saturation");
      }

      private static class Textures
      {
        internal static readonly int CurveTexture = Shader.PropertyToID("_CurveTex");
      }

      private static class Keywords
      {
        internal static readonly string DetailSharpen = "DETAIL_SHARPEN";
        internal static readonly string DetailEmboss = "DETAIL_EMBOSS";
        internal static readonly string WaterColor = "WATER_COLOR";
        internal static readonly string ProcessDepth = "PROCESS_DEPTH";
        internal static readonly string DetailPass = "DETAIL_PASS";
        internal static readonly string ViewDepth = "VIEW_DEPTH";
      }

      public RenderPass() : base()
      {
        profilingSampler = new ProfilingSampler(Constants.Asset.AssemblyName);
        ConfigureInput(ScriptableRenderPassInput.Depth);
      }

      ~RenderPass() => material = null;

      private static int GetCurveHash(AnimationCurve animationCurve)
      {
        if (animationCurve == null)
          return 0;

        unchecked
        {
          int hash = 17;
          hash = hash * 23 + (int)animationCurve.preWrapMode;
          hash = hash * 23 + (int)animationCurve.postWrapMode;

          Keyframe[] keys = animationCurve.keys;
          hash = hash * 23 + keys.Length;

          for (int i = 0; i < keys.Length; ++i)
          {
            Keyframe key = keys[i];
            hash = hash * 23 + key.time.GetHashCode();
            hash = hash * 23 + key.value.GetHashCode();
            hash = hash * 23 + key.inTangent.GetHashCode();
            hash = hash * 23 + key.outTangent.GetHashCode();
            hash = hash * 23 + key.inWeight.GetHashCode();
            hash = hash * 23 + key.outWeight.GetHashCode();
            hash = hash * 23 + (int)key.weightedMode;
          }

          return hash;
        }
      }

      internal void UpdateCurveTexture()
      {
        curve = volume.depthCurve.value;
        curveHash = GetCurveHash(curve);

        CoreUtils.Destroy(curveTexture);

        const int width = 256;
        const int height = 4;
        curveTexture = new Texture2D(width, height, TextureFormat.RGB24, false) { filterMode = FilterMode.Point, wrapMode = TextureWrapMode.Clamp };

        const float inv = 1.0f / (width - 1);
        for (int y = 0; y < height; ++y)
        {
          for (int x = 0; x < width; ++x)
            curveTexture.SetPixel(x, y, Color.white * curve.Evaluate(x * inv));
        }

        curveTexture.Apply();
      }

      private void UpdateMaterial()
      {
        material.shaderKeywords = null;
        material.SetFloat(ShaderIDs.Intensity, volume.intensity.value);
        material.SetInt(ShaderIDs.Radius, volume.radius.value);

        if (volume.detail.value != OilPaint.Detail.None || volume.waterColor.value > 0.0f)
          material.EnableKeyword(Keywords.DetailPass);

        if (volume.processDepth.value == true)
        {
          material.EnableKeyword(Keywords.ProcessDepth);

          AnimationCurve depthCurve = volume.depthCurve.value;
          int currentCurveHash = GetCurveHash(depthCurve);
          if (curveTexture == null || curve != depthCurve || curveHash != currentCurveHash)
            UpdateCurveTexture();

#if UNITY_EDITOR
          if (volume.viewDepthCurve.value == true)
            material.EnableKeyword(Keywords.ViewDepth);
#endif
          material.SetTexture(Textures.CurveTexture, curveTexture);
          material.SetFloat(ShaderIDs.DepthPower, volume.depthPower.value * 0.1f);
          material.SetInt(ShaderIDs.SampleSky, volume.sampleSky.value == true ? 1 : 0);
        }

        material.SetFloat(ShaderIDs.Brightness, volume.brightness.value);
        material.SetFloat(ShaderIDs.Contrast, volume.contrast.value);
        material.SetFloat(ShaderIDs.Gamma, 1.0f / volume.gamma.value);
        material.SetFloat(ShaderIDs.Hue, volume.hue.value);
        material.SetFloat(ShaderIDs.Saturation, volume.saturation.value);
      }

      public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
      {
        volume = VolumeManager.instance.stack.GetComponent<SymmetricNearestNeighborVolume>();
        if (volume == null || volume.IsActive() == false || material == null)
          return;

        UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
        if (resourceData.isActiveTargetBackBuffer == true)
          return;

        UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
        if (cameraData.camera.cameraType == CameraType.SceneView && volume.affectSceneView.value == false || cameraData.postProcessEnabled == false)
          return;

        TextureHandle source = resourceData.activeColorTexture;
        TextureDesc sourceDesc = source.GetDescriptor(renderGraph);
        float renderSize = Mathf.Max(0.01f, volume.renderSize.value);
        sourceDesc.width = (int)(cameraData.cameraTargetDescriptor.width * renderSize);
        sourceDesc.height = (int)(cameraData.cameraTargetDescriptor.height * renderSize);
        sourceDesc.clearBuffer = false;
        sourceDesc.depthBufferBits = 0;

        UpdateMaterial();

        int i = 0;
        for (; i < volume.passes.value; ++i)
          renderTextureHandles[i] = renderGraph.CreateTexture(sourceDesc);

        for (i = 0; i < volume.passes.value; ++i)
          renderGraph.AddBlitPass(new RenderGraphUtils.BlitMaterialParameters(i == 0 ? source : renderTextureHandles[i - 1], renderTextureHandles[i], material, 0), $"{Constants.Asset.AssemblyName}.Pass{i}");

        TextureHandle lastRenderTarget = renderTextureHandles[i - 1];

        if (volume.detail.value != OilPaint.Detail.None || volume.waterColor.value > 0.0f)
        {
          renderTextureHandlesDetail = renderGraph.CreateTexture(sourceDesc);

          material.SetFloat(ShaderIDs.DetailStrength, volume.detailStrength.value);

          switch (volume.detail.value)
          {
            case OilPaint.Detail.None: break;
            case OilPaint.Detail.Sharpen: material.EnableKeyword(Keywords.DetailSharpen); break;
            case OilPaint.Detail.Emboss:
              material.EnableKeyword(Keywords.DetailEmboss);
              material.SetFloat(ShaderIDs.EmbossStrength, volume.embossStrength.value);
              material.SetFloat(ShaderIDs.EmbossAngle, volume.embossAngle.value * Mathf.Deg2Rad);
              break;
          }

          if (volume.waterColor.value > 0.0f)
          {
            material.EnableKeyword(Keywords.WaterColor);
            material.SetFloat(ShaderIDs.WaterColor, volume.waterColor.value);
          }

          renderGraph.AddBlitPass(new RenderGraphUtils.BlitMaterialParameters(lastRenderTarget, renderTextureHandlesDetail, material, 1), $"{Constants.Asset.AssemblyName}.PassDetail");
          lastRenderTarget = renderTextureHandlesDetail;
        }

        if (renderSize < 1.0f)
        {
          TextureDesc targetDesc = source.GetDescriptor(renderGraph);
          targetDesc.clearBuffer = false;
          targetDesc.depthBufferBits = 0;
          TextureHandle upscaleTarget = renderGraph.CreateTexture(targetDesc);
          renderGraph.AddBlitPass(lastRenderTarget, upscaleTarget, Vector2.one, Vector2.zero);
          lastRenderTarget = upscaleTarget;
        }

        resourceData.cameraColor = lastRenderTarget;
      }
    }
  }
}
