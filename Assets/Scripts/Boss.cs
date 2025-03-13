using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public static int vidaTotal;
    private float scaleBoss;
    public Animator animator;
    private float sentidoX;
    private float velocidad = 5f;
    private int vida;
    public GameObject cura;
    private float sentido;
    private bool atacando;
    private bool chocandoConSoldado;
    public Transform coordenadaPersonaje;
    public AudioSource robotAtacando;
    public AudioSource roborMuriendose;
    public Text saludSoldado;
    // Start is called before the first frame update
    void Start()
    {
        vidaTotal = PlayerPrefs.GetInt("vidaBoss"); //Igualamos la vida total del boss dependiendo de la dificultad elegida por el usuario
        chocandoConSoldado = false;
        atacando = false;
        vida = vidaTotal;
        sentidoX = -1; //El sentido por defecto que tiene el boss al reaparecer
        scaleBoss = transform.localScale.x;
        sentido = scaleBoss;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale != 0 && !Soldado.muerto) //Si no está el juego en pausa y el soldado no está muerto, el boss puede realizar estas acciones:
        {
            if (AreaBoss.estaSoldadoCerca) //Si el soldado está cerca, el boss andará hacia los lados (ya que el boss se encuentra en una habitación y andará de un lado a otro)
            {
                animator.SetBool("BossAndando", true);
                transform.Translate(sentidoX * velocidad * Time.deltaTime, 0, 0);
                transform.localScale = new Vector2(sentido, transform.localScale.y);
            }
            else //Si el soldado no se encuentra cerca, el boss no anda
            {
                animator.SetBool("BossAndando", false);
            }
            if (chocandoConSoldado) //Si el boss está chocando con el soldado, podrá atacarle
            {
                if (!atacando) //Para que el juego sea equilibrado, hemos pensado que el boss tampoco pueda atacar todo el rato, y tenga un "cooldown" para que pueda atacar de nuevo
                {
                    atacando = true; //Si entra en el if, ponemos el boo de atacadno a true
                    animator.SetBool("BossAtacando", true); 
                    robotAtacando.Play();
                    Soldado.salud -= 20; //Se le quitara 20 de vida al soldado
                    if (Soldado.salud < 0)
                    {
                        saludSoldado.text = 0 + "/" + Soldado.saludTotal.ToString();
                    }
                    else
                    {
                        saludSoldado.text = Soldado.salud.ToString() + "/" + Soldado.saludTotal.ToString();
                    }
                     //Se actualizará en el HUD
                    Invoke("quitarAnimacionAtacar", 0.5f);
                    Invoke("permitirAtacar", 1); //Ponemos que pueda volver a atacar cuando pase un segundo
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Pared")) //Si se choca con alguna pared de la habitación, cambiará de sentido para andar hacia el otro lado
        {
            sentidoX = -sentidoX;
            sentido = -sentido;
        }
        if (collision.gameObject.tag.Equals("Player")) //Si se choca con el soldado, se pondra la bool "chocandoConSoldado" a true
        {
            chocandoConSoldado = true;
        }
    }

    private void quitarAnimacionAtacar()
    {
        animator.SetBool("BossAtacando", false);
    }
    private void permitirAtacar()
    {
        atacando = false;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player")) //Si el soldado se sale de la colisión del boss, la bool "chocandoConSoldado" se pondrá a false
        {
            animator.SetBool("BossAtacando", false);
            chocandoConSoldado = false;
        }
    }
    /*Esto permitirá que el boss reciba saño por parte del soldado*/
    public void recibirDisparo(int danho)
    {
        vida -= danho;
        if (vida <= 0)
        {
            animator.SetBool("BossMuerte", true);
            Invoke("destruirBoss", 0.5f); //Si el boss se queda sin vidas, se destruirá el boss
        }
    }
    private void destruirBoss() //Se desruirá el boss y soltará la cura final del juego
    {
        GameObject curaSoltada = Instantiate(cura, gameObject.transform.position, gameObject.transform.rotation);
        curaSoltada.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 50));
        Destroy(gameObject);
        roborMuriendose.Play();
    }
}