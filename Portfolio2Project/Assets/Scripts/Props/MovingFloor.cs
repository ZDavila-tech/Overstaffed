using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloor : MonoBehaviour
{
    public float moveSpeed;

    // Var Initialization
    Transform thingToMove;

    Transform destination;

    Vector3 startPoint;
    Vector3 startSize;

    bool move = true;
    // Start is called before the first frame update
    void Start()
    {
        thingToMove = transform.GetChild(0);
        destination = transform.GetChild(1);

        startPoint = thingToMove.position;
        startSize = thingToMove.localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (move)
        {
            thingToMove.transform.position = Vector3.MoveTowards(thingToMove.position, destination.position, moveSpeed * Time.deltaTime);

        }
        else
        {
            thingToMove.transform.position = Vector3.MoveTowards(thingToMove.position, startPoint, moveSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(thingToMove.position,destination.position) < 0.01)
        {
            move = false;
        }
        else if (Vector3.Distance(thingToMove.position, startPoint) < 0.01)
        {
            move = true;
        }
    }
}
