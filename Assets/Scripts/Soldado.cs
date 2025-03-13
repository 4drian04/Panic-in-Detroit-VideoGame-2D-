using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Soldado : MonoBehaviour
{

    public CanvasGroup disolverCanvasGroup;
    public float tiempoDisolverEntrada;
    public float tiempoDisolverSalida;
    //Constantes necesarias para el juego
    public static float velocidad = 5f;
    private const int balasMaximoEnCargador = 10;
    
    //Datos importantes con el personajes
    public static float speed;
    public static float fuerzaSalto;
    public static int balasEnCargador;
    public static int balasTotales;
    public static int numeroGranadas;
    public static int lingotes;
    public static int salud;
    public static int saludTotal;
    public static bool muerto;
    //Variables necesarias para operaciones con el soldado, que no tienen que ver con el personajes como la salud o su velocidad
    private int sentidoX;
    private int sentidoY;
    public Transform pie;
    public LayerMask suelo;
    public Animator animator;
    private bool disparar;
    public Transform posicionDisparo;
    public GameObject bala;
    public GameObject granada;
    public static Transform scaleSoldado;
    public Text textoSalud;
    public Text textoLingotes;
    public Text textoBalas;
    public Text textoGranadas;
    public AudioSource disparoSonido;
    public AudioSource recargaSonido;
    public AudioSource sonidoFinal;
    public AudioSource sonidoMuerte;
    public AudioSource salto;
    public AudioSource armaSinBalas;
    private bool estaEnJefeFinal;
    public static bool juegoFinalizado;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        //Inicializamos las variables necesarias
        if (PlayerPrefs.HasKey("posicionX"))
        {
            transform.position = new Vector2(PlayerPrefs.GetFloat("posicionX"), PlayerPrefs.GetFloat("posicionY"));
            salud = PlayerPrefs.GetInt("salud");
            saludTotal = PlayerPrefs.GetInt("saludTotal");
            lingotes = PlayerPrefs.GetInt("lingotes");
            balasEnCargador = PlayerPrefs.GetInt("balasCargador");
            balasTotales = PlayerPrefs.GetInt("balas");
            fuerzaSalto = PlayerPrefs.GetFloat("fuerzaSalto");
            speed = PlayerPrefs.GetFloat("velocidad");
            numeroGranadas = PlayerPrefs.GetInt("numeroGranadas");
        }
        else
        {
            saludTotal = 100;
            speed = velocidad;
            salud = saludTotal;
            lingotes = 0;
            balasEnCargador = 10;
            balasTotales = 20;
            fuerzaSalto = 460f;
            numeroGranadas = 1;
        }
        sentidoX = 1;
        sentidoY = 1;
        scaleSoldado = transform;
        disparar = true;
        textoSalud.text = salud.ToString() + "/" + saludTotal.ToString();
        textoBalas.text = balasEnCargador + "/" + balasTotales;
        textoLingotes.text = lingotes.ToString();
        textoGranadas.text = numeroGranadas.ToString();
        muerto = false;
        estaEnJefeFinal = false;
        juegoFinalizado = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0 && !muerto)
        {
            //Cuando se pulsa el shift y está en el suelo y está andando, el personaje puede correr
            if (Input.GetKey(KeyCode.LeftShift) && estaEnSuelo() && animator.GetBool("andando"))
            {
                speed = velocidad * 2f;
                animator.SetBool("corriendo", true);
            }
            else //Si no está corriendo, se establece la velocidad normal
            {
                speed = velocidad * 1f;
                animator.SetBool("corriendo", false);
            }
            //Cuando se pulsa la tecla A o la flecha izquierda y el personaje no está recargando, el personaje puede andar hacia la izqueirda
            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && !animator.GetBool("recargando"))
            {
                transform.Translate(-sentidoX * speed *Time.deltaTime, 0, 0);
            }
            //Si pulsa la tecla D o la flecha derecha y no está recargando, el personaje se mueve a la derecha
            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !animator.GetBool("recargando"))
            {
                transform.Translate(sentidoX * speed * Time.deltaTime, 0, 0);
            }
            //Si pulsa la tecla W o la flecha hacia arriba y no está recargando el personaje puede saltar
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !animator.GetBool("recargando"))
            {
                //Pero previamente se comprueba si el personaje está en el suelo
                if (estaEnSuelo())
                {
                    saltar(); //Si está en el suelo, el personaje realiza la acción de saltar
                    salto.Play();
                }
            }
            //Si pulsa la tecla Espacio y no está recargando, ni andando, ni saltando, el personaje puede disparar
            if (Input.GetKeyDown(KeyCode.Space) && !animator.GetBool("recargando") && !animator.GetBool("andando") && disparar && !animator.GetBool("saltando"))
            {
                //Peor comprobamos antes si tiene balas en el cargador
                if (balasEnCargador > 0)
                {
                    accionDisparar();
                    disparoSonido.Play();
                }
                else
                {
                    armaSinBalas.Play();
                }
            }
            else //Si no está disparando, la animación de disparando se pone a false
            {
                animator.SetBool("disparando", false);
            }
            //Si pulsa la tecla R, y ha gastado mínimo una bala y no está saltando el personaje puede saltar
            if (Input.GetKeyDown(KeyCode.R) && balasEnCargador < balasMaximoEnCargador && !animator.GetBool("saltando"))
            {
                int balasRestanteRecargar = balasMaximoEnCargador - balasEnCargador; //Calculamos en primer lugar cuantas balas necesitaría recargar el personaje
                                                                                     //Si tiene más balas de las que necesita recargar, realiza la acción de recargar
                if (balasRestanteRecargar <= balasTotales)
                {
                    animator.SetBool("recargando", true);
                    recargaSonido.Play();
                    balasEnCargador += balasRestanteRecargar;
                    balasTotales -= balasRestanteRecargar;
                    textoBalas.text = balasEnCargador + "/" + balasTotales;
                    Invoke("quitarAnimacionRecarga", 1);

                }
            }
            if(Input.GetKeyDown(KeyCode.G) && numeroGranadas > 0 && estaSoldadoQuieto())
            {
                animator.SetBool("tirandoGranada", true);
                numeroGranadas--;
                textoGranadas.text = numeroGranadas.ToString();
                Invoke("tirarGranada", 0.5f);
            }
            listenerOrientacion(); //Esto nos permite que el personaje se gire cuando cambiamos de sentido en el eje X
            if (Granada.granadaImpactada)
            {
                textoSalud.text = salud.ToString() + "/" + saludTotal.ToString();
            }
        }
        muerteSoldado();
    }

    public bool getEstaEnJefeFinal()
    {
        return estaEnJefeFinal;
    }
    private void tirarGranada()
    {
        GameObject granadaInstanciada = Instantiate(granada, posicionDisparo.position, posicionDisparo.rotation);
        if (transform.localScale.x > 0)
        {
            granadaInstanciada.GetComponent<Rigidbody2D>().AddForce(new Vector2(350, 50));
        }
        else
        {
            granadaInstanciada.GetComponent<Rigidbody2D>().AddForce(new Vector2(-350, 50));
        }
        Invoke("quitarAnimacionGranada", 0.2f);
    }

    private void quitarAnimacionGranada()
    {
        animator.SetBool("tirandoGranada", false);
    }

    private bool estaSoldadoQuieto()
    {
        return !animator.GetBool("recargando") && !animator.GetBool("andando") && !animator.GetBool("saltando") && !animator.GetBool("disparando") && !animator.GetBool("corriendo");
    }

    private void muerteSoldado()
    {
        if (salud < 1)
        {
            muerto = true;
            sonidoMuerte.Play();
            animator.SetBool("muerto", true);
            Invoke("caidaSoldado", 0.8f);
        }
    }

    private void caidaSoldado()
    {
        animator.SetBool("caido", true);
        Invoke("dirigirPantallaMuerte", 1);
    }

    private void dirigirPantallaMuerte()
    {
        SceneManager.LoadScene("PantallaMuerte");
    }

    private void accionDisparar()
    {
        //Cuando dispara, se instancia una bala (prefabs), en la posicion del gameObject posicionDisparo
        Instantiate(bala, posicionDisparo.position, posicionDisparo.rotation);
        disparar = false; //Lo ponemos a false, ya que antes de llamar a este método comprobamos si el personaje puede disparar con este boolean, con esto evitamos que el usuario le de tantas veces a disparar y las animaciones no vayan bien, y haya un pequeño retraso
        animator.SetBool("disparando", true);
        balasEnCargador--; //Cuando dispara le quitamos una bala del cargador
        textoBalas.text = balasEnCargador + "/" + balasTotales;
        Invoke("permitirDisparar", 0.5f); //Cuando pase 0.5 segundos permitimos de nuevo al usuario que dispare
    }
    private void permitirDisparar()
    {
        disparar = true;
    }

    //Este método nos ayuda a quitar la animación de recarga, ya que hemos visto que la animación era muy larga y ayuda a la optimización del juego y mejora la experiencia del usuario
    private void quitarAnimacionRecarga()
    {
        animator.SetBool("recargando", false);
    }

    //Permite al usuario saltar, añadiéndole una fuerza al personaje
    private void saltar()
    {
        animator.SetBool("saltando", true);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, fuerzaSalto));
    }
    //Esto cambia de orientación al personaje dependiendo en que sentido se encuentre en el eje X
    private void listenerOrientacion()
    {
        float posicionX = transform.localScale.x;
        float posicionY = transform.localScale.y;
        float sentido = Input.GetAxis("Horizontal"); //Si el personaje se dirige hacia la izquierda da un valor <0 y en caso contrario da >0
        if (sentido != 0)
        {
            animator.SetBool("andando", true);
            if ((posicionX > 0 && sentido < 0) || (posicionX < 0 && sentido > 0))
            {
                transform.localScale = new Vector2(-posicionX, posicionY);
            }
        }
        else
        {
            animator.SetBool("andando", false);
        }

    }

    //Esto nos permite saber si el personaje está en el suelo, añadiendo un gameObject pie a nuestro personaje (situado en el pie del personaje)
    private bool estaEnSuelo()
    {
        return Physics2D.OverlapCircle(pie.position, 0.2f, suelo);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Esto es para que cuando el personaje esté cayendo de un sitio, haga la animcación de saltar, que también nos sirve cuando está cayendo
        if (collision.gameObject.tag.Equals("Suelo") && !estaEnSuelo())
        {
            animator.SetBool("saltando", true);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Suelo")) //Si está en el suelo le quitamos la animación de saltar
        {
            animator.SetBool("saltando", false);
        }
        if (collision.gameObject.tag.Equals("Municion"))
        {
            Destroy(collision.gameObject);
            balasTotales += 10;
            textoBalas.text = balasEnCargador + "/" + balasTotales;
        }
        if (collision.gameObject.tag.Equals("Lingote"))
        {
            Destroy(collision.gameObject);
            lingotes += 20;
            textoLingotes.text = lingotes.ToString();
        }
        if (collision.gameObject.tag.Equals("Cura"))
        {
            sonidoFinal.Play();
            Destroy(collision.gameObject);
            juegoFinalizado = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Checkpoint"))
        {
            PlayerPrefs.SetFloat("posicionX", transform.position.x);
            PlayerPrefs.SetFloat("posicionY", transform.position.y);
            PlayerPrefs.SetInt("balas", balasTotales);
            PlayerPrefs.SetInt("balasCargador", balasEnCargador);
            PlayerPrefs.SetInt("salud", salud);
            PlayerPrefs.SetInt("saludTotal", saludTotal);
            PlayerPrefs.SetFloat("fuerzaSalto", fuerzaSalto);
            PlayerPrefs.SetInt("lingotes", lingotes);
            PlayerPrefs.SetFloat("velocidad", speed);
            PlayerPrefs.SetInt("botonHabilitado", 1); //Esto lo guardamos, ya que así en el menú, se habilitaría el botón para continuar desde el último checkpoints
            PlayerPrefs.SetInt("numeroGranadas", numeroGranadas);
            if (collision.gameObject.name.Equals("SiguienteMapa"))
            {
                PlayerPrefs.SetFloat("posicionX", -20.82f);
                PlayerPrefs.SetFloat("posicionY", -2.08f);
                DisolverSalida("LabScene");
            }
        }
        if (collision.gameObject.tag.Equals("SueloMuerte"))
        {
            salud = 0;
            muerto = true;
        }
        if (collision.gameObject.name.Equals("Pared2"))
        {
            estaEnJefeFinal = true;
            transform.position = new Vector2(406.47f, 20);
            collision.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
        if (collision.gameObject.name.Equals("Final")) //Esto es cuando llega choca con un collider, que es cuando derrota al jefe final
        {
            float volumen = PlayerPrefs.GetFloat("volumen");
            PlayerPrefs.DeleteAll(); //Como ha terminado el juego, borramos los playerPrefs para que empiece de nuevo
            PlayerPrefs.SetFloat("volumen", volumen); //Esto es para no perder el PlayerPrefs del volumen
            DisolverSalida("Creditos");
        }
    }
    private void DisolverSalida(string nombreEscena)
    {
        disolverCanvasGroup.blocksRaycasts = true;
        disolverCanvasGroup.interactable = true;

        LeanTween.alphaCanvas(disolverCanvasGroup, 1f, tiempoDisolverSalida).setOnComplete(() => {
            SceneManager.LoadScene(nombreEscena);
        });
    }
}
