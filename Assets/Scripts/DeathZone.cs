using UnityEngine;
using UnityEngine.SceneManagement;  // Para reiniciar la escena

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Si el objeto que colisiona es el jugador
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia la escena
        }
    }
}
