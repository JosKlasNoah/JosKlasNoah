using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    private Vector3 startPos;
    [SerializeField] Vector3 endPos = new Vector3(0, 20, 0);
    [SerializeField] private float speed = 1;
    [SerializeField] private float waitTime = 5f;

    /*private bool startTimer = false;
    private float timer = 5;*/

    private void Start()
    {
        Debug.Log(transform.localPosition);
        startPos = transform.localPosition;
    }

    bool started = false;

    public void StartPlatform()
    {
        if (!started)
        {
            StartCoroutine(MovePlatform());
            started = true;
        }
    }


    IEnumerator MovePlatform()
    {
        while (true)
        {
            while (transform.localPosition.y < startPos.y + endPos.y)
            {
                transform.localPosition += Vector3.up * speed;

                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForSeconds(waitTime);

            while (transform.localPosition.y > startPos.y)
            {

                transform.localPosition -= Vector3.up * speed;
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForSeconds(waitTime);

        }
    }

}
