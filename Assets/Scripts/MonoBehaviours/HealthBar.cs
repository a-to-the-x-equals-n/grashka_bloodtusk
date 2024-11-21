using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Reference to the same HitPoints scriptable object that the player prefab refers to
    // This container allows sharing of data between two objects
    public HitPoints hitPoints;

    // Reference to the current Player objecft to get maxHitPoints
    // Will be set programmatically, instead of through the Unity Editor, so it is hidden in the Inspector window
    [HideInInspector]
    public Player character;

    // For convenience, a direct reference to the health bar meter; set through the Unity Editor
    public Image meterImage;

    // For convenience, a direct reference to the text in the health bar; set through the Unity Editor
    public Text hpText;

    // Cache the max hit points in a local variable
    float maxHitPoints;


    // Start is called before the first frame update
    void Start()
    {
        // Retrieve and store max hit points for the character
        maxHitPoints = character.maxHitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (character != null)
        {
            meterImage.fillAmount = hitPoints.value / maxHitPoints;

            hpText.text = "HP: " + (meterImage.fillAmount * 100);
        }
    }
}
