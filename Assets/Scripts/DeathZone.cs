using UnityEngine;
using UnityEngine.SceneManagement;  // Para reiniciar la escena

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Asegúrate de que la bola tenga el tag "Player"
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia la escena
        }
    }
}
