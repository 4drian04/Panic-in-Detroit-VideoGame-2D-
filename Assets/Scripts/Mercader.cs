using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mercader : MonoBehaviour
{
    //Declaramos los distintos precios de los productos
    private const int precioMunicion = 5;
    private const int precioVendas = 10;
    private const int precioBotiquin = 30;
    private const int precioAumentoSalud = 100;
    private const int precioAumentoVelocidad = 50;
    private const int precioAumentoSalto = 50;
    private const int precioGranadas = 80;
    //Declaramos las variables correspondientes
    private bool estaSoldadoCerca;
    public Animator animator;
    public GameObject UI;
    public GameObject pantallaTienda;
    public Text textoMunicion;
    public Text textoVida;
    public Text textoLingote;
    public Text textoGranadas;
    public AudioSource sonidoChaqueta;
    public AudioSource compraEfectiva;
    public AudioSource compraFallida;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.X) && estaSoldadoCerca) //Si el soldado está cerca y le da a la tecla X, se pausa el juego, y se desactiva el HUD del jugador, y la pantalla de la tienda se activa
        {
            Time.timeScale = 0;
            UI.SetActive(false);
            pantallaTienda.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player")) //Si un jugador se acerca al area del mercader, se hace una animación concreta, se reprodce un sonido
            //y se pone la bool "estaSoldadoCerca" a true
        {
            animator.SetBool("tradeando", true);
            sonidoChaqueta.Play();
            estaSoldadoCerca = true;
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player")) //Si el jugador se aleja del rango del mercader, se hace lo contrario a lo anterior
        {
            //Se pone la bool "estaSoldadoCerca" a false
            animator.SetBool("tradeando", false);
            sonidoChaqueta.Play();
            estaSoldadoCerca = false;
            animator.SetBool("terminarTradear", true);

        }
    }

    //Dependiendo a que botón seleccione para comprar, se compra una cosa u otra, previamente comprobando que tiene los lingotes suficientes.
    public void onClickButtonComprarMunicion()
    {
        if (Soldado.lingotes >= precioMunicion)
        {
            Soldado.balasTotales += 20;
            Soldado.lingotes -= precioMunicion;
            textoMunicion.text = Soldado.balasEnCargador + "/" + Soldado.balasTotales;
            textoLingote.text = Soldado.lingotes.ToString();
            compraEfectiva.Play();
        }
        else
        {
            compraFallida.Play();
        }
    }

    public void onClickButtonComprarVendas()
    {
        if (Soldado.lingotes > precioVendas)
        {
            Soldado.salud += 25;
            if (Soldado.salud >= Soldado.saludTotal)
            {
                Soldado.salud = Soldado.saludTotal;
            }
            textoVida.text = Soldado.salud.ToString() + "/" + Soldado.saludTotal.ToString();
            Soldado.lingotes -= precioVendas;
            textoLingote.text = Soldado.lingotes.ToString();
            compraEfectiva.Play();
        }
        else
        {
            compraFallida.Play();
        }
    }
    public void onClickButtonComprarBotiquin()
    {
        if (Soldado.lingotes >= precioBotiquin)
        {
            Soldado.salud = Soldado.saludTotal;
            Soldado.lingotes -= precioBotiquin;
            textoVida.text = Soldado.salud.ToString() + "/" + Soldado.saludTotal.ToString();
            textoLingote.text = Soldado.lingotes.ToString();
            compraEfectiva.Play();
        }
        else
        {
            compraFallida.Play();
        }
    }

    public void onClickButtonAumentoSalud()
    {
        if (Soldado.lingotes >= precioAumentoSalud)
        {
            Soldado.saludTotal += 25;
            Soldado.salud = Soldado.saludTotal;
            Soldado.lingotes -= precioAumentoSalud;
            textoVida.text = Soldado.salud.ToString() + "/" + Soldado.saludTotal.ToString();
            textoLingote.text = Soldado.lingotes.ToString();
            compraEfectiva.Play();
        }
        else
        {
            compraFallida.Play();
        }
    }
    public void onClickButtonAumentoVelocidad()
    {
        if (Soldado.lingotes >= precioAumentoVelocidad)
        {
            Soldado.velocidad += 0.5f;
            Soldado.lingotes -= precioAumentoVelocidad;
            textoLingote.text = Soldado.lingotes.ToString();
            compraEfectiva.Play();
        }
        else
        {
            compraFallida.Play();
        }
    }
    public void onClickButtonAumentarSalto()
    {
        if (Soldado.lingotes >= precioAumentoSalto)
        {
            Soldado.fuerzaSalto += 50f;
            Soldado.lingotes -= precioAumentoSalto;
            textoLingote.text = Soldado.lingotes.ToString();
            compraEfectiva.Play();
        }
        else
        {
            compraFallida.Play();
        }
    }
    public void onClickButtonVolver()
    {
        Time.timeScale = 1;
        UI.SetActive(true);
        pantallaTienda.SetActive(false);
    }

    public void onClickButtonGranadas()
    {
        if (Soldado.lingotes >= precioGranadas)
        {
            Soldado.numeroGranadas++;
            Soldado.lingotes -= precioGranadas;
            textoGranadas.text = Soldado.numeroGranadas.ToString();
            textoLingote.text = Soldado.lingotes.ToString();
            compraEfectiva.Play();
        }
        else
        {
            compraFallida.Play();
        }
    }
}
