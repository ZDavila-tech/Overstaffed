using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    [SerializeField] GameObject orbitpoint;
    [SerializeField] float range;
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(orbitpoint.transform.position, Vector3.up * range, speed * Time.deltaTime);
    }
}
