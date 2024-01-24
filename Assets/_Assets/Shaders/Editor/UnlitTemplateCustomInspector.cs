using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class UnlitTemplateCustomInspector : ShaderGUI
{
    private MaterialProperty surfaceProp;
    private MaterialProperty faceProp;
    private MaterialProperty cubeProp;
    private MaterialProperty cubeTexProp;

    public enum SurfaceType
    {
        Opaque,
        TransparentBlend,
        TransparentCutout
    }

    // For optimizing some overhead rendering based on Culling mode
    public enum FaceRenderingMode
    {
        FrontOnly,
        NoCulling,
        DoubleSided
    }

    public enum CubeReflectionMode
    {
        None,
        Reflect
    }

    public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
    {
        base.AssignNewShaderToMaterial(material, oldShader, newShader);

        if (newShader.name == "Unlit/MyUnlitTemplate")
        {
            UpdateSurfaceType(material);
        }
    }

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        Material material = materialEditor.target as Material;
        material.hideFlags = HideFlags.None;
        surfaceProp = BaseShaderGUI.FindProperty("_SurfaceType", properties, true);
        faceProp = BaseShaderGUI.FindProperty("_FaceRenderingMode", properties, true);
        cubeProp = BaseShaderGUI.FindProperty("_CubeReflection", properties, true);
        cubeTexProp = BaseShaderGUI.FindProperty("_CubeMap", properties, true);

        EditorGUI.BeginChangeCheck();
        surfaceProp.floatValue = (int)(SurfaceType)EditorGUILayout.EnumPopup("Surface type", (SurfaceType)surfaceProp.floatValue);
        faceProp.floatValue = (int)(FaceRenderingMode)EditorGUILayout.EnumPopup("Face rendering mode", (FaceRenderingMode)faceProp.floatValue);
        cubeProp.floatValue = (int)(CubeReflectionMode)EditorGUILayout.EnumPopup("Cube reflection", (CubeReflectionMode)cubeProp.floatValue);
        if (EditorGUI.EndChangeCheck())
        {
            UpdateSurfaceType(material);
        }

        base.OnGUI(materialEditor, properties);
    }

    private void UpdateSurfaceType(Material material)
    {
        // Update Surface type
        SurfaceType surface = (SurfaceType)material.GetFloat("_SurfaceType");

        switch (surface)
        {
            case SurfaceType.Opaque:
                material.renderQueue = (int)RenderQueue.Geometry;
                material.SetOverrideTag("RenderType", "Opaque");
                break;
            case SurfaceType.TransparentCutout:
                material.renderQueue = (int)RenderQueue.AlphaTest;
                material.SetOverrideTag("RenderType", "TransparentCutout");
                break;
            case SurfaceType.TransparentBlend:
                material.renderQueue = (int)RenderQueue.Transparent;
                material.SetOverrideTag("RenderType", "Transparent");
                break;
        }

        switch (surface)
        {
            case SurfaceType.Opaque:
            case SurfaceType.TransparentCutout:
                material.SetInt("_SourceBlend", (int)BlendMode.One);
                material.SetInt("_DestBlend", (int)BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                break;
            case SurfaceType.TransparentBlend:
                material.renderQueue = (int)RenderQueue.Transparent;
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetInt("_SourceBlend", (int)BlendMode.SrcAlpha);
                material.SetInt("_DestBlend", (int)BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                break;
        }

        material.SetShaderPassEnabled("ShadowCaster", surface != SurfaceType.TransparentBlend);

        if (surface == SurfaceType.TransparentCutout)
        {
            material.EnableKeyword("_ALPHA_CUTOUT");
        }
        else
        {
            material.DisableKeyword("_ALPHA_CUTOUT");
        }

        // Update Face rendering mode
        FaceRenderingMode faceRenderingMode = (FaceRenderingMode)material.GetFloat("_FaceRenderingMode");
        if (faceRenderingMode == FaceRenderingMode.FrontOnly)
        {
            material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Back);
        }
        else
        {
            material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        }

        if (faceRenderingMode == FaceRenderingMode.DoubleSided)
        {
            material.EnableKeyword("_DOUBLE_SIDED_NORMALS");
        }
        else
        {
            material.DisableKeyword("_DOUBLE_SIDED_NORMALS");
        }

        // Update Cube reflection mode
        CubeReflectionMode cubeReflectionMode = (CubeReflectionMode)material.GetFloat("_CubeReflection");
        if (cubeReflectionMode == CubeReflectionMode.Reflect)
        {
            material.EnableKeyword("_CUBE_REFLECT");
        }
        else
        {
            material.DisableKeyword("_CUBE_REFLECT");
        }
    }
}
