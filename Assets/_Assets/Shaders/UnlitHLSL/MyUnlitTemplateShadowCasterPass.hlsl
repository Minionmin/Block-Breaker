#ifndef MY_UNLIT_TEMPLATE_SHADOW_CASTER_PASS_INCLUDED
#define MY_UNLIT_TEMPLATE_SHADOW_CASTER_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
#include "MyUnlitCommon.hlsl"

CBUFFER_START(UnityPerMaterial)
float4 _BaseMap_ST;
half4 _Color;
float4 _EmissionColor;
half _Cutoff;
half4 _SpecColor;
half _SpecStrength;
half _SpecSmoothness;
half _Atten;
CBUFFER_END

// Vertex struct
struct Attributes
{
    float4 positionOS : POSITION;
    float3 normalOS : NORMAL;
#ifdef _ALPHA_CUTOUT
    float2 uv :TEXCOORD0;
#endif
};

// Fragment struct
struct Varyings
{
    float4 positionHCS : SV_POSITION;
#ifdef _ALPHA_CUTOUT
    float2 uv :TEXCOORD0;
#endif
};

float3 FlipNormalBasedOnViewDir(float3 normalWs, float3 positionWS)
{
    float3 viewDirWS = GetWorldSpaceNormalizeViewDir(positionWS);
    return normalWs * (dot(normalWs, viewDirWS) < 0 ? -1 : 1);
}

float3 _LightDirection; // URP Global Variable

// For fixing shadow acne
float4 GetShadowCasterPositionCS(float3 positionWS, float3 normalWS)
{
    float3 lightDirectionWS = _LightDirection;
    
#ifdef _DOUBLE_SIDED_NORMALS
    normalWS = FlipNormalBasedOnViewDir(normalWS, positionWS);
#endif
    
    float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));
    
#if UNITY_REVERSED_Z
    positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
#else
    positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
#endif
    
    return positionCS;
}

// Vertex Shader
Varyings vert(Attributes IN)
{
    Varyings OUT;
    
    VertexPositionInputs posInputs = GetVertexPositionInputs(IN.positionOS.xyz);
    VertexNormalInputs normInputs = GetVertexNormalInputs(IN.normalOS);
    
    OUT.positionHCS = GetShadowCasterPositionCS(posInputs.positionWS, normInputs.normalWS);
    
#ifdef _ALPHA_CUTOUT
    OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
#endif
    
    return OUT;
}

// Fragment Shader
half4 frag(Varyings IN) : SV_Target
{
#ifdef _ALPHA_CUTOUT
    float2 uv = IN.uv;
    float4 texCol = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);
    TestAlphaClip(texCol, _Color, _Cutoff);
#endif
    
    return  0;
}
#endif