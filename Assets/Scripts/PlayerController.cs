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

    private enum PlayerState { Inactivo, Moviendo, Saltando, Cayendo, Muerto } // Estados del jugador
    private PlayerState currentState; // Estado actual del jugador

    // Se llama antes de la primera actualización del marco
    void Start()
    {
        winTextObject.SetActive(false);
        SetCountText();
        count = 0;
        rb = GetComponent<Rigidbody>();
        isGrounded = false;
        currentState = PlayerState.Inactivo; // Estado inicial del jugador es inactivo
    }

    // Se llama cuando el jugador presiona una tecla
    void OnMove(InputValue movementValue)
    {
        if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.downArrowKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed || Keyboard.current.rightArrowKey.isPressed) // Si el jugador presiona una tecla de movimiento se actualiza el estado a moviendo
        {
            Vector2 movementVector = movementValue.Get<Vector2>(); // Obtiene el vector de movimiento
            movementX = movementVector.x;
            movementY = movementVector.y;
            currentState = PlayerState.Moviendo; // Actualiza el estado del jugador a moviendo
        }
        else // Si el jugador no presiona ninguna tecla de movimiento se actualiza el estado a inactivo
        {
            movementX = 0;
            movementY = 0;
            currentState = PlayerState.Inactivo; // Actualiza el estado del jugador a inactivo
        }
    }

    // Se llama cuando el jugador presiona la tecla de salto
    void Update()
    {
        if (isGrounded && Keyboard.current.spaceKey.wasPressedThisFrame) // Si el jugador está en el suelo y presiona la tecla de espacio
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // El jugador salta
            isGrounded = false;
            currentState = PlayerState.Saltando; // Actualiza el estado del jugador a saltando
        }
    }

    // Se llama cuando el jugador colisiona con un objeto
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // Si el jugador colisiona con el suelo con la etiqueta "Ground"
        {
            isGrounded = true; // Se cambia isGrounded a verdadero
            if (currentState == PlayerState.Cayendo) // Si el estado actual del jugador es cayendo
            {
                currentState = PlayerState.Inactivo; // Se cambia el estado a inactivo
            }
        }
    }

    // Se llama cuando el jugador deja de colisionar con un objeto
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // Si el jugador deja de colisionar con el suelo con la etiqueta "Ground"
        {
            isGrounded = false;
            currentState = PlayerState.Cayendo; // Se cambia el estado a cayendo
        }
    }

    // Se llama cuando el jugador colisiona con un objeto
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) // Si el jugador colisiona con un enemigo
        {
            Debug.Log("El jugador ha tocado un enemigo.");
            currentState = PlayerState.Muerto; // Se cambia el estado a muerto
            Destroy(gameObject); // Se destruye el jugador
            winTextObject.SetActive(true); // Se muestra el mensaje de perder
            winTextObject.GetComponent<TextMeshProUGUI>().text = "Perdiste!"; // Se muestra el mensaje de perder
        }
        else if (other.gameObject.CompareTag("PickUp")) // Si el jugador colisiona con un PickUp
        {
            Debug.Log("Se ha recogido un PickUp."); 
            other.gameObject.SetActive(false); // Se desactiva el PickUp
            count++; // Se incrementa la puntuación
            SetCountText();
        }
    }

    // Se llama para actualizar la puntuación
    void SetCountText()
    {
        countText.text = "Puntuación: " + count.ToString(); // Se actualiza la puntuación
        if (count >= 15) // Si la puntuación es mayor o igual a 15
        {
            winTextObject.SetActive(true); // Se muestra el mensaje de ganar
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Se obtienen todos los enemigos
            foreach (GameObject enemy in enemies) // Se recorren todos los enemigos
            {
                Destroy(enemy); // Se destruyen los enemigos cuando se gana
            }
        }
    }

    // Se llama para actualizar la física
    void FixedUpdate()
    {
        if (currentState == PlayerState.Muerto) // Si el estado actual del jugador es muerto
            return; // Se sale de la función

        Vector3 forward = cameraTransform.forward; // Obtiene la dirección hacia adelante de la cámara
        Vector3 right = cameraTransform.right; // Obtiene la dirección hacia la derecha de la cámara

        forward.y = 0f; // 
        right.y = 0f;

        forward.Normalize(); // Normaliza la dirección hacia adelante
        right.Normalize(); // Normaliza la dirección hacia la derecha

        Vector3 movement = forward * movementY + right * movementX; // Calcula el vector de movimiento
        rb.AddForce(movement * speed); // Aplica la fuerza de movimiento al jugador
    }
}
