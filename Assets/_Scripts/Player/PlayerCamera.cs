using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public float sensitivityX;
    public float sensitivityY;
    public Transform orientation;

    float xRotation;
    float yRotation;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY * Time.fixedDeltaTime;

        yRotation += mouseX; // Rotate around the y-axis (left/right)
        xRotation -= mouseY; // Rotate around the x-axis (up/down)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp the xRotation to prevent flipping

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0); // Apply the rotation to the camera
        orientation.rotation = Quaternion.Euler(0, yRotation, 0); // Apply the yRotation to the orientation object (if needed)
    }
}
