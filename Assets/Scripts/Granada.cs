using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Granada : MonoBehaviour
{
    public Animator animator; //Nos servir� para la animaci�n de la granada
    public GameObject granada; //Es el gameObject de la granada, ya que est� Script se utiliza en el area de la granda, que es un gameObject hijo de la granada
    private bool estaCercaSoldado; //Luego, definimos los siguientes tres bool para comprobar si hay algun enemigo o jugador cerca
    private bool estaCercaZombie;
    private bool estaCercaBoss;
    private GameObject boss; //Obtenemos tanto los gameObject del boss como del zombie, para quitarles da�o en caso de que la granada impacte en ellos
    private GameObject zombie;
    public static bool granadaImpactada; //Esto comprueba si la granada ha impactado en el soldado, esto nos servir� para actualizar el HUD.
    public AudioSource granadaExplotando;
    // Start is called before the first frame update
    void Start()
    {
        //Lo igualamos todo a false en el inicio, y el grupo ha pensado que 2 segundos es un tiempo correcto para que explote la granada
        granadaImpactada = false;
        estaCercaBoss = false;
        estaCercaSoldado = false;
        estaCercaZombie = false;
        Invoke("explotarGranada", 2);
    }
    private void explotarGranada()
    {
        animator.SetBool("granadaExplotada", true); //Cuando explota la granada, se realiza la animaci�n de explotar
        Quaternion rotacion = granada.gameObject.transform.rotation; /*Se obtiene la rotaci�n de la granada, esto nos va servir para poner la rotaci�n a 0, 
                                                                      ya que como la granada se mueve y rota, luego lo ponemos a 0 para que la animaci�n de explotar se haga correctamente
                                                                      sin que este tambi�n dando vueltas la explosi�n*/
        rotacion.z = 0;
        granada.gameObject.transform.rotation = rotacion; //Ponemos la respectiva rotaci�n modificada en la granda
        granada.gameObject.GetComponent<Rigidbody2D>().freezeRotation = true; //Congelamos las rotaciones, para que no pueda seguir rodando la granada.
        granada.gameObject.GetComponent<CircleCollider2D>().radius = 0.1f;
        granada.gameObject.transform.localScale = new Vector2(5, 5); //La linea anterior y esta nos sirve ya que como la granada es muy peque�a, la otra animaci�n tambi�n es bastante peque�a,
        //y lo que hacemos es, una vez est� en la animaci�n de explosi�n, redimensionamos la granada para que la explosi�n se vea mucho m�s grande, y no se vea como el tama�o de la granada
        granadaExplotando.Play();
        Invoke("destruirGranada", 1.5f); //Por �ltimo, destuimos la granada una vez terminado la explosi�n
    }

    private void destruirGranada()
    {
        //Antes de destruir la granada, vemos si hay alguien cerca para quitarle su respectivo da�o
        if (estaCercaBoss)
        {
            boss.GetComponent<Boss>().recibirDisparo(40);
        }
        if (estaCercaZombie)
        {
            zombie.GetComponent<Zombie>().recibirDisparo(50);
        }
        if (estaCercaSoldado)
        {
            Soldado.salud -= 40;
            granadaImpactada = true;
        }
        Destroy(granada.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si alg�n enemigo entra en el area de la granada, ponemos los respectivos bool a true
            if (collision.gameObject.tag.Equals("Enemigo"))
            {
                estaCercaZombie = true;
                zombie = collision.gameObject;
            }
            if (collision.gameObject.tag.Equals("Boss"))
            {
                estaCercaBoss = true;
                boss = collision.gameObject;
            }
            if (collision.gameObject.tag.Equals("Player"))
            {
                estaCercaSoldado = true;
            }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Sin embargo, si alguno se sale, ponemos los respectivos bool a false
        if (collision.gameObject.tag.Equals("Enemigo"))
        {
            estaCercaZombie = false;
            boss = collision.gameObject;
        }
        if (collision.gameObject.tag.Equals("Boss"))
        {
            estaCercaBoss = false;
            zombie = collision.gameObject;
        }
        if (collision.gameObject.tag.Equals("Player"))
        {
            estaCercaSoldado = false;
        }
    }
}
