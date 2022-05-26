using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoutonContinue : MonoBehaviour
{
    public Button bouton;

    public Color okayColor;
    public Color notOkayColor;
    
    // Start is called before the first frame update
    void Start()
    {
        bouton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SpawnPointManagement.spawnWasModifiedOnce)
        {
            ColorBlock cb = bouton.colors;
            cb.normalColor = okayColor;
            bouton.colors = cb;
        }

        else
        {
            ColorBlock cb = bouton.colors;
            cb.normalColor = notOkayColor;
            bouton.colors = cb;
        }
    }
}
