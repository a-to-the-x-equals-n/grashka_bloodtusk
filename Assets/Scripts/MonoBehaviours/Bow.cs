using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class EnemyWeapon : MonoBehaviour
{
    // Reference to the ammo prefab, used to create new ammo objects
    public GameObject arrowPrefab;

    // Velocity of ammo fired from the weapon
    public float arrowVelocity;

    // Reference to the target (e.g., the player)
    public Transform target;

    // Firing interval in seconds
    public float fireInterval = 2f;

    // Whether the enemy should fire at the player
    private bool isFiring = false;

    // Reference to the enemy's collider
    private Collider2D enemyCollider; 
    private Collider2D arrowCollider;

    private void Awake()
    {
        // Cache this enemy's collider
        enemyCollider = GetComponent<Collider2D>();
    }


    // Method to trigger the enemy weapon to fire
    public void FireAtTarget()
    {
        if (target == null) return;

        // Calculate the direction towards the target
        Vector3 direction = (target.position - transform.position).normalized;

        // Calculate the end position based on the direction (optional)
        Vector3 endPosition = transform.position + direction * 10f; // 10f = distance to travel 'til despawn

        // Get a new ammo object located at the weapon's current position
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);

        // Ignore collision between the arrow and the firing enemy
        arrowCollider = arrow.GetComponent<Collider2D>();

        if (enemyCollider != null && arrowCollider != null)
        {
            Physics2D.IgnoreCollision(enemyCollider, arrowCollider);
        }

        // Rotate the ammo to face the direction
        arrow.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

        // Get reference to the Arc or projectile script
        Arc arcScript = arrow.GetComponent<Arc>();

        if (arcScript != null)
        {
            // Calculate the travel duration
            float travelDuration = 1.0f / arrowVelocity;

            // Use the Arc script for the projectile's movement
            StartCoroutine(arcScript.TravelArc(endPosition, travelDuration));
        }

        // Despawn ammo after 5 seconds
        Destroy(arrow, 5f);
    }

    // Coroutine to handle firing intervals
    private IEnumerator FireArrow()
    {
        while (isFiring)
        {
            FireAtTarget();
            yield return new WaitForSeconds(fireInterval);
        }
    }

    // Called when something enters the enemy's CircleCollider2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isFiring)
            {
                target = other.transform; // Set the target to the player's transform
                isFiring = true; // Start firing
                StartCoroutine(FireArrow());
            }
        }
    }

    // Called when something exits the enemy's CircleCollider2D
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isFiring = false; // Stop firing
            StopCoroutine(FireArrow()); // Stop the firing routine
            new WaitForSeconds(fireInterval);
        }
    }

    private IEnumerator FireWait()
    {
        yield return new WaitForSeconds(2f);
    }
}
