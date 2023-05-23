using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField] int sensHor;
    [SerializeField] int sensVert;

    [SerializeField] int lockVerMin;
    [SerializeField] int lockVerMax;


    float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Get Input
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVert;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHor;

        //Convert input to rotation float and give the option for inverted controls
        if (gameManager.instance.invert.isOn)
        {
            xRotation += mouseY;
        }
        else
        {
            xRotation -= mouseY;
        }
       

        //Clamp Camera rotation
        xRotation = Mathf.Clamp(xRotation, lockVerMin, lockVerMax);

        //Rotate the camera on the X axis
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //Rotate the player on the y axix
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
