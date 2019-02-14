using UnityEngine;

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

    public static void End()
    {
        Application.Quit();
    }
}
