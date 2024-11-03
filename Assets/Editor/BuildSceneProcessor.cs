using UnityEditor;
using System.Collections.Generic;

// docs: https://docs.unity3d.com/ScriptReference/AssetModificationProcessor.OnWillSaveAssets.html
public class BuildSceneProcessor : AssetModificationProcessor
{
    public static void OnWillCreateAsset(string path)
    {
        if (path.EndsWith(".unity.meta"))
            path = path.Substring(0, path.Length - 5);

        string scenePath = ProcessAssetsForScenes(new string[] { path });
        if (!string.IsNullOrEmpty(scenePath)) AddSceneToBuildSettings(scenePath);
    }

    public static string[] OnWillSaveAssets(string[] paths)
    {
        string scenePath = ProcessAssetsForScenes(paths);
        if (!string.IsNullOrEmpty(scenePath)) AddSceneToBuildSettings(scenePath);

        return paths;
    }

    public static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions options)
    {
        string scenePath = ProcessAssetsForScenes(new string[] { path });
        if (!string.IsNullOrEmpty(scenePath)) RemoveSceneFromBuildSettings(scenePath);

        return AssetDeleteResult.DidNotDelete;
    }

    private static string ProcessAssetsForScenes(string[] paths)
    {
        string scenePath = string.Empty;

        foreach (string path in paths)
        {
            if (path.Contains(".unity"))
                scenePath = path;
        }

        return scenePath;
    }

    private static void AddSceneToBuildSettings(string scenePath)
    {
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

        foreach (EditorBuildSettingsScene scene in scenes)
        {
            if (scene.path == scenePath)
                return;
        }

        EditorBuildSettingsScene newScene = new EditorBuildSettingsScene();
        newScene.path = scenePath;
        newScene.enabled = true;

        scenes.Add(newScene);
        EditorBuildSettings.scenes = scenes.ToArray();
    }

    private static void RemoveSceneFromBuildSettings(string scenePath)
    {
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

        scenes.RemoveAll(scene => scene.path == scenePath);

        EditorBuildSettings.scenes = scenes.ToArray();
    }
}