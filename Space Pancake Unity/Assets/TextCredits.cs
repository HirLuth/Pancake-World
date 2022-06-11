using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TextCredits : MonoBehaviour
{
    void Update()
    {
        if (ScriptFinDeJeu.Instance.endEvent)
        {
            if (ScriptFinDeJeu.Instance.launchCredits && !ScriptFinDeJeu.Instance.stopCredits)
            {
                transform.DOMoveY(transform.position.y + 150, 5);
            }
            else
            {
                transform.DOMoveY(transform.position.y, 5);
            }
        }
    }
}
