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

    public static void OpenDoor(GameObject obj, bool open)
    {
        Door door = obj.GetComponent<Door>();
        if (door != null)
        {
            door.OpenDoor(open);

        }
        else
        {
            Debug.LogWarning(obj + " is not a door");
        }
    }

    public static void StairCaseFall(GameObject staircase)
    {
        Collider stairCollision = staircase.GetComponentInChildren<Collider>();
        Rigidbody[] children = staircase.GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < children.Length; i++)
        {
            children[i].mass = Random.Range(90, 140);
            children[i].useGravity = true;
            children[i].isKinematic = false;
        }
        stairCollision.enabled = false;
        GameObject.Destroy(staircase, 5);
    }

    public static void OpenScene(string name)
    {
        SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
    }

    public static void CloseScene(string name)
    {
        SceneManager.UnloadSceneAsync(name);
    }

    public static void SetElevator(GameObject elevator)
    {
        elevator.GetComponent<Elevator>().StartPlatform();
    }

    public static void ExitGame()
    {
        SceneManager.LoadScene(0);
    }
}
