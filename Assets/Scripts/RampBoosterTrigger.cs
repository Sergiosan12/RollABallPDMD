using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampBoostTrigger : MonoBehaviour
{
    public float boostForce = 10f; // Fuerza del empuje

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica que la bola tenga el tag "Player"
        {
            Rigidbody rb = other.GetComponent<Rigidbody>(); // Obtiene el Rigidbody de la bola
            if (rb != null)
            {
                Vector3 boostDirection = transform.forward + Vector3.up; // Dirección del boost (hacia arriba y adelante)
                rb.AddForce(boostDirection * boostForce, ForceMode.Impulse); // Aplica la fuerza de impulso
            }
        }
    }
}
