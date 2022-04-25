using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockageVariablesPerso : MonoBehaviour
{
    public bool moveLeft;
    public bool moveRight;
    public bool run;

    public StockageVariablesPerso Instance;

    
    void Start()
    {
        Instance = this;

        Character.Instance.moveLeft = moveLeft;
        Character.Instance.moveRight = moveRight;
    }
    
    
    void Update()
    {
        if (!Character.Instance.noControl)
        {
            moveLeft = Character.Instance.moveLeft;
            moveRight = Character.Instance.moveRight;
        }
        else
        {
            Character.Instance.moveLeft = moveLeft;
            Character.Instance.moveRight = moveRight;
        }
    }
}
