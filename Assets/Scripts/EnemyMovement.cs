using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private enum EnemyState { Inactivo, Siguiendo } // Estados del enemigo
    private EnemyState currentState;

    public Transform player;
    private NavMeshAgent navMeshAgent;

   // Inicialización del enemigo.
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Inactivo;
    }

   // Actualización del enemigo.
    void Update()
    {
        if (player != null) // Si el jugador no ha sido destruido
        {
            if (currentState != EnemyState.Siguiendo) // Si el enemigo no está siguiendo al jugador
            {
                currentState = EnemyState.Siguiendo; // Cambia el estado a siguiendo
                Debug.Log("El enemigo está persiguiendo al jugador."); // Debug para verificar el estado
            }
            
            navMeshAgent.SetDestination(player.position); // Establece la posición del jugador como destino
        }
        else
        {
            if (currentState != EnemyState.Inactivo) // Si el enemigo no está en espera
            {
                currentState = EnemyState.Inactivo; // Cambia el estado a inactivo
                Debug.Log("El enemigo está en espera.");
            }
        }
    }
}
