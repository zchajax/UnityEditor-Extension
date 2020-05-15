using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class FindShaderUse : EditorWindow
{
    [MenuItem("Tools/Shader Use Find")]
    public static void Open()
    {
        GetWindow<FindShaderUse>();
    }

    Shader shader;
    List<string> materials = new List<string>();
    Vector2 scroll;

    void OnGUI()
    {
        Shader prev = shader;


        GUILayout.BeginHorizontal();
        shader = EditorGUILayout.ObjectField(shader, typeof(Shader), false) as Shader;
        if (GUILayout.Button("Find"))
        {
            string shaderPath = AssetDatabase.GetAssetPath(shader);
            string[] allMaterials = AssetDatabase.FindAssets("t:Material");
            materials.Clear();
            for (int i = 0; i < allMaterials.Length; i++)
            {
                allMaterials[i] = AssetDatabase.GUIDToAssetPath(allMaterials[i]);
                string[] dep = AssetDatabase.GetDependencies(allMaterials[i]);
                if (ArrayUtility.Contains(dep, shaderPath))
                    materials.Add(allMaterials[i]);
            }
        }
        GUILayout.EndHorizontal();

        scroll = GUILayout.BeginScrollView(scroll);
        {
            for (int i = 0; i < materials.Count; i++)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(Path.GetFileNameWithoutExtension(materials[i]));
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Show"))
                        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(materials[i], typeof(Material)));
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndScrollView();
    }
}
