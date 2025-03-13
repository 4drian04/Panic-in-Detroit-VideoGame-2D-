using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cofre : MonoBehaviour
{
    public Animator animator; //Ainmación del cofre
    //Los objetos que soltará el cofre
    public GameObject lingote;
    public GameObject municion;
    
    private bool estaEnRango; //Con esto controlamos si el soldado está cerca o no
    private bool esAbierto = false; //Inicializamos a false la bool que controla si el cofre está abierto o no
    public AudioSource abriendose; //Sonido para cuando se abra el cofre
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!esAbierto && estaEnRango && Input.GetKeyDown(KeyCode.X)) //Si no está abierto, el jugador está en el rango y le da a la tecla X, el cofre se abrira
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
        //Instanciamos el lingote (prefab) en la posición del cofre
        GameObject lingoteSoltado = Instantiate(lingote, new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity);
        //Le añadimos una fuerza, que esto lo que hará un efecto típico de los juegos cuando abres un cofre, que sale hacia arriba y hacia el lado
        lingoteSoltado.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-100, 150));
    }
    private void soltarObjectoMunicion()
    {
        //Instanciamos la munición (prefab) en la posición del cofre
        GameObject municionSoltada = Instantiate(municion, new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity);
        //Le añadimos una fuerza, que esto lo que hará un efecto típico de los juegos cuando abres un cofre, que sale hacia arriba y hacia el lado
        municionSoltada.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(100, 150));
    }
}
