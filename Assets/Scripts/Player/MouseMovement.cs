using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSens;
    public Transform playerBody;
    private float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // clamp to prevent player "flipping"

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // rotate up/down
        playerBody.Rotate(Vector3.up * mouseX); // rotate left/right
        
    }

    void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
