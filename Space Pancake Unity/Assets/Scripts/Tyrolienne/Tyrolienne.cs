using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tyrolienne : MonoBehaviour
{
    [Header("Paramètres Importants")] 
    public float start;
    
    public Vector2 cible;
    private float speedTyrolienne;
    public static bool usingTyrolienne;
    public float speedLimit;
    public float acceleration;
    private Vector2 direction;

    public GameObject poteau1;
    public GameObject poteau2;
    
    [Header("Player")]
    public GameObject player;
    public Character character;
    private Rigidbody2D rb;
    private float stockageGravity;
    private PlayerControls controls;
    
    [Header("PlayerAirControl")]
    public static bool noAirControl;
    private float timer;


    private void Awake()
    {
        controls = new PlayerControls();
    }
    
    private void OnEnable()
    {
        controls.Personnage.Enable();
    }

    private void OnDisable()
    {
        controls.Personnage.Disable();
    }



    private void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
        stockageGravity = rb.gravityScale;
        
        direction = new Vector2(poteau2.transform.position.x - poteau1.transform.position.x, poteau2.transform.position.y - poteau1.transform.position.y);
    }


    void Update()
    {
        if (usingTyrolienne && (player.transform.position.x < poteau2.transform.position.x && player.transform.position.x > poteau1.transform.position.x) && Detection.canUseZipline)
        {
            if (controls.Personnage.Sauter.WasPressedThisFrame())
            {
                timer = 0;
                character.Jump();
                usingTyrolienne = false;
                noAirControl = true;
            }
            
            rb.gravityScale = 0;

            if (speedTyrolienne < speedLimit)
            {
                speedTyrolienne += Time.deltaTime * acceleration;
            }
            
            rb.velocity = direction.normalized * speedTyrolienne;
        }

        else if (player.transform.position.x >= poteau2.transform.position.x || player.transform.position.x <= poteau1.transform.position.x || !usingTyrolienne)
        {
            usingTyrolienne = false;
            rb.gravityScale = stockageGravity;

            timer += Time.deltaTime;
            if (timer > 0.4f)
            {
                noAirControl = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        usingTyrolienne = true;
        speedTyrolienne = rb.velocity.x;
    }
}
