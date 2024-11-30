using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : MonoBehaviour
{
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
            Debug.LogError("Player object not found");
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
}
