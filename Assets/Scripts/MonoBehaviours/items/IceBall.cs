using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : MonoBehaviour
{
    // Amount of damage the ammunition will inflict on an enemy
    public int damageInflicted;
    public int speed = 1;
    GameObject player;
    Rigidbody2D rb2D;
    Vector2 movement = new Vector2();
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        // Find the player object
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (player == null)
        {
            player = GameObject.Find("Grashka(Clone)");
        }
        MoveTowardPlayer();
        //transform.position += transform.forward * Time.deltaTime * 10;
    }

    private void MoveTowardPlayer()
    {
        
        if (player != null)
        {
            // Find the direction to the player
            movement = player.transform.position - transform.position;
            // Normalize the direction to get a unit vector
            movement.Normalize();
            // Move the ice ball in the direction of the player
            rb2D.velocity = movement * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check that we have hit the box collider inside the enemy, and not it's circle collider
        if (collision.gameObject.CompareTag("Player"))
        {
            // Retrieve the player script from the enemy object
            Player player = collision.gameObject.GetComponent<Player>();

            // Start the damage coroutine; 0.0f will inflict a one-time damage
            StartCoroutine(player.DamageCharacter(damageInflicted, 0.0f));

            // Since the ammo has struck the enemy, set the ammo gameobject to be inactive
            // Note it is inactive -- not "destroyed" so we can use object pooling for better performance
            gameObject.SetActive(false);
        }
        if (collision is BoxCollider2D) // && collision != parentCollider)
        {
            gameObject.SetActive(false);
        }
    }
}
