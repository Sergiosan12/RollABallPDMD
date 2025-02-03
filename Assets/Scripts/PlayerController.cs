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
    

    public float speed = 10f; // Velocidad de movimiento.
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public Transform cameraTransform; // Transform de la cámara para orientar el movimiento.

    void Start()
    {
        winTextObject.SetActive(false);
        SetCountText();
        count = 0;
        rb = GetComponent<Rigidbody>();
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
            // No se mueve si no se presionan las teclas de flecha
            movementX = 0;
            movementY = 0;
        }
    }

    void SetCountText() 
   {
       countText.text =  "Count: " + count.ToString();
       if (count >= 15)
       {
    // Display the win text.
            winTextObject.SetActive(true);

    // Destroy the enemy GameObject.
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));

       }
   }

    private void OnCollisionEnter(Collision collision)
    {
     if (collision.gameObject.CompareTag("Enemy"))
        {
            // Destroy the current object
            Destroy(gameObject); 
    
            // Update the winText to display "You Lose!"
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
    
        }

    }

    void FixedUpdate()
    {
        // Movimiento relativo a la cámara
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Ignorar la inclinación vertical de la cámara
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 movement = forward * movementY + right * movementX;
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter (Collider other) 
    {
       if (other.gameObject.CompareTag("PickUp")) 
       {
           other.gameObject.SetActive(false);
           count = count + 1;
           SetCountText();
       }
    }
}
