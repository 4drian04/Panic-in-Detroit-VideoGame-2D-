using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaZombie : MonoBehaviour
{
    private bool estaPersonajeCerca;
    public AudioSource soldadoCerca;
    // Start is called before the first frame update
    void Start()
    {
        estaPersonajeCerca = false; //Inicializamos que el soldado esté cerca a false
    }

    public bool getEstaPersonajeCerca() //Hacemos un get del bool para usarlo en el script de Zombie
    {
        return estaPersonajeCerca;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player")) //Si en el area entra el soldado, ponemos el bool a true
        {
            estaPersonajeCerca = true;
            soldadoCerca.Play();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player")) //Si se sale del rango de visión del zombie, ponemos el bool a false
        {
            estaPersonajeCerca = false;
        }
    }
}
