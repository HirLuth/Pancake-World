using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bird : MonoBehaviour
{
    public AnimationCurve trajectoireX;
    public AnimationCurve trajectoireY;
    public float vitesseTrajectoire;
    private float avanceeTrajectoire;

    private float facteur;

    private Vector3 originalPosition;
    private bool fly;


    private void Start()
    {
        originalPosition = transform.position;
        
        facteur = Random.Range(-0.1f, 0.1f);
    }


    private void Update()
    {
        if (fly)
        {
            avanceeTrajectoire += Time.deltaTime * (vitesseTrajectoire + facteur);

            transform.position = originalPosition + new Vector3(trajectoireX.Evaluate(avanceeTrajectoire), trajectoireY.Evaluate(avanceeTrajectoire), 0);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Character")
            fly = true;
    }
}
