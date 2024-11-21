using System.Collections;
using UnityEngine;

public class Player : Character
{
    public HitPoints hitPoints;

    // Used to get a reference to the prefab
    public Inventory inventoryPrefab;

    // A copy of the inventory prefab
    Inventory inventory;

    // Used to get a reference to the prefab
    public HealthBar healthBarPrefab;

    // A copy of the health bar prefab
    HealthBar healthBar;

    // Part of MonoBehaviour class; onEnable is called every time an object becomes both enabled and active
    private void OnEnable()
    {
        ResetCharacter();
    }

    public override void ResetCharacter()
    {
        // Get a copy of the inventory prefab and store a reference to it
        inventory = Instantiate(inventoryPrefab);

        // Start teh player off with the starting hit point value
        hitPoints.value = startingHitPoints;

        // Get a copy of the health bar prfefab and store a reference to it
        healthBar = Instantiate(healthBarPrefab);

        // Set the healthBar's character property to this cahracter so it can retrieve the maxHitPoints
        healthBar.character = this;
    }

    // Called when palyer's collider touches ans "Is Trigger" collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // trigger for any player collisions with consumable items
        // Retrive the game object that the player collided with, and check the tag
        if (collision.gameObject.CompareTag("CanBePickedUp"))
        {

            // Grab a reference to the Item (scriptable object) inside the Consumable class and assign it toe hitObject
            Item hitObject = collision.gameObject.GetComponent<Consumable>().item;

            // Check for null to make sure it was successfully retrieved, and avoid potential errors
            if (hitObject != null)
            {
                // indicates if the collision object should disappear
                bool shouldDisappear = false;

                // debugging
                print("it: " + hitObject.objectName);

                switch(hitObject.itemType)
                {
                    case Item.ItemType.COIN:
                        // coins will disappear if they can be added to the inventory
                        shouldDisappear = inventory.AddItem(hitObject);
                        break;

                    case Item.ItemType.HEALTH:
                        // hearts should disappear if they adjust the player's hit points
                        // when health meter is full, hearts aren't picked up and remain in the scene
                        shouldDisappear = AdjustHitPoints(hitObject.quantity);
                        break;
                    
                    default:
                        break;
                }

                if (shouldDisappear)
                {
                    collision.gameObject.SetActive(false);
                }   
            }
        }
    }

    public bool AdjustHitPoints(int amount)
    {
        // don't increase above the max amount
        if (hitPoints.value < maxHitPoints)
        {
            hitPoints.value = hitPoints.value + amount;
            print("Adjusted hitpoints by: " + amount + ". New value: " + hitPoints);
            return true;
        }

        // return false if hit poitns is at max and can't be adjsuted
        return false;
    }

    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        // continuously inflict damage until the loop breaks
        while (true)
        {
            // inflict damage
            hitPoints.value = hitPoints.value - damage;

            // player is dead; kill off game object and exit loop
            if (hitPoints.value <= 0)
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

    public override void KillCharacter()
    {
        // Call KillCharacter in parent(Character) class, which will destroy thec player game object
        base.KillCharacter();

        // Destroy health and inventory bars
        Destroy(healthBar.gameObject);
        Destroy(inventory.gameObject);
    }
}
