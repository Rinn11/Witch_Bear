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

    private Quaternion originalCameraRotation;

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

    public float sprintSpeedMultiplier = 1.5f;

    private Vector3 originalScale;

    public Camera playerCamera;

    private bool reversedControls = false;
    private bool isFlipped = false;
    private bool hasStrengthBuff = false;
    private bool isShrunk = false;
    private string activeEffect = "None";

    private void Start()
    {
        originalMoveSpeed = moveSpeed;
        originalScale = transform.localScale;
        originalCameraRotation = playerCamera.transform.rotation;
    }

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
        rotation.x = Mathf.Clamp(rotation.x, -60 / rotate, 60 / rotate);
        playerHead.transform.eulerAngles = rotation * rotate;
    }

    void FixedUpdate()
    {
        Vector3 moveDirectionTrue = transform.forward * moveDirection.y + transform.right * moveDirection.x;
        playerMovement = moveDirectionTrue * currentSpeed;

        rb.velocity = playerMovement;
    }

    public void ApplySpeedMultiplier(float multiplier)
    {
        if (activeEffect != "Speed") ResetEffects();
        moveSpeed = originalMoveSpeed * multiplier;
        activeEffect = "Speed";
        Debug.Log("Speed changed to: " + moveSpeed);
    }

    public void ApplyJumpBoost(float multiplier)
    {
        if (activeEffect != "Jump") ResetEffects();
        jumpMultiplier = multiplier;
        activeEffect = "Jump";
        Debug.Log("Jump height changed to: " + (jumpForce * jumpMultiplier));
    }

    public void EnableStrengthBuff()
    {
        if (activeEffect != "Strength") ResetEffects();
        hasStrengthBuff = true;
        activeEffect = "Strength";
        Debug.Log("Strength Buff Activated!");
    }

    public void ShrinkPlayer()
    {
        if (activeEffect != "Shrink") ResetEffects();
        transform.localScale = originalScale * 0.5f;
        isShrunk = true;
        activeEffect = "Shrink";
        Debug.Log("Player Shrunk!");
    }

    public void ReverseControls()
    {
        if (activeEffect != "Reverse") ResetEffects();
        reversedControls = true;
        activeEffect = "Reverse";
        Debug.Log("Controls Reversed!");
    }

    public void FlipScreen()
    {
        if (activeEffect != "Flip") ResetEffects();
        playerCamera.transform.Rotate(0, 0, 180);
        isFlipped = true;
        activeEffect = "Flip";
        Debug.Log("Screen Flipped!");
    }

    public void TeleportPlayer()
    {
        if (activeEffect != "Teleport") ResetEffects();
        Vector3 randomOffset = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        transform.position += randomOffset;
        activeEffect = "Teleport";
        Debug.Log("Player Teleported!");
    }

    private void ResetEffects()
    {
        hasStrengthBuff = false;
        reversedControls = false;
        isFlipped = false;
        isShrunk = false;
        moveSpeed = originalMoveSpeed;
        transform.localScale = originalScale;
        playerCamera.transform.rotation = originalCameraRotation;
        jumpMultiplier = 1.0f;
        activeEffect = "None";
        Debug.Log("Effects Reset");
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

        move = playerControls.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        attack.Disable();
    }
}
