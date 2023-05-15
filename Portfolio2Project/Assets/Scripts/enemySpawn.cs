using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawn : MonoBehaviour
{

    [SerializeField] float spawnAreaX;
    [SerializeField] float spawnAreaY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
            Gizmos.DrawWireCube(new Vector3(transform.position.x,transform.position.y + 1,transform.position.z), new Vector3(spawnAreaX, 2, spawnAreaY));
    }
}
