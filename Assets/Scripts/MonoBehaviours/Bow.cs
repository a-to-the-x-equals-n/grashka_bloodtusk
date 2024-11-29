using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class EnemyWeapon : MonoBehaviour
{
    // Reference to the ammo prefab, used to create new ammo objects
    public GameObject ammoPrefab;

    // Velocity of ammo fired from the weapon
    public float weaponVelocity;

    // Reference to the target (e.g., the player)
    public Transform target;

    // Firing interval in seconds
    public float fireInterval = 2f;

    // Whether the enemy should fire at the player
    private bool isFiring = false;



    // Called when the gameobject is destroyed
    private void OnDestroy()
    {
        
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
        GameObject ammo = Instantiate(ammoPrefab, transform.position, Quaternion.identity);

        // Rotate the ammo to face the direction
        ammo.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

        // Get reference to the Arc or projectile script
        Arc arcScript = ammo.GetComponent<Arc>();

        if (arcScript != null)
        {
            // Calculate the travel duration
            float travelDuration = 1.0f / weaponVelocity;

            // Use the Arc script for the projectile's movement
            StartCoroutine(arcScript.TravelArc(endPosition, travelDuration));
        }

        // Despawn ammo after 5 seconds
        Destroy(ammo, 5f);
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
            target = other.transform; // Set the target to the player's transform
            isFiring = true; // Start firing
            StartCoroutine(FireArrow());
        }
    }

    // Called when something exits the enemy's CircleCollider2D
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isFiring = false; // Stop firing
            StopCoroutine(FireArrow()); // Stop the firing routine
        }
    }
}
