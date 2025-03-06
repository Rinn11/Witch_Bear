using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using Cursor = UnityEngine.Cursor;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float sprintSpeedMutiplyer = 1.5f;
    public Camera playerHead;
    public float currentSpeed;
    public float accelSpeed;

    public float acceleration = 0.6f;
    public float decceleration = 0.6f;

    public float jumpForce = 3;
    private float jumpMultiplier = 1.0f;
    private float originalMoveSpeed;

    public PlayerControls playerControls;

    private InputAction move;
    private InputAction jump;
    private InputAction sprint;
    private InputAction attack;

    Vector2 moveDirection = Vector2.zero;
    float jumping;
    bool sprinting;

    bool ground;

    Vector3 playerMovement;

    public Rigidbody rb;

    Vector2 rotation;

    private float rotateSpeed = 3;
    private float rotate;

    public AudioSource audioSource;
    public AudioClip eatingSound;

    private void Awake()
    {
        playerControls = new PlayerControls();

        rb = GetComponent<Rigidbody>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rotate = rotateSpeed;
        currentSpeed = moveSpeed;
        originalMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        playerLook();
        moveDirection = move.ReadValue<Vector2>();
    }

    public void playerLook()
    {
        rotation.y += Input.GetAxis("Mouse X");
        transform.eulerAngles = new Vector2(0, rotation.y * rotate);

        rotation.x += -Input.GetAxis("Mouse Y");
        rotation.x = Mathf.Clamp(rotation.x, -60 / rotate, 60 / rotate); // Limit vertical look angle
        playerHead.transform.eulerAngles = rotation * rotate;
    }

    // Physics based movement
    void FixedUpdate()
    {
        // Translate input to world-space movement based on player's forward and right vectors
        Vector3 moveDirectionTrue = transform.forward * moveDirection.y + transform.right * moveDirection.x;
        playerMovement = moveDirectionTrue * currentSpeed + new Vector3(0, grounded() ? rb.velocity.y + (jumping * jumpForce * jumpMultiplier) : rb.velocity.y, 0); // Preserve vertical velocity (e.g., gravity)

        rb.velocity = playerMovement;
    }

    public void fire(InputAction.CallbackContext context)
    {

    }

    private void runFast(InputAction.CallbackContext context)
    {
        accelSpeed = context.performed ? moveSpeed * sprintSpeedMutiplyer : moveSpeed;
    }

    private bool grounded()
    {
        return rb.velocity.y == 0;
    }

    public void ApplySpeedMultiplier(float multiplier)
    {
        moveSpeed = originalMoveSpeed * multiplier;
        Debug.Log("Speed changed to: " + moveSpeed);
    }

    public void ApplyJumpBoost(float multiplier)
    {
        jumpMultiplier = multiplier;
        Debug.Log("Jump height changed to: " + (jumpForce * jumpMultiplier));
    }

    public void StartEating()
    {
        if (audioSource != null && eatingSound != null)
        {
            audioSource.PlayOneShot(eatingSound);
        }
    }

    private void OnEnable()
    {
        attack = playerControls.Player.Fire;
        attack.Enable();
        attack.performed += fire;
        attack.canceled += fire;

        move = playerControls.Player.Move;
        move.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
        attack.Disable();
    }
}
