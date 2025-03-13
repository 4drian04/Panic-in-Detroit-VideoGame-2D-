using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransicionEscenaUI : MonoBehaviour
{
    //Este script nos permite hacer transiciones más naturales entre una escena y otra, utilizando un framework llamado LeanTween

    public static TransicionEscenaUI instance;

    public CanvasGroup disolverCanvasGroup;
    public float tiempoDisolverEntrada;
    public float tiempoDisolverSalida;
    // Start is called before the first frame update
    void Start()
    {
        DisolverEntrada();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /*Esto nos permite que, hacer una transición cuando se cargue la escena, que será usado en el start*/
    private void DisolverEntrada()
    {
        LeanTween.alphaCanvas(disolverCanvasGroup, 0f, tiempoDisolverEntrada).setOnComplete(() => {
            disolverCanvasGroup.blocksRaycasts = false;
            disolverCanvasGroup.interactable = false;
        });
    }
    
}
