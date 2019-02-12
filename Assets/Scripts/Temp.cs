using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Temp : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField]
    float speed = 0;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = Vector3.up * speed;

    }
}
