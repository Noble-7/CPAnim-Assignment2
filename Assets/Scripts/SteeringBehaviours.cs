using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviours : MonoBehaviour
{
    public List<GameObject> targets = new List<GameObject>();

    public Rigidbody target;

    float time = 0f;
    public float changeTime = 3f;
    int index = 0;

    Rigidbody rb;

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
        //float t = 1.0f - SteeringLab.Attenuate(target.transform.position, rb.position, proxim);

        time += Time.deltaTime;

        if (time > changeTime)
        {
            index = Random.Range(0, targets.Count);
            time = 0f;
        }

        //transform.position = Vector3.MoveTowards(transform.position, targets[index].transform.position, speed * Time.deltaTime);

        //transform.LookAt(targets[index].transform.position);

        Arrive();
        rb.AddForce(steeringForce);
    }

    private void OnTriggerEnter(Collider other)
    {
        transform.position = -Vector3.MoveTowards(transform.position, targets[index].transform.position, speed * Time.deltaTime);
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
