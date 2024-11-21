using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // A reference to the slot prefab object; attached in the unity editor
    public GameObject slotPrefab;

    // Number of slots that the inventory bar contains
    public const int NUMSLOTS = 5;

    // Holds references to slot prefabs
    GameObject[] slots = new GameObject[NUMSLOTS];

    // An array to hold the image components
    Image[] itemImages = new Image[NUMSLOTS];

    // Holds references to the actual item (scriptable object) that the player picked up
    Item[] items = new Item[NUMSLOTS];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
