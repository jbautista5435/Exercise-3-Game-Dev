using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TC edited the moveSpeed to 50 and swappwed the multiplication of the lookspeed variables -- [mohammedajao]
// TC also deleted the main camera to allow first person to work in the Unity object explorer -- [mohammedajao]

public class PlayerController : MonoBehaviour
{
    int moveSpeed = 25; // how fast the player moves
    float lookSpeedX = 6; // left/right mouse sensitivity
    float lookSpeedY = 3; // up/down mouse sensitivity
    int jumpForce = 250; // ammount of force applied to create a jump

    public Transform camTrans; // a reference to the camera transform
    float xRotation;
    float yRotation;
    Rigidbody _rigidbody;

    //The physics layers you want the player to be able to jump off of. Just dont include the layer the palyer is on.
    public LayerMask groundLayer;

    public Transform feetTrans; //Position of where the players feet touch the ground
    float groundCheckDist = .5f; //How far down to check for the ground. The radius of Physics.CheckSphere
    public bool grounded = false; //Is the player on the ground

    void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        lookSpeedX *= .65f; //WebGL has a bug where the mouse has higher sensitibity. This compensates for the change. 
        lookSpeedY *= .65f; //.65 is a rough guess based on testing in firefox.
#endif
        _rigidbody = GetComponent<Rigidbody>(); // Using GetComponent is expensive. Always do it in start and chache it when you can.
        Cursor.lockState = CursorLockMode.Locked; // Hides the mouse and locks it to the center of the screen.
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        //Creates a movement vector local to the direction the player is facing.
        Vector3 moveDir = transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxis("Horizontal"); // Use GetAxisRaw for snappier but non-analogue  movement
        moveDir *= moveSpeed;
        moveDir.y = _rigidbody.velocity.y; // We dont want y so we replace y with that the _rigidbody.velocity.y already is.
        _rigidbody.velocity = moveDir; // Set the velocity to our movement vector

        //The sphere check draws a sphere like a ray cast and returns true if any collider is withing its radius.
        //grounded is set to true if a sphere at feetTrans.position with a radius of groundCheckDist detects any objects on groundLayer within it
        grounded = Physics.CheckSphere(feetTrans.position, groundCheckDist, groundLayer);
    }

    void Update()
    {
        // Seems the movement isn't completely 1:1 with the mouse.[@mohammedajao]
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeedY;
        float mouseX = Input.GetAxis("Mouse X") * lookSpeedX;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90, 90); //Keeps up/down head rotation realistic
        camTrans.localEulerAngles = new Vector3(xRotation, yRotation, 0);
        transform.eulerAngles = new Vector3(0, yRotation, 0);

        if (grounded && Input.GetButtonDown("Jump")) //if the player is on the ground and press Spacebar
        {
            _rigidbody.AddForce(new Vector3(0, jumpForce, 0)); // Add a force jumpForce in the Y direction
        }
    }
}