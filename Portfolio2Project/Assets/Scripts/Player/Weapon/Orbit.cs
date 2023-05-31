using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    [SerializeField] GameObject orbitpoint;
    [SerializeField] float range;
    [SerializeField] float speed;

    enum orbitType
    {
        Horizontal,
        Diagonal,
        Vertical
    }

    [SerializeField] orbitType type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case orbitType.Horizontal:
        transform.RotateAround(orbitpoint.transform.position, Vector3.up * range, speed * Time.deltaTime);
                break;
            case orbitType.Vertical:
                transform.RotateAround(orbitpoint.transform.position, Vector3.forward * range, speed * Time.deltaTime);
                break;
            case orbitType.Diagonal:
                transform.RotateAround(orbitpoint.transform.position, Vector3.right * range, speed * Time.deltaTime);
                break;
        }
    }
}
