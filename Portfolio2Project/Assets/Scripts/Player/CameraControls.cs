using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    UIManager uiManager;

    [SerializeField] int sensHor;
    [SerializeField] int sensVert;

    [SerializeField] int lockVerMin;
    [SerializeField] int lockVerMax;
    


    float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = UIManager.instance;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        //sensHor = (int)(uiManager.mouseSen.value * 300);
        //sensVert = (int)(uiManager.mouseSen.value * 300);
        //Get Input
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * (sensVert + (uiManager.mouseSen.value * 300));
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * (sensHor + (uiManager.mouseSen.value * 300));

        //Convert input to rotation float and give the option for inverted controls
        if (uiManager != null && uiManager.invert.isOn)
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
