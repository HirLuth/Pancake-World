using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    [SerializeField] private EventManager eventManager;
    public static LoadManager instance;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        eventManager = EventManager.Instance;
        eventManager.UI = UIPrincipale.Instance.gameObject;
        eventManager.score = UIPrincipale.Instance.textScore;
        eventManager.AddPoints(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
