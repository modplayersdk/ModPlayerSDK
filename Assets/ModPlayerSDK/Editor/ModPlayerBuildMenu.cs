using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class ModPlayerBuildMenu : EditorWindow
{
    [MenuItem("ModPlayer/Publish")]
    public static void ShowPublishWindow()
    {
        var win = new ModPlayerBuildMenu();
        win.maxSize = new Vector2(450, 300);
        win.minSize = new Vector2(450, 300);
        win.Show();
    }

    private Texture2D logo;

    private string modName;
    private string description;

    public void OnGUI()
    {
        EnsureResources();

        GUI.DrawTexture(new Rect(0, 0, 450, 400), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(0, 0, 450, 100), logo);

        EditorGUILayout.BeginVertical();
        GUILayout.Space(110);
        modName = EditorGUILayout.TextField("Mod name", modName);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Description", GUILayout.Width(146));
        description = EditorGUILayout.TextArea(description, 
            GUILayout.Height(40));
        EditorGUILayout.EndHorizontal();

        if (GUI.Button(new Rect(340, 270, 100, 30), "Publish"))
        {
            if (string.IsNullOrEmpty(modName) == false)
                BuildMod();
        }

        EditorGUILayout.EndVertical();
    }

    private void EnsureResources()
    {
        if (logo == null)
            logo = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ModPlayerSDK/Images/modplayer.png");
    }

    private void BuildMod()
    {
        var scene = EditorSceneManager.GetActiveScene();

        AssetImporter.GetAtPath(scene.path)
            .SetAssetBundleNameAndVariant($"{modName}_scene", "");

        MergeCsx.CreateMonolith(
            "Assets/modplayer_script.json",
            $"{modName}_script");

        UniAssetBundle.BuildUniScriptScene(
            $"{modName}_scene", $"{modName}_script");
    }
}
