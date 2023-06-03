using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionSystem : MonoBehaviour
{
    [SerializeField] private Transform interactScanCenter;
    [SerializeField] private float interactScanRadius;
    [SerializeField] private LayerMask interactableLayerMask;

    private readonly Collider[] interactables = new Collider[1];
    [SerializeField] private int numberOfInteractablesFound;

    UIManager uiManager;

    private IInteract interactable;

    private void Start()
    {
        uiManager = UIManager.instance;
    }

    void Update()
    {
        numberOfInteractablesFound = Physics.OverlapSphereNonAlloc(interactScanCenter.position, interactScanRadius, interactables, (int)interactableLayerMask);
        if (numberOfInteractablesFound > 0)
        {
            interactable = interactables[0].GetComponent<IInteract>();
            if (interactable != null )
            {
                string interactionText = "[Z] " + interactable.InteractionPrompt;
                uiManager.UpdateInteractText(1, interactionText);

                if (Input.GetButtonUp("Interact"))
                {
                    interactable.Interact(this);
                }
            }
        }
        else
        {
            if(interactable != null)
            {
                interactable = null;
            }
            uiManager.UpdateInteractText(0);
        }
    }    
}
