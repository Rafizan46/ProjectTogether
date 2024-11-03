using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System;

public class LevelItem : MonoBehaviour
{
    public void SetScene(int scene)
    {
        _sceneIndex = scene;

         String scenePath = SceneUtility.GetScenePathByBuildIndex(_sceneIndex);

        if (scenePath == null)
        {
            _label.SetText("");    
            return;
        }

        String fileName = scenePath.Substring(scenePath.LastIndexOf("/") + 1);
        String levelName = fileName.Substring(0, fileName.Length - ".unity".Length);

        _label.SetText(levelName);
    }

    public int GetScene()
    {
        return _sceneIndex;
    }

    public void OnSelected()
    {
        SceneManager.LoadScene(_sceneIndex);
    }

    private int _sceneIndex;

    [SerializeField] private TextMeshProUGUI _label;
}
