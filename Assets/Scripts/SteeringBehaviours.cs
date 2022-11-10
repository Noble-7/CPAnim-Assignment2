using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviours : MonoBehaviour
{
    public List<GameObject> targets = new List<GameObject>();

    float time = 0f;
    public float changeTime = 3f;
    int index = 0;

    [Range(0, 5)]
    public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time > changeTime)
        {
            index = Random.Range(0, targets.Count);
            time = 0f;
        }

        transform.position = Vector3.MoveTowards(transform.position, targets[index].transform.position, speed * Time.deltaTime);

        transform.LookAt(targets[index].transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        transform.position = -Vector3.MoveTowards(transform.position, targets[index].transform.position, speed * Time.deltaTime);
    }
}
