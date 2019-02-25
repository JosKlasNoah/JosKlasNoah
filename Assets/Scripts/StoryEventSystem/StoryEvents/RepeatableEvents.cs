using UnityEngine;
using UnityEngine.SceneManagement;

public class RepeatableEvents
{

    public static void Remove(GameObject removeWhat)
    {
        removeWhat.SetActive(false);
    }

    public static void Activate(GameObject activateWhat)
    {
        activateWhat.SetActive(true);
    }

    public static void MoveSomething(GameObject what, Vector3 where)
    {
        what.transform.position = where;
    }

    public static void OpenScene(string name)
    {
        SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
    }

    public static void CloseScene(string name)
    {
        SceneManager.UnloadSceneAsync(name);
    }

    public static void End()
    {
        Application.Quit();
    }
}
