using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float movementSpeed;

    //Holds 2D points, used to represent a characters location in 2D space, or where its moving to
    Vector2 movement = new Vector2();

    //Holds reference to the animator component in the game object
    Animator animator;

    string animationState = "AnimationState";

    Rigidbody2D rb2D;


    // enumerated constants to correspond to the values assigned to the animations
    enum CharStates
    {
        walkEast = 1,
        walkSouth = 2,
        walkWest = 3,
        walkNorth = 4,
        idleSouth = 5

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
        //update the animation state machine
        UpdateState();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void UpdateState()
    {
        if (movement.x > 0)
            animator.SetInteger(animationState, (int)CharStates.walkEast);
        else if (movement.x < 0)
            animator.SetInteger(animationState, (int)CharStates.walkWest);
        else if (movement.y > 0)
            animator.SetInteger(animationState, (int)CharStates.walkNorth);
        else if (movement.y < 0)
            animator.SetInteger(animationState, (int)CharStates.walkSouth);
        else
            animator.SetInteger(animationState, (int)CharStates.idleSouth);
    }

    private void MoveCharacter()
    {
        //get user input

        //Allows us to specify which axis we're interested in
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //keeps players moving at the same rate of speed, no matter which direction they are moving in
        movement.Normalize();

        rb2D.velocity = movement * movementSpeed;
    }

}