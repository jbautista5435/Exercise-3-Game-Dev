using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// TC edited the moveSpeed to 50 and swappwed the multiplication of the lookspeed variables -- [mohammedajao]
// TC also deleted the main camera to allow first person to work in the Unity object explorer -- [mohammedajao]

public class PlayerController : MonoBehaviour
{
    public float health = 100;
    public Image healthBarImage;
    public GameObject mainMenu;
    public GameObject levelLoader;

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

    private const float maxHealth = 100;
    private bool mainMenuActive = false;
    private string username;

    void Start()
    {
        mainMenu.SetActive(false);
#if UNITY_WEBGL && !UNITY_EDITOR
        lookSpeedX *= .65f; //WebGL has a bug where the mouse has higher sensitibity. This compensates for the change. 
        lookSpeedY *= .65f; //.65 is a rough guess based on testing in firefox.
#endif
        _rigidbody = GetComponent<Rigidbody>(); // Using GetComponent is expensive. Always do it in start and chache it when you can.
        if (!mainMenuActive) 
        {
            Cursor.lockState = CursorLockMode.Locked; // Hides the mouse and locks it to the center of the screen.
            Cursor.visible = false;
        }
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
        if (health <= 0) {
            print("loading lose");
            levelLoader.GetComponent<LevelLoader>().LoadLoseScreen();
        }
        UpdateHealthBar();
        // Seems the movement isn't completely 1:1 with the mouse.[@mohammedajao]
        float mouseY = Input.GetAxis("Mouse X") * lookSpeedY;
        float mouseX = Input.GetAxis("Mouse Y") * lookSpeedX;

        yRotation -= mouseX;
        xRotation += mouseY;

        yRotation = Mathf.Clamp(yRotation, -90, 90); //Keeps up/down head rotation realistic
        camTrans.localEulerAngles = new Vector3(yRotation,0 , 0);
        transform.eulerAngles = new Vector3(0, xRotation, 0);

        if (grounded && Input.GetButtonDown("Jump")) //if the player is on the ground and press Spacebar
        {
            _rigidbody.AddForce(new Vector3(0, jumpForce, 0)); // Add a force jumpForce in the Y direction
            grounded = false;
        }

        if (Input.GetButtonDown("escape")) 
        {
            mainMenuActive = !mainMenuActive;
            mainMenu.SetActive(mainMenuActive);
            if (mainMenuActive) {
                Cursor.lockState = CursorLockMode.None; // Hides the mouse and locks it to the center of the screen.
                Cursor.visible = true;
            }
        }

        if (transform.position.y < -10.0f) {
            levelLoader.GetComponent<LevelLoader>().ResetLevel();
        }
    }

    public void UpdateHealthBar() {
        healthBarImage.GetComponent<Image>().fillAmount = Mathf.Clamp(health/maxHealth, 0, 1f);
    }

    public string getUsername() { 
        return username;
    }

    public void setUsername(string username) {
        this.username = username;
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        UpdateHealthBar();
        print(health + " health");
    }
}