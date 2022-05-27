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
        if (SpawnPointManagement.spawnPointLocation.x == 0 || !SpawnPointManagement.spawnWasModifiedOnce)
        {
            ColorBlock cb = bouton.colors;
            cb.normalColor = notOkayColor;
            bouton.colors = cb;
        }

        else
        {
            ColorBlock cb = bouton.colors;
            cb.normalColor = okayColor;
            bouton.colors = cb;
        }
    }
}
