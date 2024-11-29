using UnityEngine;

public class Ammo : MonoBehaviour
{
    // Amount of damage the ammunition will inflict on an enemy
    public int damageInflicted;

    // Called when another object enters the trigger collider attached to the ammo gameobject
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check that we have hit the box collider inside the enemy, and not it's circle collider
        if (collision is BoxCollider2D)
        {
            // Retrieve the enemy script from the enemy object
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            // Start the damage coroutine; 0.0f will inflict a one-time damage
            StartCoroutine(enemy.DamageCharacter(damageInflicted, 0.0f));

            // Since the ammo has struck the enemy, set the ammo gameobject to be inactive
            // Note it is inactive -- not "destroyed" so we can use object pooling for better performance
            gameObject.SetActive(false);
        }
    }
}
