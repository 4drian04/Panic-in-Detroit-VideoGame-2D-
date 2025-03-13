using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class botonesScripts : MonoBehaviour
{
    public Button botonContinuar;

    private void Start()
    {
        if (PlayerPrefs.HasKey("botonHabilitado")){

            botonContinuar.enabled = true;
        }
        else
        {
            botonContinuar.enabled = false;
        }
    }
    public void onClickButtonJugar()
    {
        SceneManager.LoadScene("LabScene"); //Aqui pondríamos el nombre de la escena donde reaparecerá el protagonista
    }
    public void onClickButtonOpciones()
    {
        SceneManager.LoadScene("Opciones"); //Aqui se pone la escena de configuración.
    }

    public void onClickButtonSalir()
    {
        Application.Quit();
    }
    public void onClickButtonNuevaPartida()
    {
        PlayerPrefs.DeleteAll(); //Si selecciona una nueva partida, se elimina todos los playerPrefs que existan
        SceneManager.LoadScene("MenuDificultad");
    }
    public void oncClickButtonControles()
    {
        SceneManager.LoadScene("ControlesInfo");
    }
}
