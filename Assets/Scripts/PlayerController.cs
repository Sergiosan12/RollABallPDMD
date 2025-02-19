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

    private enum PlayerState { Idle, Walking, Jumping, Falling, Dead } // Estados del jugador
    private PlayerState currentState; // Estado actual del jugador

    // Se llama antes de la primera actualización del marco
    void Start()
    {
        winTextObject.SetActive(false);
        SetCountText();
        count = 0;
        rb = GetComponent<Rigidbody>();
        isGrounded = false;
        currentState = PlayerState.Idle; // Estado inicial del jugador es Idle
        animator = GetComponent<Animator>(); // Inicializo el animator

    }

    // Se llama cuando el jugador presiona una tecla
    void OnMove(InputValue movementValue)
    {
        if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.downArrowKey.isPressed ||
        Keyboard.current.leftArrowKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
        currentState = PlayerState.Walking;
        UpdateAnimator();
    }
    else
    {
        movementX = 0;
        movementY = 0;
        currentState = PlayerState.Idle;
        UpdateAnimator();
    }
    }

    // Se llama cuando el jugador presiona la tecla de salto
    void Update()
        {
        if (isGrounded && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // El jugador salta
            isGrounded = false;
            currentState = PlayerState.Jumping; // Actualiza el estado del jugador a Jumping
            UpdateAnimator();
        }

    }

    // Se llama cuando el jugador colisiona con un objeto
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // Si el jugador colisiona con el suelo con la etiqueta "Ground"
        {
            isGrounded = true; // Se cambia isGrounded a verdadero
            if (currentState == PlayerState.Falling) // Si el estado actual del jugador es Falling
            {
                currentState = PlayerState.Idle; // Se cambia el estado a Idle
                UpdateAnimator();
            }
        }
    }

    // Se llama cuando el jugador deja de colisionar con un objeto
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // Si el jugador deja de colisionar con el suelo con la etiqueta "Ground"
        {
            isGrounded = false;
            currentState = PlayerState.Falling; // Se cambia el estado a Falling
            UpdateAnimator();
        }
    }

    // Se llama cuando el jugador colisiona con un objeto
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) // Si el jugador colisiona con un enemigo
        {
            Debug.Log("El jugador ha tocado un enemigo.");
            currentState = PlayerState.Dead; // Se cambia el estado a Dead
            UpdateAnimator();
            winTextObject.SetActive(true); // Se muestra el mensaje de perder
            winTextObject.GetComponent<TextMeshProUGUI>().text = "Perdiste!"; 

            gameObject.SetActive(false); // Desactiva el jugador para simular que desaparece

            Invoke("RestartGame", 1.5f); // Llama a la función para reiniciar el juego después de 3 segundos
        }
        else if (other.gameObject.CompareTag("PickUp")) // Si el jugador colisiona con un PickUp
        {
            Debug.Log("Se ha recogido un PickUp.");
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    // Función para reiniciar el juego
    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
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
        if (currentState == PlayerState.Dead) // Si el estado actual del jugador es Dead
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

    // Se llama para actualizar el animador
    void UpdateAnimator()
    {
        animator.SetBool("isWalking", currentState == PlayerState.Walking);
        animator.SetBool("isJumping", currentState == PlayerState.Jumping);
        animator.SetBool("isFalling", currentState == PlayerState.Falling);
        animator.SetBool("isDead", currentState == PlayerState.Dead);
    }

}
