using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Zombie : MonoBehaviour
{
    public static int vidaTotal;
    public Transform coordenadaPersonaje;
    public Animator animator;
    private int sentidoX;
    private float scaleZombie;
    private float velocidad = 3f;
    private int vida;
    private bool chocandoConSoldado = false;
    private bool atacando = false;
    public GameObject municion;
    public GameObject lingote;
    public Text textoSalud;
    public AudioSource atacarZombie;
    // Start is called before the first frame update
    void Start()
    {
        vidaTotal = PlayerPrefs.GetInt("vidaZombie"); //Cuando se empieza, si o si habr� un PlayerPrefs de "vidaZombie", ya que el usuario previamente ha tenido que elegir la dificultad, guardandose �l playerPrefs de la vida del zombie
        vida = vidaTotal; //Se iguala la vida a la vida total
        sentidoX = 1; //Este es el sentido por defecto que tendr�n los zombies al reaparecer
        scaleZombie = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        float coordenada = 1;
        float sentido = transform.localScale.x;
        if (Time.timeScale != 0 && !Soldado.muerto) //Si el juego no est� pausado y el soldado no ha muerto
        {
            if (GetComponentInChildren<AreaZombie>().getEstaPersonajeCerca()) //Obtenemos si el soldado est� cerca del zombie
            {
                /*Si est� cerca, el zombie empezar� a andar hacia el soldado*/
                animator.SetBool("andandoZombie", true);
                coordenada = coordenadaPersonaje.localPosition.x - transform.localPosition.x; //Calculamos si el soldado est� a la izquierda o derecha del zombie
                if (coordenada > 0) //Si esta a la derecha del zombie, andar� hacia la derecha
                {
                    sentido = scaleZombie;
                    transform.Translate(sentidoX * velocidad * Time.deltaTime, 0, 0);
                }
                else //Sin embargo, en caso contrario, se mover� a la izquierda
                {
                    sentido = -scaleZombie;
                    transform.Translate(-sentidoX * velocidad * Time.deltaTime, 0, 0);

                }
                transform.localScale = new Vector2(sentido, transform.localScale.y); //Esto es para que el zombie, a parte de andar a la izquierda o derecha, est� mirando hacia el soldado
            }
            else //Si no hay ning�n soldado cerca, el zombie no andar�
            {
                animator.SetBool("andandoZombie", false);
            }
            if (chocandoConSoldado) //Si el soldado est� chocando con el zombie, el zombie podr� atacar
            {
                if (!atacando && vida>0) //Para que sea un juego equilibrado y disfutable, vamos a controlar que ataque una vez cada segundo y medio
                {
                    atacando = true; //Si no est� atacando, entra en este if y se pone la variable de atacando a true
                    animator.SetBool("atacar", true);
                    Soldado.salud += -20; //Se resta 20 de vida al soldado
                    if (Soldado.salud < 0)
                    {
                        textoSalud.text = 0 + "/" + Soldado.saludTotal.ToString();
                    }
                    else
                    {
                        textoSalud.text = Soldado.salud.ToString() + "/" + Soldado.saludTotal.ToString();
                    }
                    atacarZombie.Play(); //Suena un sonido cuando el zombie ataca
                    Invoke("quitarAnimacionAtacar", 0.5f); //Se quita la animaci�n de atacar
                    Invoke("permitirAtacar", 1.5f); //Esperamos un segundo y medio para que pueda volver a atacar
                }

            }
        }
    }
    private void quitarAnimacionAtacar()
    {
        animator.SetBool("atacar", false);
    }
    private void permitirAtacar()
    {
        atacando = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Soldado")) //Cuando entra en colisi�n con el soldado, se pone la variable "chocandoConSoldado" a true
        {
            chocandoConSoldado = true;

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Soldado")) //Se deja de colisionar con el soldado, se pone la variable "chocandoConSoldado" a false
        {
            chocandoConSoldado = false;
            animator.SetBool("atacar", false);
        }
    }
    /*Este m�todo permite que el zombie pueda recibir da�o por parte del soldado, ya sea cuando dispare o cuando le impacte una granada*/
    public void recibirDisparo(int danho) 
    {
        vida -= danho;
        if (vida <= 0)
        {
            animator.SetBool("morir", true); //Si muere, har� la animaci�n de muerte
            Invoke("destruirZombie", 0.5f); //Se destruye el zombie una vez se haya terminado la animaci�n
        }
    }
    /*Esto destruye el zombie, pero adem�s, suelta un par de objetos de utilidad para el soldado, que son balas y lingotes*/
    private void destruirZombie()
    {
        GameObject municionaSoltada = Instantiate(municion, gameObject.transform.position, gameObject.transform.rotation);
        municionaSoltada.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-50, 50)); //Esto es para que de un efecto tipico de los juegos cuando un enemigo suelta algo (ya sea balas, cura, etc)
        GameObject lingoteSoltado = Instantiate(lingote, new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y, gameObject.transform.position.z), gameObject.transform.rotation);
        lingoteSoltado.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(50, 50));
        Destroy(gameObject);
    }
}