using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Parallax : MonoBehaviour
{
    private float length;
    private float startPos;
    
    public GameObject cam;
    public float parallaxEffect;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        startPos = cam.transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        Debug.Log(startPos + length);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        Debug.Log(temp);
        
        if (temp > startPos + length)
        {
            startPos += length;
        }
        else if (temp < startPos - length)
        {
            startPos -= length;
        }
    }
}
