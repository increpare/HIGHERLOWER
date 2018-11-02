// Standard shader with triplanar mapping
// https://github.com/keijiro/StandardTriplanar

using UnityEngine;
using UnityEditor;

public class StandardTriplanarInspector : ShaderGUI
{
    static class Styles
    {
        static public readonly GUIContent albedo_wand = new GUIContent("Albedo Wand", "Albedo Wand (RGB)");
        static public readonly GUIContent albedo_boden = new GUIContent("Albedo Boden", "Albedo Boden (RGB)");
    }

    bool _initialized;

    public override void OnGUI(MaterialEditor editor, MaterialProperty[] props)
    {
        EditorGUI.BeginChangeCheck();

        editor.TexturePropertySingleLine(
            Styles.albedo_wand, FindProperty("_MainTex", props), FindProperty("_Color", props)
        );

        editor.TexturePropertySingleLine(
            Styles.albedo_boden, FindProperty("_MainTex2", props), FindProperty("_Color2", props)
        );

        editor.ShaderProperty(FindProperty("_Metallic", props), "Metallic");
        editor.ShaderProperty(FindProperty("_Glossiness", props), "Smoothness");



        editor.ShaderProperty(FindProperty("_MapScale", props), "Texture Scale");

        if (EditorGUI.EndChangeCheck() || !_initialized)
            foreach (Material m in editor.targets)
                SetMaterialKeywords(m);

        _initialized = true;
    }

    static void SetMaterialKeywords(Material material)
    {
    }

    static void SetKeyword(Material m, string keyword, bool state)
    {
        if (state)
            m.EnableKeyword(keyword);
        else
            m.DisableKeyword(keyword);
    }
}
