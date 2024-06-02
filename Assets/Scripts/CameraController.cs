using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;

    public float normalSpeed;
    public float fastSpeed;
    public float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;

    public Quaternion newRotation;
    public Vector3 newPosition;
    public Vector3 newZoom;

    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
    }

    void HandleMovementInput()
    {
        // Movement
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) 
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition -= (transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition -= (transform.right * movementSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movementSpeed);
        }

        // Rotation
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(0, rotationAmount, 0);
        }
        if (Input.GetKey(KeyCode.R))
        {
            newRotation *= Quaternion.Euler(0, -rotationAmount, 0);
        }

        // Zoom
        if (Input.GetKey(KeyCode.Z))
        {
            newZoom += zoomAmount;
        }
        if (Input.GetKey(KeyCode.X))
        {
            newZoom -= zoomAmount;
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, movementTime * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, movementTime * Time.deltaTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, movementTime * Time.deltaTime);
    }
}
