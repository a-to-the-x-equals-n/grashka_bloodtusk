using UnityEngine;

public class ThrownAxe : MonoBehaviour
{
    // Amount of damage the ammunition will inflict on an enemy
    public int damageInflicted;

    // Called when another object enters the trigger collider attached to the ammo gameobject
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if (collision is Collider2D)
        // Check that we have hit the box collider inside the enemy, and not it's circle collider
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Retrieve the player script from the enemy object
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            // Start the damage coroutine; 0.0f will inflict a one-time damage
            StartCoroutine(enemy.DamageCharacter(damageInflicted, 0.0f));
        }
    }
}
