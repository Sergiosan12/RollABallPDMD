using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private enum EnemyState { Idle, Chasing }
    private EnemyState currentState;

    public Transform player;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Idle;
    }

    void Update()
    {
        if (player != null)
        {
            if (currentState != EnemyState.Chasing) // Solo cambia si es diferente
            {
                currentState = EnemyState.Chasing;
                Debug.Log("El enemigo está persiguiendo al jugador."); // Debug para verificar el estado
            }
            
            navMeshAgent.SetDestination(player.position);
        }
        else
        {
            if (currentState != EnemyState.Idle)
            {
                currentState = EnemyState.Idle;
                Debug.Log("El enemigo está en espera.");
            }
        }
    }
}
