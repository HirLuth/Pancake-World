using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoutonPause : MonoBehaviour
{
    void Update()
    {
        if (MenuManager.Instance.optionsActives)
        {
            gameObject.GetComponent<Button>().interactable = false;
        }

        else if (!MenuManager.Instance.optionsActives)
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
    }
}
