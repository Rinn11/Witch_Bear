using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using Cursor = UnityEngine.Cursor;
using Image = UnityEngine.UI.Image;

public class PlayerController : MonoBehaviour
{
    public Image effectUI;

    public float grvityMutiplyer;

    private float zRotate = 0;

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
        playerHead.transform.eulerAngles = new Vector3(rotation.x,rotation.y,zRotate) * rotate;
    }

    void FixedUpdate()
    {
        Vector3 moveDirectionTrue = transform.forward * moveDirection.y + transform.right * moveDirection.x;
        playerMovement = moveDirectionTrue * currentSpeed;

        //rb.velocity = playerMovement;
        rb.velocity = new Vector3(playerMovement.x, -grvityMutiplyer, playerMovement.z);
    }

    public void ApplySpeedMultiplier(float multiplier, Sprite effect)
    {
        if (activeEffect != "Speed") ResetEffects();
        effectUI.sprite = effect;

        moveSpeed = originalMoveSpeed * multiplier;
        activeEffect = "Speed";
        Debug.Log("Speed changed to: " + moveSpeed);
    }

    public void ApplyJumpBoost(float multiplier, Sprite effect)
    {
        if (activeEffect != "Jump") ResetEffects();

        effectUI.sprite = effect;
        jumpMultiplier = multiplier;
        activeEffect = "Jump";
        Debug.Log("Jump height changed to: " + (jumpForce * jumpMultiplier));
    }

    public void EnableStrengthBuff(Sprite effect)
    {
        if (activeEffect != "Strength") ResetEffects();
        effectUI.sprite = effect;
        hasStrengthBuff = true;
        activeEffect = "Strength";
        Debug.Log("Strength Buff Activated!");
    }

    public void ShrinkPlayer(Sprite effect)
    {
        if (activeEffect != "Shrink") ResetEffects();
        effectUI.sprite = effect;
        transform.localScale = originalScale * 0.5f;
        isShrunk = true;
        activeEffect = "Shrink";
        Debug.Log("Player Shrunk!");
    }

    public void ReverseControls(Sprite effect)
    {
        if (activeEffect != "Reverse") ResetEffects();
        reversedControls = true;
        effectUI.sprite = effect;
        activeEffect = "Reverse";
        Debug.Log("Controls Reversed!");
    }

    public void FlipScreen(Sprite effect)
    {
        if (activeEffect != "Flip") ResetEffects();
        zRotate = 180;
        effectUI.sprite = effect;
        isFlipped = true;
        activeEffect = "Flip";
        Debug.Log("Screen Flipped!");
    }

    public void TeleportPlayer(Sprite effect)
    {
        if (activeEffect != "Teleport") ResetEffects();
        Vector3 randomOffset = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
        transform.position += randomOffset;
        activeEffect = "Teleport";
        effectUI.sprite = effect;
        Debug.Log("Player Teleported!");
    }

    private void ResetEffects()
    {
       // effectUI = null;
        hasStrengthBuff = false;
        reversedControls = false;
        isFlipped = false;
        isShrunk = false;
        moveSpeed = originalMoveSpeed;
        transform.localScale = originalScale;
        zRotate = 0;
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
