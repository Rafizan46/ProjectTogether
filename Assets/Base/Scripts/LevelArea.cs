using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelArea : MonoBehaviour
{
    public void Refresh() => Refresh("");

    public void Refresh(string searchParam)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            String scenePath = SceneUtility.GetScenePathByBuildIndex(i);

            if (scenePath == null) continue;

            String fileName = scenePath.Substring(scenePath.LastIndexOf("/") + 1);
            String levelName = fileName.Substring(0, fileName.Length - ".unity".Length);

            if (levelName == SceneManager.GetActiveScene().name) continue;
            if (!levelName.ToLower().StartsWith(searchParam.ToLower())) continue;

            LevelItem newLevelItem = Instantiate(_levelItemPrefab, transform);
            newLevelItem.SetScene(i);
        }
    }

    public void RefreshFromSearchField() => Refresh(_searchField.text);

    [SerializeField] private LevelItem _levelItemPrefab;
    [SerializeField] private TMP_InputField _searchField;

    void Awake()
    {
        Refresh();
    }
}
