using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cofre : MonoBehaviour
{
    public Animator animator; //Ainmaci�n del cofre
    //Los objetos que soltar� el cofre
    public GameObject lingote;
    public GameObject municion;
    
    private bool estaEnRango; //Con esto controlamos si el soldado est� cerca o no
    private bool esAbierto = false; //Inicializamos a false la bool que controla si el cofre est� abierto o no
    public AudioSource abriendose; //Sonido para cuando se abra el cofre
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!esAbierto && estaEnRango && Input.GetKeyDown(KeyCode.X)) //Si no est� abierto, el jugador est� en el rango y le da a la tecla X, el cofre se abrira
        {
            esAbierto = true;
            animator.SetBool("cofreAbierto", true);
            abriendose.Play();
            Invoke("soltarObjectoLingote", 0.5f); //Hacemos que suelte un objeto primero y luego otro
            Invoke("soltarObjectoMunicion", 1f);
            animator.SetBool("cofreUsado", true);
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            estaEnRango = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            estaEnRango = false;
        }
    }
    private void soltarObjectoLingote()
    {
        //Instanciamos el lingote (prefab) en la posici�n del cofre
        GameObject lingoteSoltado = Instantiate(lingote, new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity);
        //Le a�adimos una fuerza, que esto lo que har� un efecto t�pico de los juegos cuando abres un cofre, que sale hacia arriba y hacia el lado
        lingoteSoltado.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-100, 150));
    }
    private void soltarObjectoMunicion()
    {
        //Instanciamos la munici�n (prefab) en la posici�n del cofre
        GameObject municionSoltada = Instantiate(municion, new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity);
        //Le a�adimos una fuerza, que esto lo que har� un efecto t�pico de los juegos cuando abres un cofre, que sale hacia arriba y hacia el lado
        municionSoltada.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(100, 150));
    }
}
