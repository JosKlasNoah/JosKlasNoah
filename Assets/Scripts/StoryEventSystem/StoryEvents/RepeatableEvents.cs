using UnityEngine;

public class RepeatableEvents : MonoBehaviour
{
    [SerializeField] Vector3 where = new Vector3();

    public void Remove(GameObject removeWhat)
    {
        removeWhat.SetActive(false);
    }

    public void Activate(GameObject activateWhat)
    {
        activateWhat.SetActive(true);
    }

    public void MoveSomething(GameObject what)
    {
        what.transform.position = where;
    }

    public void End()
    {
        Application.Quit();
    }
}
