using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;


public class TextBoxRotateIntro : MonoBehaviour
{
    // Reference to the TextMeshProUGUI component
    public TextMeshProUGUI textBox;
   
    // Array of strings for different lines
    public string[] textBlocks = new string[] {
        "Princess Flay: Ugh! Mom I’m so bored. Tell me the story about how you got your axe again!",
        "Queen Grashka: You would prefer boredom to war, my child. This peace is why I ventured for the axe in the first place…",
        "Princess Flay: Pleeeeease!!!",
        "Queen Grashka: It would serve you well to use manners, small one. As you know The Devil himself once roamed this realm like you and I, despite my warning him to stick to his own domain."
    };


    private int currentTextIndex = 0;

    // Reference to the Devil_still GameObject
    public GameObject Devil_still;


    void Start()
    {
        if (textBlocks.Length > 0)
        {
            textBox.text = textBlocks[currentTextIndex];  // Set initial text
        }
    }


    void Update()
    {
        if (currentTextIndex > 9)
        {
            Devil_still.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Space) && currentTextIndex != 17)
        {
            currentTextIndex = (currentTextIndex + 1) % textBlocks.Length;
            textBox.text = textBlocks[currentTextIndex];
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Hell_1");   
        }
    }
}

