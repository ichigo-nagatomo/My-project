using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    
    private float timeTaken = 0.2f;

    private float timeErapsed;

    private Vector3 destination;

    private Vector3 origin;

    public void MoveTo(Vector3 newDestination) {
        timeErapsed = 0;

        origin = destination;
        transform.position = origin;

        destination = newDestination;
    }

    // Start is called before the first frame update
    void Start()
    {
        destination = transform.position;
        origin = destination;
    }

    // Update is called once per frame
    void Update()
    {
        if (origin == destination) {
            return;
        }

        timeErapsed += Time.deltaTime;

        float timeRate = timeErapsed / timeTaken;

        if (timeRate > 1) {
            timeRate = 1;
        }

        float easing = timeRate;

        Vector3 currentPosition = Vector3.Lerp(origin, destination, easing);

        transform.position = currentPosition;
    }
}
