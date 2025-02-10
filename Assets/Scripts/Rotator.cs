using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    // Se llama una vez por frame
    void Update()
    {
        transform.Rotate (new Vector3 (0, 0, 50) * Time.deltaTime); // Rota el objeto 50 grados por segundo en el eje Z 
    }
}
