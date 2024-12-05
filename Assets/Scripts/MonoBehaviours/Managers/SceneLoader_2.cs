using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader_2 : MonoBehaviour
{
    public void LoadNextScene()
    {
        int curScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(curScene + 1);

    }

    public void ReloadCurrentScene()
    {
        int curScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(curScene);

    }

    public void LoadScenebyName(string BossLevel_3)
    {
        SceneManager.LoadScene(BossLevel_3);
    }
}
