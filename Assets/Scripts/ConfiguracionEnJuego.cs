using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfiguracionEnJuego : MonoBehaviour
{
    public GameObject menuConfiguracion; //Obtenemos tanto el men� de configuraci�n como el HUD del usuario
    public GameObject arrayUI; 
    
    public void onClickButtonConfiguracion() //Si le da click al bot�n de configuraci�n, desactivamos el HUD del usuario y paramos el juego
    {
        arrayUI.SetActive(false);
        Time.timeScale = 0;
        menuConfiguracion.SetActive(true); //Actiavamos adem�s el men� de configuraci�n
    }
    public void onClickButtonContinuar() //Cuando le da al bot�n de continuar, se activa el HUD del usuario y se reanuda el juego
    {
        arrayUI.SetActive(true);
        Time.timeScale = 1;
        menuConfiguracion.SetActive(false); //Adem�s, se desactiva el men� de configuraci�n
    }
    public void onClickButtonSalir() //Cuando le da al bot�n de salir, cargamos la escena del men� principal
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}
