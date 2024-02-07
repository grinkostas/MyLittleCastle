using UnityEngine;
using UnityEngine.SceneManagement;


public static class CastleScenes
{
    private static readonly string _sceneSaveName = "CurrentScene";
    private static readonly string _defaultScene = "GameScene";
    
    public static string GetCurrentSceneName()
    {
        return ES3.Load(_sceneSaveName, defaultValue:_defaultScene);
    }

    public static void ChangeCurrentGameScene(string sceneName)
    {
        ES3.Save(_sceneSaveName, sceneName);
    }

    public static string GetOpenedSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public static void LoadCurrentGameScene()
    {
        if (GetOpenedSceneName() == GetCurrentSceneName())
            return;
        SceneManager.LoadScene(GetCurrentSceneName());
    }
}
