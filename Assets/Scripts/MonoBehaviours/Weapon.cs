using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Reference to the ammo prefab, used to create new ammo objects
    public GameObject ammoPrefab;

    // The ammo pool; static means only one copy will exist
    static List<GameObject> ammoPool;

    // Number of objects to pre-instantiate in the pool
    public int poolSize;

    // Velocity of ammo fired from the weapon
    public float weaponVelocity;

    // Called when the script is being loaded
    private void Awake()
    {
        // Create the pool
        ammoPool ??= new List<GameObject>();

        // Loop to create ammo objects and add to the pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject ammoObject = Instantiate(ammoPrefab);
            ammoObject.SetActive(false);
            ammoPool.Add(ammoObject);
        }
    }

    // Called when the gameobject is destroyed
    private void OnDestroy()
    {
        ammoPool = null;
    }

    // Called each frame
    private void Update()
    {
        // Check to see if user has clicked the mouse to fire the slingshot
        // Parameter 0 checks for left mouse button; 1 checks for right
        if (Input.GetMouseButtonDown(0))
        {
            FireAmmo();
        }
    }

    // Retrieves and returns an activated AmmoObject from the object pool
    // Location: where to place the retrieved AmmoObject
    GameObject SpawnAmmo(Vector3 location)
    {
        foreach (GameObject ammo in ammoPool)
        {
            // Check to see if the current object is inactive
            if (ammo.activeSelf == false)
            {
                // Activate it
                ammo.SetActive(true);

                // Set it's location
                ammo.transform.position = location;

                return ammo;
            }
        }

        // Return null if all objects in the pool are currently being used
        return null;
    }

    // Responsible for moving the AmmoObject from the spawned location to the endpoint where the mouse was clicked
    private void FireAmmo()
    {
        // Convert the mouse position from screen space to world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Get a new ammo object located at the weapon's current position
        GameObject ammo = SpawnAmmo(transform.position);

        // Make sure you got an ammo object
        if (ammo != null)
        {
            // Get reference to the arc script
            Arc arcScript = ammo.GetComponent<Arc>();

            // Calculate the amount of time for ammo travel
            // Example: if velocity is 2, then 1/2 = 0.5 or a half second to travel across the screen
            float travelDuration = 1.0f / weaponVelocity;

            StartCoroutine(arcScript.TravelArc(mousePosition, travelDuration));
        }
    }
}
