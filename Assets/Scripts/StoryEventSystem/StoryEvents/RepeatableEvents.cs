using UnityEngine;
using UnityEngine.SceneManagement;

public class RepeatableEvents
{

    public static void SetObjectActive(GameObject obj, bool Active)
    {
        obj.SetActive(Active);
    }

    public static void MoveObject(GameObject obj, Vector3 targetPos)
    {
        obj.transform.position = targetPos;
    }

    public static void RotateObject(GameObject obj, Vector3 rotation)
    {
        obj.transform.rotation = Quaternion.Euler(rotation);
    }

    public static void OpenDoor(GameObject obj,bool open)
    {
        Door door = obj.GetComponent<Door>();
        if(door != null)
        {
            door.OpenDoor(open);

        }
        else
        {
            Debug.LogWarning(obj + " is not a door");
        }
    }

    public static void OpenScene(string name)
    {
        SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
    }

    public static void CloseScene(string name)
    {
        SceneManager.UnloadSceneAsync(name);
    }

    public static void ExitGame()
    {
        Application.Quit();
    }
}
