using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeBehaviour : MonoBehaviour
{
    public Transform target;
    public float speed;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
}
