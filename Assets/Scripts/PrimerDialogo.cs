using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimerDialogo : MonoBehaviour
{
    //Este script será utilizado unicamente para el principio, donde sale el audio de inicio
    public AudioSource primerDialogo;
    private bool audioEscuchado=false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(("Player")) && !audioEscuchado)
        {
            primerDialogo.Play();
            audioEscuchado = true;
        }
    }
}
