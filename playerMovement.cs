using System.Collections;
using UnityEngine;

//I didnt knew about the character movement component while writing this script.
//Character movement component makes movement much easier, much smoother and flexible

//This script is attached to the player, responsible to its various motions and uses transform for normal movement 
//And uses Rigidbody for Jumping/Dashing

//Also this script is responsible for horizontal player rotation and camera rotations.
//Also responsible for the zoom in features while scoped in

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float sprintSpeed = 10f;
    public float sprintTimer = 3f;
    public float unZoomLookSpeedX = 60f;
    public float unZoomLookSpeedY = 60f;

    private float lookSpeedX;
    private float lookSpeedY;

    private Rigidbody rb;
    private playerStats ps;
    private scopeZoom sz;
    private bool isGrounded = true;
    private bool sprintTrigger = true;
    private float verticalInput;
    private float horizontalInput;
    private float Xrotation;
    private float mouseX;
    private float mouseY;
    private Vector3 moveDirection;

    public Transform cameraHolder;
    public Vector3 offset;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<playerStats>();
        sz = GetComponent<scopeZoom>();

        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;

        //by default normal sensitivity
        lookSpeedX = unZoomLookSpeedX;
        lookSpeedY = unZoomLookSpeedY;
    }

    void Update()
    {
        MouseSensitivity();//switches mouse sensitivity when zoomed in or out
        HandleInput();//takes player inputs for motion, Y axis rotation in player and camera and moves and rotates both
        LookAround();//take X axis rotation(up and down looking) and rotates only camera
    }

    void FixedUpdate()
    {
        //fixed update is more prefarable for physic actions in rigidbody
        HandleActions();// handels jumping and sprinting
    }

    void HandleInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        moveDirection = new Vector3(horizontalInput, 0f, verticalInput) * Time.deltaTime * moveSpeed;
        transform.Translate(moveDirection);

        mouseX = Input.GetAxisRaw("Mouse X") * lookSpeedX * Time.deltaTime * 2;
        transform.Rotate(Vector3.up * mouseX);

        cameraHolder.position = transform.position + offset;
    }

    void HandleActions()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && ps.currentStamina >= 10 && sprintTrigger == true)
        {
            Dash();
        }
    }
    private IEnumerator SprintCooldown()
    {
        //Sprint cooldown to prevent spam sprinting
        sprintTrigger = false;
        yield return new WaitForSeconds(sprintTimer);
        sprintTrigger = true;
    }

    void Jump()
    {
        //Jump logic
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void Dash()
    {
        //Dash logic
        rb.AddForce(transform.forward * sprintSpeed, ForceMode.Impulse);
        ps.currentStamina -= 10;
        ps.staminaText.text = "" + ps.currentStamina;
        StartCoroutine(SprintCooldown());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //detects ground collision
            isGrounded = true;
        }
    }

    void LookAround()
    {
        mouseY = Input.GetAxisRaw("Mouse Y") * lookSpeedY * Time.deltaTime * 2;
        Xrotation -= mouseY;
        Xrotation = Mathf.Clamp(Xrotation, -90f, 90f);//make sure you dont look up and down infinitely. locks sensitivity at 90 degrees in both up and down

        cameraHolder.localRotation = Quaternion.Euler(Xrotation, 0f, 0f);
        cameraHolder.rotation = Quaternion.Euler(cameraHolder.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f);
    }
    private void MouseSensitivity()
    {
        if (sz.isZoomed)
        {
            //less sensitivity when zoomed in
            lookSpeedX = sz.lookSensitivityX;
            lookSpeedY = sz.lookSensitivityY;
        }
        else
        {
            //returns to normal sensitivity
            lookSpeedX = unZoomLookSpeedX;
            lookSpeedY = unZoomLookSpeedY;
        }
    }
}
