using UnityEngine;
using System.Collections;

public class Enemy : Character
{
    float hitPoints;

    // Amount of damage the enemy will inflict when it runs into the player
    public int damageStrength;

    // Reference to a running coroutine
    Coroutine damageCoroutine;

    private void OnEnable()
    {
        ResetCharacter();
    }

    public override void ResetCharacter()
    {
        hitPoints = startingHitPoints;
    }

    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        // continuously inflict damage until the loop breaks
        while (true)
        {
            // inflict damage
            hitPoints = hitPoints - damage;

            // player is dead; kill off game object and exit loop
            if (hitPoints <= 0)
            {
                KillCharacter();
                break;
            }

            if (interval > 0)
            {
                // wait a specified amount of seconds and inflict more damage
                yield return new WaitForSeconds (interval);
            }
            else
            {
                // Intervale = 0; inflict one-time damage and exit loop
                break;
            }
        }
    }

    // Called by the Unity engine whenever the current enemy object's Collider2D makes contact with another object's Collider2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // See if the enemey has collided with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get a reference to the colliding player object
            Player player = collision.gameObject.GetComponent<Player>();

            // If coroutine is not currently executing
            if (damageCoroutine == null)
            {
                // start teh coroutien to inflict damage to the player every 1 second
                damageCoroutine = StartCoroutine(player.DamageCharacter(damageStrength, 1.0f));
            }
        }
    }

    // Called by the Unity engine whenever the current enemy object stops touching another object's Collider2d
    private void OnCollisionExit2D(Collision2D collision)
    {
        // See if the enemy has just stopped colliding with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // If coroutien is currrently executing
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }
}