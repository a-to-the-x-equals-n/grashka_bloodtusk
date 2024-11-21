using UnityEngine;

// Create an entry in the "Create submenu" to be able to easily create instances of the object
[CreateAssetMenu(menuName = "HitPoints")]
public class HitPoints : ScriptableObject
{
    // The health meter image will need a float value
    public float value;
}
