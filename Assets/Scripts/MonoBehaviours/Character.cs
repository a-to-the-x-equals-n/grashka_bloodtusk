using UnityEngine;


//  Make the class abstract as it will need to be inherited by a subclass
public abstract class Character : MonoBehaviour
{
    // Properties common to all characters
    public int hitPoints;
    public int maxHitPoints;
}
