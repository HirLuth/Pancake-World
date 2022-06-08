using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoutonPause : MonoBehaviour
{
    public Vector3 posOriginale;
    public bool desactivate;

    public GameObject boutonMenu;
    
    

    void Start()
    {
        posOriginale = transform.position;
    }
    
    
    void Update()
    {
        if (MenuManager.Instance.optionsActives && !desactivate)
        {
            gameObject.GetComponent<Button>().interactable = false;

            if (boutonMenu != null)
            {
                if (FirstSelected.Instance.bouton != boutonMenu)
                    boutonMenu.transform.position = new Vector3(-8.912598f, 9.101929f, 0);
            }
        }

        else if (!MenuManager.Instance.optionsActives && !desactivate)
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
    }
}
