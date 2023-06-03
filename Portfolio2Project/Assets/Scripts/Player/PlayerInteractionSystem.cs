using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionSystem : MonoBehaviour
{
    [SerializeField] private Transform interactScanCenter;
    [SerializeField] private float interactScanRadius;
    [SerializeField] private LayerMask interactableLayerMask;

    private readonly Collider[] interactables = new Collider[3];
    [SerializeField] private int numberOfInteractablesFound;

    void Update()
    {
        numberOfInteractablesFound = Physics.OverlapSphereNonAlloc(interactScanCenter.position, interactScanRadius, interactables, (int)interactableLayerMask);
    }

    
}
