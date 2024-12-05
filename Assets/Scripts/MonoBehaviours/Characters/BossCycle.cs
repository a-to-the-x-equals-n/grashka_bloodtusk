using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCycle : MonoBehaviour
{
    float attackTime = 0;
    string animationState = "AnimationState";
    Animator animator;
    public int speed = 1;
    GameObject player;
    Rigidbody2D rb2D;
    Vector2 movement = new Vector2();
    enum CharStates
    {
        walkEast = 1,
        walkSouth = 2,
        walkWest = 3,
        walkNorth = 4,
        idle = 5,
        attack = 6
    }
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    void UpdateState()
    {
        if (attackTime < 200)
        {
            // x axis
            if (movement.x > 0.5)
                animator.SetInteger(animationState, (int)CharStates.walkEast);
            else if (movement.x < -0.5)
                animator.SetInteger(animationState, (int)CharStates.walkWest);
            // y axis
            else if (movement.y > 0)
                animator.SetInteger(animationState, (int)CharStates.walkNorth);
            else if (movement.y < 0)
                animator.SetInteger(animationState, (int)CharStates.walkSouth);
            // idle
            else
                animator.SetInteger(animationState, (int)CharStates.idle);
        }
        else
        {
            //Attack on time interval
            animator.SetInteger(animationState, (int)CharStates.attack);
        }
            
    }
    private void FixedUpdate()
    {
        MoveAwayFromPlayer();
        //Reset time interval after attack animation
        attackTime ++;
        if (attackTime > 350)
        {
            attackTime = 0;
        }
    }
    private void MoveAwayFromPlayer()
    {
        player = GameObject.Find("Grashka(Clone)");
        if (player != null)
        {
            if (attackTime < 200)
            {
                //Find the direction to the player
                movement = transform.position - player.transform.position;
                
                movement.Normalize();
                
                rb2D.velocity = movement * speed;
            }
            else
            {
                rb2D.velocity = Vector2.zero;
            }
        }
    }
}
