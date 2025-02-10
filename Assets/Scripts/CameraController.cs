using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player; // Referencia al jugador (la bola).

    private enum CameraState { TerceraPersona, PrimeraPersona } // Estados de la cámara
    private CameraState currentState; // Estado actual de la cámara

    // Configuración para la vista en tercera persona.
    private Vector3 TerceraPersonaOffset;
    public float TerceraPersonaHeight = 10f;
    public float TerceraPersonaDistance = 10f;
    public float TerceraPersonaAngle = 90f;

    // Configuración para la vista en primera persona.
    public float rotationSpeed = 100f;
    public float PrimeraPersonaHeightOffset = 0.5f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    // Inicialización de la cámara.
    void Start()
    {
        TerceraPersonaOffset = new Vector3(0, TerceraPersonaHeight, -TerceraPersonaDistance);
        currentState = CameraState.TerceraPersona; // Inicia en tercera persona
    }

    void Update()
    {
        // Alternar entre cámaras con las teclas 1 y 2.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentState = CameraState.TerceraPersona;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentState = CameraState.PrimeraPersona;
        }

        // Si está en primera persona, permite la rotación de la cámara
        if (currentState == CameraState.PrimeraPersona)
        {
            CamaraPrimeraPersona();
        }
    }

    // Actualización de la cámara después de que todos los objetos se han movido.
    void LateUpdate()
    {
        if (player == null) // Si el jugador ha sido destruido, la cámara ya no lo sigue.
        {
            Debug.Log("El jugador ha sido destruido, la cámara ya no lo sigue.");
            return;
        }

        if (currentState == CameraState.PrimeraPersona) // Si está en primera persona, sigue al jugador con un offset.
        {
            transform.position = player.transform.position + Vector3.up * PrimeraPersonaHeightOffset;
        }
        else if (currentState == CameraState.TerceraPersona) // Si está en tercera persona, sigue al jugador con un offset.
        {
            Vector3 desiredPosition = player.transform.position + TerceraPersonaOffset;
            transform.position = desiredPosition;
            transform.LookAt(player.transform.position);
        }
    }

    // Rotación de la cámara en primera
    void CamaraPrimeraPersona()
    {
        if (Input.GetKey(KeyCode.A)) rotationY -= rotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) rotationY += rotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.W)) rotationX -= rotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) rotationX += rotationSpeed * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }
}
