using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathHandler : MonoBehaviour
{
    public Animator animator;
    //public GameOverManager gameOverManager;
    public float deathAnimationDuration = 2f;
    public string gameOverSceneName = "GameOver";

    public void TriggerDeath()
    {
        animator.SetTrigger("Die");
        Invoke(nameof(TriggerGameOver), deathAnimationDuration);
    }

    private void TriggerGameOver()
    {
        //gameOverManager.TriggerGameOver();
        SceneManager.LoadScene(gameOverSceneName);
    }

    public void OnDeathAnimationEnd()
    {
      
        SceneManager.LoadScene(gameOverSceneName);
    }
}