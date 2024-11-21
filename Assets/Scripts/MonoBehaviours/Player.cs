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

                collision.gameObject.SetActive(false);
            }
        }
    }
}
