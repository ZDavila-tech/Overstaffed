using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class playerStick : MonoBehaviour
{
    [SerializeField] Transform anchorPoint;
    ConstraintSource constraintSource;
    ParentConstraint constraint;
    bool touching;
    Vector3 originalScale;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TOUCHING");
        if (other.CompareTag("Player"))
        {
            originalScale = gameManager.instance.playerCharacter.transform.localScale;
            gameManager.instance.playerCharacter.transform.SetParent(anchorPoint, false);
            //gameManager.instance.playerCharacter.transform.localScale= originalScale;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerCharacter.transform.SetParent(null,false);
            gameManager.instance.playerCharacter.transform.localScale=originalScale;
        }
    }

    

}
