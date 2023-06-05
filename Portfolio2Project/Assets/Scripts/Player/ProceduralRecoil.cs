using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProceduralRecoil : MonoBehaviour
{
    Vector3 currentRotation, 
            targetRotation, 
            targetPosition, 
            currentPosition, 
            initialPosition;

    public Transform cam;

    [SerializeField] float recoilX;
    [SerializeField] float recoilY;
    [SerializeField] float recoilZ;
    [SerializeField] float kickbackZ;

    public float snappiness, returnAmount;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    
    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * returnAmount);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, Time.fixedDeltaTime * snappiness);
        cam.localRotation = Quaternion.Euler(currentRotation);
        Back();
    }

    public void Recoil()
    {
        targetPosition -= new Vector3(0, 0, kickbackZ);
        targetRotation += new Vector3(recoilX, Random.Range(recoilY, recoilY), Random.Range(recoilX, recoilX));
    }

    void Back()
    {
        targetPosition = Vector3.Lerp(targetPosition, initialPosition, Time.deltaTime * returnAmount);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.fixedDeltaTime * snappiness);
        transform.localPosition = currentPosition;
    }
}
