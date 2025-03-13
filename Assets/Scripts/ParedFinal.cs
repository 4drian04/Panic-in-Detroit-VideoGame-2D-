using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParedFinal : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Soldado.juegoFinalizado)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }
}
