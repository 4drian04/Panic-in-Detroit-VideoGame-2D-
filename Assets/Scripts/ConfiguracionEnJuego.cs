using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfiguracionEnJuego : MonoBehaviour
{
    public GameObject menuConfiguracion; //Obtenemos tanto el menú de configuración como el HUD del usuario
    public GameObject arrayUI; 
    
    public void onClickButtonConfiguracion() //Si le da click al botón de configuración, desactivamos el HUD del usuario y paramos el juego
    {
        arrayUI.SetActive(false);
        Time.timeScale = 0;
        menuConfiguracion.SetActive(true); //Actiavamos además el menú de configuración
    }
    public void onClickButtonContinuar() //Cuando le da al botón de continuar, se activa el HUD del usuario y se reanuda el juego
    {
        arrayUI.SetActive(true);
        Time.timeScale = 1;
        menuConfiguracion.SetActive(false); //Además, se desactiva el menú de configuración
    }
    public void onClickButtonSalir() //Cuando le da al botón de salir, cargamos la escena del menú principal
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}
