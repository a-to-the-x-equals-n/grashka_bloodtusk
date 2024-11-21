using UnityEngine;

public class Player : Character
{
    // Used to get a reference to the prefab
    public Inventory inventoryPrefab;

    // A copy of the inventory prefab
    Inventory inventory;

    // Used to get a reference to the prefab
    public HealthBar healthBarPrefab;

    // A copy of the health bar prefab
    HealthBar healthBar;

    public void Start()
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
        if (hitPoints.value < maxHitPoints)
        {
            hitPoints.value = hitPoints.value + amount;
            print("Adjusted hitpoints by: " + amount + ". New value: " + hitPoints);
            return true;
        }
        return false;
    }
}
