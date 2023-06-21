using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FollowPlayer : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        pos.x = player.transform.position.x;
        pos.y = player.transform.position.y - 1f;
        pos.z = player.transform.position.z;

        transform.position = pos;
    }
}
