using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class botonesOpciones : MonoBehaviour
{
    public void onClickButtonSalir()
    {
        Application.Quit();
    }
    public void onClickBottonMenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}
