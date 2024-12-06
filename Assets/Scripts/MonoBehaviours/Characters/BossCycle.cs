using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCycle : MonoBehaviour
{
    // Time between attacks
    int attackTime = 0;
    CircleCollider2D circleCollider;

    // Speed at which the enemy pursues the player
    public float pursuitSpeed;

    // General wandering speed
    public float wanderSpeed;

    // How often the enemy should change wandering directions
    public float directionChangeInterval;

    // Current speed, which is one of the previous two speeds
    protected float currentSpeed;

    // Player chasing behaviour (can turn off if characters other than enemies are created)
    public bool followPlayer;

    // Reference to the currently running movement coroutine
    protected Coroutine moveCoroutine;

    // Components attached to the game object
    protected Rigidbody2D rb2d;
    protected Animator animator;

    // The player's transform (position)
    protected Transform targetTransform = null;

    // Destination where the enemy is wandering
    protected Vector3 endPosition;

    // Angle is used to generate a vector which becomes the destination
    protected float currentAngle = 0;

    protected readonly string animationState = "AnimationState";

    // enumerated constants to correspond to the values assigned to the animations
    protected enum CharStates
    {
        walkEast = 1,
        walkSouth = 2,
        walkWest = 3,
        walkNorth = 4,
        idleSouth = 5,
        attack = 6
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        currentSpeed = wanderSpeed;
        StartCoroutine(WanderRoutine());
    }

    protected void OnDrawGizmos()
    {
        if (circleCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        Debug.DrawLine(rb2d.position, endPosition, Color.red);
    }

    protected void FixedUpdate()
    {
        if (attackTime < 350)
        {
            attackTime++;
        }
        else
        {
            attackTime = 0;
        }
    }

    public IEnumerator WanderRoutine()
    {
        // Enemy should wander indefinitely
        while(true)
        {
            // Choose a new endpoint for the enemy to move toward
            ChooseNewEndpoint();

            // If enemy is already moving, stop it before moving in a new direciton
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            // Start the new move routine
            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));

            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    protected void ChooseNewEndpoint()
    {
        // Choose a random value between 0 and 360 to represent a new direciton to travel toward
        currentAngle += UnityEngine.Random.Range(0, 360);

        // Effectively perform a mod operation so that currentAngle is always between 0 and 360
        currentAngle = Mathf.Repeat(currentAngle, 360);

        // Convert Angle to a Vector 3 and add result to endPosition
        endPosition += Vector3FromAngle(currentAngle);
    }

    // Takes an angle in degrees, converts it to radians, and return a directional vector
    protected Vector3 Vector3FromAngle(float inputAngleDegrees)
    {
        // Convert angle degrees to radians
        float inputAngleRadians = inputAngleDegrees * Mathf.Deg2Rad;

        // Create a normalized directional vector for the enemy direciton
        return new Vector3(Mathf.Cos(inputAngleRadians), Mathf.Sin(inputAngleRadians), 0);
    }


    protected virtual IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        // Retrieve the rough distance remaining between teh current enemy position and the destination
        // Magnitude is a unity function to return the length of the vector
        float remainingDistance = (transform.position - endPosition).sqrMagnitude;

        while (remainingDistance > float.Epsilon)
        {
            if (targetTransform != null)
            {
                // If targetTransform is set, calculate the direction away from the player
                Vector3 directionAwayFromPlayer = transform.position - targetTransform.position;
                
                // Optionally, you can multiply by a distance factor to control how far away you want the object to move
                float distanceAway = 5f; // Example distance, adjust as needed
                endPosition = transform.position + directionAwayFromPlayer.normalized * distanceAway;
            }
            
            if (rigidBodyToMove != null && attackTime < 200)
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
                remainingDistance = (transform.position - endPosition).sqrMagnitude;
            }
            else if (attackTime > 200)
            {
                //Attack on time interval
                animator.SetInteger(animationState, (int)CharStates.attack);
            }

            // Pause execution until the next Fixed Frame Update
            yield return new WaitForFixedUpdate();
        }

        // Stop walking / idle
        animator.SetBool("isWalking", false);
        animator.SetInteger(animationState, (int)CharStates.idleSouth);
    }

    // Called when player enters the circle collider for the enemy
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // See if the object that the enemy has collided with is the cplayer and
        // that the enemy is supposed to be following the player
        if (collision.gameObject.CompareTag("Player") && followPlayer)
        {
            // Update the current speed
            currentSpeed = pursuitSpeed;

            // Set the targetTransform to be the player's
            targetTransform = collision.gameObject.transform;
        }
        
        // if enemy is moving, stop it
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        // Start the move routine with the updated information
        // i.e. to follow the player at the new speed
        moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
    }

    // Called when player exits the circle collider for the enemy
    // Can only happen if player can move faster than the enemy
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        // See if the object that the enemy is no longer colliding with is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Enemy is confused and pauses for a miunute after losing sight of the player
            animator.SetBool("isWalking", false);

            // Slow the speed down
            currentSpeed = wanderSpeed;

            // If enemy is moving, stop it
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            // Set to null, since enemy is no longer following player
            targetTransform = null;
        }
    }
}
