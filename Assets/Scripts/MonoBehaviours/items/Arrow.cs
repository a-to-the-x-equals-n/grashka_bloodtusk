using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Amount of damage the ammunition will inflict on an enemy
    public int damageInflicted;

    // public Collider2D parentCollider;

    // Called when another object enters the trigger collider attached to the ammo gameobject
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
