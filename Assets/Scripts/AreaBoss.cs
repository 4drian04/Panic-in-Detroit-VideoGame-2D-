using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBoss : MonoBehaviour
{
    public static bool estaSoldadoCerca; //Creamos una variable para comprobar que el soldado esta cerca
    public AudioSource enemigoDetectado; //Un sonido para cuando el robot vea al soldado
    // Start is called before the first frame update
    void Start()
    {
        estaSoldadoCerca = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player")) //Si entra en el rango del robot, la bool se pondrá a true y se reproducirá un sonido
        {
            estaSoldadoCerca=true;
            enemigoDetectado.Play();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player")) //Si el soldado sale del rango del robot, la bool se pone a false
        {
            estaSoldadoCerca = false;
        }
    }
}
