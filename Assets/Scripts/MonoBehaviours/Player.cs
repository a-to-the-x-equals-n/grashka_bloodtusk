using UnityEngine;

public class Player : Character
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // trigger for any player collisions with consumable items
        if (collision.gameObject.CompareTag("CanBePickedUp"))
        {
            Item hitObject = collision.gameObject.GetComponent<Consumable>().item;

            if (hitObject != null)
            {
                // debugging
                print("it: " + hitObject.objectName);

                switch(hitObject.itemType)
                {
                    case Item.ItemType.COIN:
                        break;

                    case Item.ItemType.HEALTH:
                        AdjustHitPoints(hitObject.quantity);
                        break;
                    
                    default:
                        break;
                }
                                
                collision.gameObject.SetActive(false);
            }
        }
    }

    public void AdjustHitPoints(int amount)
    {
        hitPoints = hitPoints + amount;
        print("Adjusted hitpoints by: " + amount + ". New value: " + hitPoints);
    }
}
