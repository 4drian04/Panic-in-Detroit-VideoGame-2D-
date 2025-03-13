using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dificultad : MonoBehaviour
{
    private int opcionSeleccionada; //Declaramos esta variable para la manejar la opción elegida por el usuario
    // Start is called before the first frame update
    void Start()
    {
        opcionSeleccionada = 1; //Ponemos por defecto la primera opción
    }

    public void onClickButtonFacil()
    {
        opcionSeleccionada = 1; //Si selecciona la primera opción, la opción será 1
    }
    public void onClickButtonMedio()
    {
        opcionSeleccionada = 2; //Si elige la segunda, la opción será 2
    }

    public void onClickButtonDificil()
    {
        opcionSeleccionada = 3; //Por último, si elige la tercera, la opción será 3
    }

    public void onClickButtonJugar()
    {
        if (opcionSeleccionada == 1)//Si la opción es 1, el boss tendrá 300 de vida, y los zombies 40
        {
            Boss.vidaTotal = 300;
            PlayerPrefs.SetInt("vidaBoss", 300);
            Zombie.vidaTotal = 40;
            PlayerPrefs.SetInt("vidaZombie", 40);
        }
        else
        {
            if (opcionSeleccionada == 2) //Si la opción es 2, el boss tendrá 350 de vida, y los zombies 80
            {
                Boss.vidaTotal = 350;
                PlayerPrefs.SetInt("vidaBoss", 350);
                Zombie.vidaTotal = 80;
                PlayerPrefs.SetInt("vidaZombie", 80);
            }
            else //Si la opción es 3, el boss tendrá 400 de vida, y los zombies 100
            {
                Boss.vidaTotal = 400;
                PlayerPrefs.SetInt("vidaBoss", 400);
                Zombie.vidaTotal = 100;
                PlayerPrefs.SetInt("vidaZombie", 100);
            }
        }
        SceneManager.LoadScene("Controles"); //Por último se carga la escena "CityScene"
    }
    public void onClickButtonJugarPartida()
    {
        SceneManager.LoadScene("CityScene");
    }
}
