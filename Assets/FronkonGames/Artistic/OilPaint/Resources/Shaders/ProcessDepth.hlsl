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
#ifndef OILPAINT_PROCESS_DEPTH
#define OILPAINT_PROCESS_DEPTH

TEXTURE2D_X(_CurveTex);
TEXTURE2D(_CameraDepthTexture);
SAMPLER(sampler_CameraDepthTexture);

int _SampleSky;
float _DepthPower;

inline float SampleDepth(float2 uv)
{
  return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, uv);
}

inline float SampleLinear01Depth(float2 uv)
{
  return Linear01Depth(SampleDepth(uv), _ZBufferParams);
}

inline float SampleDepthCurve(float2 uv)
{
  const float depth = SampleLinear01Depth(uv);
  return SAMPLE_TEXTURE2D_X(_CurveTex, sampler_LinearClamp, float2(SafePositivePow_float(depth, _DepthPower), 0.5)).x;
}

inline uint CalculateRadius(uint radius, float2 uv)
{
  return round(lerp(1, radius, SampleDepthCurve(uv)));
}

inline float CalculateDetail(float detail, float2 uv)
{
  return lerp(0.0, detail, SampleDepthCurve(uv));
}

inline half3 Palette(in float t, in half3 a, in half3 b, in half3 c, in half3 d)
{
  return a + b * cos(6.28318 * (c * t + d));
}

inline half4 ViewRadius(uint radius, float2 uv)
{
  float palette = (float)CalculateRadius(radius, uv) / (float)radius;

  return half4(Palette(palette, half3(0.5, 0.5,  0.5),
                                half3(0.5, 0.5,  0.5),
                                half3(0.8, 0.8,  0.8),
                                half3(0.0, 0.33, 0.67) + 0.21), 1.0);
}

#endif