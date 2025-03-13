using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraManager : MonoBehaviour
{
    public GameObject personaje; //Se necesitar� el GameObject del personaje
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (personaje.gameObject.transform.position.x > 1 && !personaje.GetComponent<Soldado>().getEstaEnJefeFinal())
        {
            transform.position = personaje.gameObject.transform.position - new Vector3(0, 0, 13); //Ponemos que la c�mara siga al personaje, tanto en el eje x como en el y
        }
        else
        {
            if (personaje.GetComponent<Soldado>().getEstaEnJefeFinal())
            {
                transform.position = new Vector3(427.3f, 23.1f, -10);
                GetComponent<Camera>().orthographicSize = 12.07f;
            }
        }
    }
}
