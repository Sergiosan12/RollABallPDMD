using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb; // Rigidbody del jugador.
    private int count;
    private float movementX;
    private float movementY;
    private bool isGrounded; // Verifica si la bola está en el suelo.

    public float speed = 10f; // Velocidad de movimiento.
    public float jumpForce = 5f; // Fuerza del salto.
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public Transform cameraTransform; // Transform de la cámara para orientar el movimiento.

    void Start()
    {
        winTextObject.SetActive(false);
        SetCountText();
        count = 0;
        rb = GetComponent<Rigidbody>();
        isGrounded = false; // Inicialmente está en el suelo.
    }

    void OnMove(InputValue movementValue)
    {
        // Detectar únicamente las teclas de flecha
        if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.downArrowKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            Vector2 movementVector = movementValue.Get<Vector2>();
            movementX = movementVector.x;
            movementY = movementVector.y;
        }
        else
        {
            movementX = 0;
            movementY = 0;
        }
    }

    void Update()
    {
        // Si está en el suelo y se presiona espacio, salta
        if (isGrounded && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Evita que salte en el aire.
        }
    }

    // Mejor detección del suelo
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Si la bola sigue en contacto con el suelo, puede saltar de nuevo.
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // Si la bola deja de tocar el suelo, no puede saltar hasta que vuelva a aterrizar.
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "Perdiste!";
        }
    }

    void SetCountText()
    {
        countText.text = "Puntuación: " + count.ToString();
        if (count >= 15)
        {
            winTextObject.SetActive(true);
            // Encuentra y destruye todos los enemigos
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
               Destroy(enemy);
            }
        }
    }

    void FixedUpdate()
    {
        // Movimiento relativo a la cámara
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 movement = forward * movementY + right * movementX;
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }
}
