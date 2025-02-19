using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private bool isGrounded;
    private Animator animator;

    public float speed = 10f;
    public float jumpForce = 5f;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public Transform cameraTransform;

    private enum PlayerState { Idle, Walking, Jumping, Falling, Dead }
    private PlayerState currentState;

    void Start()
    {
        winTextObject.SetActive(false);
        SetCountText();
        count = 0;
        rb = GetComponent<Rigidbody>();
        isGrounded = false;
        currentState = PlayerState.Idle;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isGrounded && (Keyboard.current.spaceKey.wasPressedThisFrame || Touchscreen.current.primaryTouch.press.isPressed))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            currentState = PlayerState.Jumping;
            UpdateAnimator();
        }
    }

    void FixedUpdate()
    {
        if (currentState == PlayerState.Dead)
            return;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector2 touchDelta = Vector2.zero;
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            touchDelta = Touchscreen.current.primaryTouch.delta.ReadValue();
        }

        movementX = touchDelta.x / Screen.width;
        movementY = touchDelta.y / Screen.height;

        Vector3 movement = forward * movementY + right * movementX;
        rb.AddForce(movement * speed);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (currentState == PlayerState.Falling)
            {
                currentState = PlayerState.Idle;
                UpdateAnimator();
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            currentState = PlayerState.Falling;
            UpdateAnimator();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("El jugador ha tocado un enemigo.");
            currentState = PlayerState.Dead;
            UpdateAnimator();
            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "Perdiste!";

            gameObject.SetActive(false);
            Invoke("RestartGame", 1.5f);
        }
        else if (other.gameObject.CompareTag("PickUp"))
        {
            Debug.Log("Se ha recogido un PickUp.");
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void SetCountText()
    {
        countText.text = "Puntuación: " + count.ToString();
        if (count >= 15)
        {
            winTextObject.SetActive(true);
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
        }
    }

    void UpdateAnimator()
    {
        animator.SetBool("isWalking", currentState == PlayerState.Walking);
        animator.SetBool("isJumping", currentState == PlayerState.Jumping);
        animator.SetBool("isFalling", currentState == PlayerState.Falling);
        animator.SetBool("isDead", currentState == PlayerState.Dead);
    }
}
