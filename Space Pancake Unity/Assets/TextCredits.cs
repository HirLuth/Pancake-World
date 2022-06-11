using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TextCredits : MonoBehaviour
{
    void Update()
    {
        if (ScriptFinDeJeu.Instance.launchCredits)
        {
            transform.DOMoveY(transform.position.y + 150, 5);
        }
    }
}
