using UnityEngine;
using System.Collections;
using Unity.VisualScripting;


public class MinotaurCharge : Wander
{
    public float chargeSpeed;
    public float chargeCooldown;
    private bool coolingDown = false;
    private bool stunned = false;
    public float stunDuration;

    protected override void Start()
    {
        base.Start(); // Call the parent class's Start method

        Debug.Log("MinotaurCharge initialized!");
    }


    protected override IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        float distance = (transform.position - endPosition).sqrMagnitude; // Get distance between enemy position and destination

        // Check if targetTransform is null and exit if there's no valid target
        if (targetTransform != null && !coolingDown)
        {
            currentSpeed = chargeSpeed;
            endPosition = targetTransform.position;

            while (distance > float.Epsilon)
            {
                if (rigidBodyToMove != null)
                {
                    // Check if stunned
                    if (stunned)
                    {
                        yield break; // Exit
                    }

                    // Set animation parameter so animator will change the anumations that's played
                    animator.SetBool("isWalking", true);

                    // Calculates the movement for a RigidBody2D
                    Vector3 direction = (endPosition - transform.position).normalized;
                    Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed * Time.deltaTime);
                    
                    // Determine the direction to adjust the animation state
                    // X AXIS
                    if (direction.x > 0) animator.SetInteger(animationState, (int)CharStates.walkEast);
                    else if (direction.x < 0) animator.SetInteger(animationState, (int)CharStates.walkWest);
                    // Y AXIS
                    else if (direction.y > 0) animator.SetInteger(animationState, (int)CharStates.walkNorth);
                    else if (direction.y < 0) animator.SetInteger(animationState, (int)CharStates.walkSouth);
                    else animator.SetInteger(animationState, (int)CharStates.idleSouth);
                        
                    // Move the RigidBody2D
                    rb2d.MovePosition(newPosition);
                    
                    // Update the distance remaining
                    distance = (transform.position - endPosition).sqrMagnitude;
                }
                yield return new WaitForFixedUpdate();
            }
            StartCoroutine(ChargeCooldownRoutine()); // Start the cooldown
        }
        else
        {
            while (distance > float.Epsilon)
            {
                if (targetTransform != null)
                {
                    // If targeTransform is set, then it's postion is the player's position
                    // This moves the enemy toward the player instead of toward the original endPosition
                    endPosition = targetTransform.position;
                }

                if (rigidBodyToMove != null)
                {
                    // Set animation parameter so animator will change the anumations that's played
                    animator.SetBool("isWalking", true);

                    // Calculates the movement for a RigidBody2D
                    Vector3 direction = (endPosition - transform.position).normalized;
                    Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed * Time.deltaTime);
                    
                    // Determine the direction to adjust the animation state
                    // X AXIS
                    if (direction.x > 0) animator.SetInteger(animationState, (int)CharStates.walkEast);
                    else if (direction.x < 0) animator.SetInteger(animationState, (int)CharStates.walkWest);
                    // Y AXIS
                    else if (direction.y > 0) animator.SetInteger(animationState, (int)CharStates.walkNorth);
                    else if (direction.y < 0) animator.SetInteger(animationState, (int)CharStates.walkSouth);
                    else animator.SetInteger(animationState, (int)CharStates.idleSouth);
                        
                    // Move the RigidBody2D
                    rb2d.MovePosition(newPosition);
                    
                    // Update the distance remaining
                    distance = (transform.position - endPosition).sqrMagnitude;
                }
                // Pause execution until the next Fixed Frame Update
                yield return new WaitForFixedUpdate();
            }
        }
        // Stop walking / idle
        animator.SetBool("isWalking", false);
        animator.SetInteger(animationState, (int)CharStates.idleSouth);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D called"); // Debug log
        if (collision.gameObject.CompareTag("Player") && followPlayer)
        {
            // If on cooldown, do nothing
            if (stunned) return;

            currentSpeed = coolingDown ? pursuitSpeed : chargeSpeed; // Set currentSpeed based on cooldown state
            targetTransform = collision.gameObject.transform; // Set the targetTransform to the player's position

            // If enemy is already moving, stop the previous coroutine
            if (moveCoroutine != null) StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed)); // Start the move coroutine
        }
    }

    private IEnumerator ChargeCooldownRoutine()
    {
        currentSpeed = pursuitSpeed;
        coolingDown = true; // Enter cooldown
        yield return new WaitForSeconds(chargeCooldown); // Wait for cooldown duration
        coolingDown = false; // Exit cooldown
    }

    private IEnumerator StunRoutine()
    {
        stunned = true; // Set the stunned state
        animator.SetBool("isWalking", false); // Stop walking animation
        // animator.SetTrigger("Stunned"); // Play stunned animation
        yield return new WaitForSeconds(stunDuration); // Wait for the stun duration
        stunned = false; // Remove the stunned state
    }
}