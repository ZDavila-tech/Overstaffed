using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStick : MonoBehaviour
{


    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TOUCHING");
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerCharacter.transform.SetParent(transform, false);
            gameManager.instance.playerCharacter.transform.localScale = new Vector3(
gameManager.instance.playerCharacter.transform.localScale.x / gameObject.transform.localScale.x,
gameManager.instance.playerCharacter.transform.localScale.y / gameObject.transform.localScale.y,
gameManager.instance.playerCharacter.transform.localScale.z / gameObject.transform.localScale.z);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerCharacter.transform.SetParent(null, true);
        }
    }

    

}
