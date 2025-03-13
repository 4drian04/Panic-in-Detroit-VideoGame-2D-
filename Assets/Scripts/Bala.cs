using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    private float velocidad=20; //Velocidad de la bala
    private int danho=20; //Daño de la bala
    private bool disparoRealizado = false; //El disparo no se ha realizado al inciio
    private bool estaMirandoADerechaSoldado = true; //Suponemos que el soldado está mirando a la derecha
    // Start is called before the first frame update
    void Start()
    {
        if (!disparoRealizado)
        {
            disparoRealizado = true;
            if (Soldado.scaleSoldado.localScale.x < 0) //Si el soldado no está mirando a la derecha, ponemos la bool "estaMirandoADerechaSoldado" a false
            {
                estaMirandoADerechaSoldado = false;

            }
        }
        Invoke("destruirBala", 2); //En caso de que antes de 2 segundos no haya impactado con ningún enemigo, se destruye.
    }

    // Update is called once per frame
    void Update()
    {
        
        if (estaMirandoADerechaSoldado) //Si está mirando a la derecha, la bala va hacia la derecha
        {
            transform.Translate(Vector2.right * velocidad * Time.deltaTime);
        }
        else //En caso contrario, la bala va hacia la izquierda
        {
            transform.Translate(Vector2.left * velocidad * Time.deltaTime);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemigo")) //Si impacta con un zombie, le quitara la vida correspondiente al zombie, y la bala se destruye
        {
            collision.GetComponent<Zombie>().recibirDisparo(danho);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag.Equals("Boss")) //Si impacta con el boss, le quitara la vida correspondiente al boss, y la bala se destruye
        {
            collision.GetComponent<Boss>().recibirDisparo(danho);
            Destroy(gameObject);
        }
    }
    private void destruirBala()
    {
        Destroy(gameObject);
    }
}
