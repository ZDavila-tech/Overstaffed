using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{

    public Staff staff;
    [SerializeField] int hitDmg;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && staff.isAttacking)
        {
            Debug.Log("Enemy Hit");
            IDamage damageable = other.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.TakeDamage(hitDmg);
                DamageCooldown();
            }
        }
    }

    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(.5f);
    }
}
