Shader "Unlit/MyUnlitTemplate"
{
    Properties
    {
        [Header(Texture)]
        [MainTexture] _BaseMap ("Main Texture", 2D) = "white" {}
        [CubeTexture] _CubeMap ("Cube Texture", CUBE) = "white" {}

        [Header(Cube Parameters)]
        _CubeStrength ("Cube strength", Range(0.1, 100)) = 1
        _CubeReflectRate ("Cube reflect Rate", Range(0, 1)) = 0
        _CubeBlur ("Cube blurriness", Range(1, 7)) = 1

        [Header(Tint)]
        [HDR] _Color ("Tint", Color) = (1,1,1,1)
        _Cutoff ("Alpha cutout threshold", Range(0,1)) = 0.5

        [Header(Specular Parameters)]
        _SpecColor ("Specular Color", Color) = (1,1,1,1)
        _SpecStrength ("Specular strength", Range(0, 5)) = 1
        _SpecSmoothness ("Specular smoothness", Range(0,2)) = 1

        [Header(Intensity)]
        _Atten ("Light attenuation", Range(0.1, 5)) = 1
        _Occlusion ("Ambient occlusion", Range(0.0, 1.0)) = 1

        [HideInInspector] _Cull ("Cull mode", Float) = 2
        [HideInInspector] _SourceBlend("Source blend", Float) = 0
        [HideInInspector] _DestBlend("Destination blend", Float) = 0
        [HideInInspector] _ZWrite("ZWrite", Float) = 0

        [HideInInspector] _SurfaceType("Surface type", Float) = 0
        [HideInInspector] _FaceRenderingMode("Face rendering type", Float) = 0
        [HideInInspector] _CubeReflection("Cube reflection", Float) = 0

        // [Header(Other)]
        // [Toggle] _hasCubeMap ("Has cube map or not", Float) = 0
    }
    SubShader
    {
        Tags 
        { 
            "RenderPipeline" = "UniversalPipeline" 
            "RenderType" = "Opaque" 
        }

        // Main Lighting Pass
        Pass
        {
            Name "ForwardLit"
            Tags
            {
                "LightMode" = "UniversalForward"
            }

            // Transparency setting
            Blend[_SourceBlend][_DestBlend] // Blend Src Dest
            ZWrite[_ZWrite] // Prevent the rasterizer from storing any of the passed data in the depth buffer so this get drawn no matter what
            // Without a depth buffer, draw order affects what ends up on the screen
            // (This Pass might not be drawn on the screen in the end if it is drawn first (the next object will be drawn and replaced this object))
            // This can be fixed by using Render Queues
            Cull[_Cull]

            HLSLPROGRAM

            // Material Keywords
            #define _SPECULAR_COLOR // always on

            #pragma shader_feature_local _ALPHA_CUTOUT
            #pragma shader_feature_local _DOUBLE_SIDED_NORMALS
            #pragma shader_feature_local _CUBE_REFLECT

            // URP Keywords
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS // AdditionalLight
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK

#if UNITY_VERSION >= 202120
            // Can be enabled in C# script. Ex: Material m; m.EnableKeyword("_MAIN_LIGHT_SHADOWS") or m.DisableKeyword("_MAIN_LIGHT_SHADOWS")
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE // Make another variant with/without Main Light. _ refers to "No Keyword"
#else
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
#endif
            #pragma multi_compile_fragment _ _SHADOWS_SOFT

            // Unity Keywords
            #pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile_fog

            #pragma vertex vert
            #pragma fragment frag

            #include "MyUnlitTemplateMainLightPass.hlsl"
            ENDHLSL
        }

        // Shadow Caster Pass
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

            ColorMask 0 // Since Shadow does not use color, only shadow mapping distance
            Cull[_Cull]

            HLSLPROGRAM

            #pragma shader_feature_local _ALPHA_CUTOUT

            #pragma vertex vert
            #pragma fragment frag

            #include "MyUnlitTemplateShadowCasterPass.hlsl"
            ENDHLSL
        }

    }

    CustomEditor "UnlitTemplateCustomInspector"
}