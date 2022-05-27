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
        if (SpawnPointManagement.instance.GetSpawn() == new Vector2(-47.3f, -4.2f))
        {
            ColorBlock cb = bouton.colors;
            cb.normalColor = notOkayColor;
            bouton.colors = cb;
            bouton.interactable = false;
        }

        else
        {
            ColorBlock cb = bouton.colors;
            cb.normalColor = okayColor;
            bouton.interactable = true;
            bouton.colors = cb;
        }
    }
}
