using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    public int up = -1;
    private float t = 0;
    private Vector3 startPos;
    [SerializeField] Vector3 endPos = new Vector3(0,20, 0);
    [SerializeField] private float speed = 1;

    /*private bool startTimer = false;
    private float timer = 5;*/

    private void Start()
    {
        startPos = transform.localPosition;  
    }

    private void MoveElevator()
    {
        //transform.localPosition = new Vector3(transform.position.x, Mathf.Lerp(46.25f, 1, 0), transform.position.z);
        transform.localPosition = Vector3.Lerp(startPos,startPos + endPos, t);
    }

    private void Update()
    {
      /*  if (t == 1)
        {
            timer -= Time.deltaTime;
        }

        if(timer <= 0)
        {
            up *= -1;
            timer = 5;
        }*/

        t += Time.deltaTime * up * speed;
        Mathf.Clamp(t, 0, 1);
       MoveElevator();
    }

}
