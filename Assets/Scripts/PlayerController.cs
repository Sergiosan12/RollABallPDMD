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

    public float speed = 10f;
    public float jumpForce = 5f;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public Transform cameraTransform;

    private enum PlayerState { Inactivo, Moviendo, Saltando, Cayendo, Muerto }
    private PlayerState currentState;

    void Start()
    {
        winTextObject.SetActive(false);
        SetCountText();
        count = 0;
        rb = GetComponent<Rigidbody>();
        isGrounded = false;
        currentState = PlayerState.Inactivo;
    }

    void OnMove(InputValue movementValue)
    {
        if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.downArrowKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            Vector2 movementVector = movementValue.Get<Vector2>();
            movementX = movementVector.x;
            movementY = movementVector.y;
            currentState = PlayerState.Moviendo;
        }
        else
        {
            movementX = 0;
            movementY = 0;
            currentState = PlayerState.Inactivo;
        }
    }

    void Update()
    {
        if (isGrounded && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            currentState = PlayerState.Saltando;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (currentState == PlayerState.Cayendo)
            {
                currentState = PlayerState.Inactivo;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            currentState = PlayerState.Cayendo;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("El jugador ha tocado un enemigo.");
            currentState = PlayerState.Muerto;
            Destroy(gameObject);
            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "Perdiste!";
        }
        else if (other.gameObject.CompareTag("PickUp"))
        {
            Debug.Log("Se ha recogido un PickUp.");
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
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

    void FixedUpdate()
    {
        if (currentState == PlayerState.Muerto)
            return;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 movement = forward * movementY + right * movementX;
        rb.AddForce(movement * speed);
    }
}
