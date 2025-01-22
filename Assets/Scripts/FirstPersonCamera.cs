using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{

    public Transform player; // Referencia al jugador (la bola).
    public float rotationSpeed = 100f; // Velocidad de rotación de la cámara.
    public float heightOffset = 0.5f; // Altura de la cámara respecto al jugador.

    private float rotationX = 0f; // Rotación acumulada en el eje vertical.
    private float rotationY = 0f; // Rotación acumulada en el eje horizontal.

    void Update()
    {
        // Rotación horizontal con A y D
        if (Input.GetKey(KeyCode.A))
        {
            rotationY -= rotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rotationY += rotationSpeed * Time.deltaTime;
        }

        // Rotación vertical con W y S
        if (Input.GetKey(KeyCode.W))
        {
            rotationX -= rotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rotationX += rotationSpeed * Time.deltaTime;
        }

        // Limitar la rotación vertical para evitar giros extraños
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        // Aplicar la rotación calculada a la cámara
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0f);
        transform.rotation = rotation;

        // Mantener la cámara en la posición del jugador con un offset en altura
        transform.position = player.position + Vector3.up * heightOffset;
    }
}
