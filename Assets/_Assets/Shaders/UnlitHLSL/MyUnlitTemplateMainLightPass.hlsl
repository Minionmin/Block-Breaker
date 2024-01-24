#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
#include "MyUnlitCommon.hlsl"

//TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap); // Macro setting for texture

CBUFFER_START(UnityPerMaterial)
// xy = x and y scale, zw = x and y offset
float4 _BaseMap_ST; // This is Automatically set by Unity. Used in TRANSFORM_TEX to apply UV tiling.
half4 _Color; // Tint
float4 _EmissionColor;
half _Cutoff; // Alpha cutout threshold
samplerCUBE _CubeMap; // Cube texture
half _CubeStrength; // Cube Light Strength
half _CubeReflectRate; // Cube Reflect Rate
half _CubeBlur; // Cube Blurriness
half4 _SpecColor; // Specular color
half _SpecStrength; // Specular strength
half _SpecSmoothness; // Specular smoothness
half _Atten; // Attenuation
half _Occlusion; // Ambient occlusion
// half _hasCubeMap; // Bool should has cube texture or not
CBUFFER_END

// Vertex struct
struct Attributes
{
    float4 positionOS : POSITION; // Local(Object) Space
    float4 color : COLOR;
    float3 normalOS : NORMAL;
    float2 uv : TEXCOORD0;
    float2 lightmapUV : TEXCOORD1;
};

// Fragment struct
struct Varyings
{
    float4 positionHCS : SV_POSITION; // Clip Space
    float4 color : COLOR;
    float3 positionWS : TEXCOORD0; // World Space
    DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);
    float2 uv : TEXCOORD2;
    float3 normalWS : TEXCOORD3;
    
#ifdef _ADDITIONAL_LIGHTS_VERTEX
	half4 fogFactorAndVertexLight : TEXCOORD4; // x: fogFactor, yzw: vertex light
#else
    half fogFactor : TEXCOORD4;
#endif

    float3 worldRefl : TEXCOORD5;
};

// Vertex Shader
Varyings vert(Attributes IN)
{
    Varyings OUT;
    
    // These helper functions, found in URP/ShaderLib/ShaderVariableFunctions.hlsl
    // Transform object space values into world and clip space
    VertexPositionInputs posInputs = GetVertexPositionInputs(IN.positionOS.xyz);
    VertexNormalInputs normInputs = GetVertexNormalInputs(IN.normalOS);

    half3 vertexLight = VertexLighting(posInputs.positionWS, normInputs.normalWS);
    half fogFactor = ComputeFogFactor(posInputs.positionCS.z);
    
    // Pass position and orientation data to the fragment function
    OUT.positionHCS = posInputs.positionCS; // Clip Space
    OUT.positionWS = posInputs.positionWS; // World Space
    OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
    OUT.normalWS = normInputs.normalWS;
    OUT.color = IN.color;
    
    // Light map uv
    OUTPUT_LIGHTMAP_UV(IN.lightmapUV, unity_LightmapST, OUT.lightmapUV);
    OUTPUT_SH(OUT.normalWS.xyz, OUT.vertexSH);
    
#ifdef _ADDITIONAL_LIGHTS_VERTEX
	OUT.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
#else
    OUT.fogFactor = fogFactor;
#endif
    
     // World Reflection
    half3 viewDirWS = GetWorldSpaceNormalizeViewDir(posInputs.positionWS);
    OUT.worldRefl = reflect(-viewDirWS, OUT.normalWS);
    
    return OUT;
}

// Fragment Shader
half4 frag(Varyings IN
#ifdef _DOUBLE_SIDED_NORMALS
    , FRONT_FACE_TYPE frontFace : FRONT_FACE_SEMANTIC
#endif
) : SV_Target
{
    half2 uv = IN.uv;
    half4 texCol = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);
    TestAlphaClip(texCol, _Color, _Cutoff);
    
    half4 cubeCol = texCUBElod(_CubeMap, float4(IN.worldRefl, _CubeBlur));
    
    // Input Data
    InputData lightingInput = (InputData) 0;
    lightingInput.positionWS = IN.positionWS;

    float3 normalWS = NormalizeNormalPerPixel(IN.normalWS);
    
#ifdef _DOUBLE_SIDED_NORMALS
    normalWS *= IS_FRONT_VFACE(frontFace, 1, -1);
#endif
    
    // For lighting to look it's best, all normals should be normalized
    lightingInput.normalWS = normalWS;
    lightingInput.viewDirectionWS = GetWorldSpaceNormalizeViewDir(IN.positionWS);
    lightingInput.shadowCoord = TransformWorldToShadowCoord(IN.positionWS);

    // Fog
#ifdef _ADDITIONAL_LIGHTS_VERTEX
	lightingInput.fogCoord = IN.fogFactorAndVertexLight.x;
	lightingInput.vertexLighting = IN.fogFactorAndVertexLight.yzw;
#else
    lightingInput.fogCoord = IN.fogFactor;
    lightingInput.vertexLighting = half3(0, 0, 0);
#endif
    
    lightingInput.bakedGI = SAMPLE_GI(IN.lightmapUV, IN.vertexSH, lightingInput.normalWS);
    lightingInput.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.positionHCS);
    lightingInput.shadowMask = SAMPLE_SHADOWMASK(IN.lightmapUV);
    
    // Surface Data
    SurfaceData surfaceInput = (SurfaceData) 0;
    surfaceInput.albedo = texCol.rgb * _Color.rgb * _Atten;
    surfaceInput.alpha = texCol.a * _Color.a;
    surfaceInput.occlusion = _Occlusion;
    
#ifdef _SPECULAR_COLOR
    surfaceInput.specular = _SpecColor.rgb * _SpecStrength;
#else
    surfaceInput.specular = _SpecStrength;
#endif
    surfaceInput.smoothness = _SpecSmoothness; // The higher the smoothness, the more specular will be focused
    
#if UNITY_VERSION >= 202120
    half4 color = UniversalFragmentBlinnPhong(lightingInput, surfaceInput);
#else
    half4 color = UniversalFragmentBlinnPhong(lightingInput, surfaceInput.albedo, float4(surfaceInput.specular, 1), surfaceInput.smoothness);
#endif

    color.rgb = MixFog(color.rgb, lightingInput.fogCoord);
    
#ifdef _CUBE_REFLECT
    color.rgb = lerp(color.rgb, color.rgb * cubeCol.rgb * _CubeStrength, _CubeReflectRate);
#endif
    
    return color;
}