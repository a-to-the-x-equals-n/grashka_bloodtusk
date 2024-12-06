using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class TextBoxRotateOutro : MonoBehaviour
{
    // Reference to the TextMeshProUGUI component
    public TextMeshProUGUI textBox;
   
    // Array of strings for different lines
    public string[] textBlocks = new string[] {
        "Princess Rikka: Mom! Is it over?",
        "Queen Grashka: Yes, child. I finished the job, like I should have done last time. Now you are truly safe. Let us go home, unless this excitement has fallen short of quenching your boredom.",
        "Princess Rikka: Ugh… no mom. You were right, I miss being bored…",
        "THE END"
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
        // Cycle through text blocks
        if (Input.GetKeyDown(KeyCode.Space) && currentTextIndex != 3)
        {
            currentTextIndex = (currentTextIndex + 1) % textBlocks.Length;
            textBox.text = textBlocks[currentTextIndex];
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Application.Quit();   
        }
    }
}
