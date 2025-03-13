using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class MenuOpciones : MonoBehaviour
{
    public AudioMixer audioMixer; //El audio mixer donde se controla el volumen de los sonidos
    public Slider valorSlider; //El slider del volumen
    private void Start()
    {
        if (PlayerPrefs.HasKey("volumen")) //Si existe un volumen guardado, se carga dicho volumen
        {
            cargarVolumen(); 
        }
    }
    public void cambiarVolumen(float volumen) //Esto nos permitirá cambiar el volumen utilizando el slider
    {
        audioMixer.SetFloat("Volumen", volumen);
        PlayerPrefs.SetFloat("volumen", volumen);
    }
    private void cargarVolumen() //Se carga el volumen del PlayerPrefs
    {
        float volumen = valorSlider.value = PlayerPrefs.GetFloat("volumen");
        cambiarVolumen(volumen);
    }
}
