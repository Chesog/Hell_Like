using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    [Header("Mouse Position")]
    [SerializeField] private float mouseX;
    [SerializeField] private float mouseY;
    [Header("Mouse Setup")]
    [SerializeField] private float mouseSensitivity = 100.0f;
    [SerializeField] private float xRotation = 0.0f;
    [Header("Player Reference")]
    [SerializeField] private Transform character;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        character ??= GetComponent<Transform>();
        if (!character)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(character)} is null");
        }
    }

    // Update is called once per frame
    void Update()
    {

        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

        transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

        character.Rotate(Vector3.up * mouseX);
    }
}
