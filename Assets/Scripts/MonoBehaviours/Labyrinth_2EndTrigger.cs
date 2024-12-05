using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Labyrinth_2EndTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FindAnyObjectByType<SceneLoader>().LoadScenebyName("Boss Level_3");
        }
    }
}
