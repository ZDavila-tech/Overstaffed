using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WaterEffect : MonoBehaviour
{
    [Header("----- Projectile Stats -----")]
    [SerializeField] float bulletLife;
    [SerializeField] float bulletRange;
    [SerializeField] int bulletSpeed;

    [Header("----- Components -----")]
    GameObject player;

    Vector3 playerPosition;
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, bulletLife);
        player = GameObject.FindGameObjectWithTag("Player");
        
    }
    private void Update()
    {
        playerPosition = player.transform.position;
        transform.RotateAround(GameManager.instance.playerCharacter.transform.position, Vector3.up * bulletRange, bulletSpeed * Time.deltaTime);
    }
}
