using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloor : MonoBehaviour
{
    [Header("---Components-----")]
    [SerializeField] Transform dest;

    [Header("-----Settings------")]
    [SerializeField, Range(0f, 30f)] float speed;
    [SerializeField, Range(0f,0.5f)] float reverseBuffer;
    [SerializeField] bool wireFrame;

    bool forward = true;
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (forward)
        {
            Vector3 newPos = Vector3.Lerp(startPos, dest.position, speed * Time.deltaTime);
        }
        else
        {
            Vector3 newPos = Vector3.Lerp(startPos, dest.position, speed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, dest.position) < reverseBuffer)
        {
            forward = !forward;
        }

    }

    private void OnDrawGizmos()
    {
        if (wireFrame)
        {
            Gizmos.DrawWireCube(dest.position, transform.localScale);
        }
        else
        {
            Gizmos.DrawCube(dest.position, transform.localScale);
        }
        Gizmos.DrawLine(dest.position, transform.localPosition);
    }
}
