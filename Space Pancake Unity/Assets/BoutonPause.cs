using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoutonPause : MonoBehaviour
{
    public Vector3 posOriginale;
    public bool desactivate;

    public GameObject boutonMenu;


    void Awake()
    {
        posOriginale = transform.position;
    }
    
    
    void Update()
    {
        if (MenuManager.Instance.optionsActives && !desactivate)
        {
            gameObject.GetComponent<Button>().interactable = false;
        }

        else if (!MenuManager.Instance.optionsActives && !desactivate)
        {
            gameObject.GetComponent<Button>().interactable = true;
            boutonMenu.transform.position = new Vector3(-8.912598f, 8.755188f, 0);
        }
    }
}
