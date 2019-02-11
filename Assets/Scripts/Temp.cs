using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField]
    float speed;

    [SerializeField]
    Transform targetpos;
    Vector3 CurrentPos;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;
        CurrentPos = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector3.forward * speed;

    }
}
