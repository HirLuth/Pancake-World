using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoutonContinue : MonoBehaviour
{
    public Button bouton;

    public Color okayColor;
    public Color notOkayColor;
    
    
    void Start()
    {
        bouton = GetComponent<Button>();
    }
    
    
    void Update()
    {
        if (!MenuManager.Instance.optionsActives && SpawnPointManagement.instance.GetSpawn().x > 0)
        {
            ColorBlock cb = bouton.colors;
            cb.normalColor = okayColor;
            bouton.interactable = true;
            bouton.colors = cb;
        }

        else
        {
            ColorBlock cb = bouton.colors;
            cb.normalColor = notOkayColor;
            bouton.colors = cb;
            bouton.interactable = false;
        }
    }
}
