using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviours : MonoBehaviour
{
    Rigidbody rb;

    public Rigidbody target;    

    Vector3 steeringForce = Vector3.zero;

    [Range(0, 20)]
    public float speed = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {        
        Seek();
        rb.AddForce(steeringForce);
    }

    void Seek()
    {
        rb.AddForce(SteeringLab.Seek(target.position, rb, speed));
    }

    void Flee()
    {
        Vector3 a = -rb.velocity;
        Vector3 b = -SteeringLab.Seek(target.position, rb, speed);
        steeringForce = Vector3.Lerp(a, b, 0.1f);
    }

    void Pursue()
    {
        steeringForce = SteeringLab.Seek(target.position + target.velocity, rb, speed);
    }

    void Evade()
    {
        Vector3 a = -rb.velocity;
        Vector3 b = -SteeringLab.Seek(target.position + target.velocity, rb, speed);
        steeringForce = Vector3.Lerp(a, b, 0.1f);
    }

    void Arrive()
    {
        Vector3 a = SteeringLab.Seek(target.position, rb, speed);
        Vector3 b = SteeringLab.Arrive(target.position, rb);
        steeringForce = Vector3.Lerp(a, b, 0.1f);
    }
}
