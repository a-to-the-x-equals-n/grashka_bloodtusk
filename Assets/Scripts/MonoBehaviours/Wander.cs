using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ensure that the game object this is attached to has these components
// If it doesn't, they will be automatically addded
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CircleCollider2D))]
public class Wander : MonoBehaviour
{
    CircleCollider2D circleCollider;

    // Speed at which the enemy pursues the player
    public float pursuitSpeed;

    // General wandering speed
    public float wanderSpeed;

    // How often the enemy should change wandering directions
    public float directionChangeInterval;

    // Current speed, which is one of the previous two speeds
    float currentSpeed;

    // Player chasing behaviour (can turn off if characters other than enemies are created)
    public bool followPlayer;

    // Reference to the currently running movement coroutine
    Coroutine moveCoroutine;

    // Components attached to the game object
    Rigidbody2D rb2d;
    Animator animator;

    // The player's transform (position)
    Transform targetTransform = null;

    // Destination where the enemy is wandering
    Vector3 endPosition;

    // Angle is used to generate a vector which becomes the destination
    float currentAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        currentSpeed = wanderSpeed;
        StartCoroutine(WanderRoutine());
    }

    private void OnDrawGizmos()
    {
        if (circleCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(rb2d.position, endPosition, Color.red);
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

    private void ChooseNewEndpoint()
    {
        // Choose a random value between 0 and 360 to represent a new direciton to travel toward
        currentAngle += UnityEngine.Random.Range(0, 360);

        // Effectively perform a mod operation so that currentAngle is always between 0 and 360
        currentAngle = Mathf.Repeat(currentAngle, 360);

        // Convert Angle to a Vector 3 and add result to endPosition
        endPosition += Vector3FromAngle(currentAngle);
    }

    // Takes an angle in degrees, converts it to radians, and return a directional vector
    private Vector3 Vector3FromAngle(float inputAngleDegrees)
    {
        // Convert angle degrees to radians
        float inputAngleRadians = inputAngleDegrees * Mathf.Deg2Rad;

        // Create a normalized directional vector for the enemy direciton
        return new Vector3(Mathf.Cos(inputAngleRadians), Mathf.Sin(inputAngleRadians), 0);
    }


    private IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        // Retrieve the rough distance remaining between teh current enemy position and the destination
        // Magnitude is a unity functioni to return the length of the vector
        float remainingDistance = (transform.position - endPosition).sqrMagnitude;

        while (remainingDistance > float.Epsilon)
        {

            if (targetTransform != null)
            {
                // If targeTransform is set, then it's postion is the player's position
                // This moves the enemy toward the player instead of toward the original endPosition
                endPosition = targetTransform.position;
            }
            
            if (rigidBodyToMove != null)
            {
                // Set anumation parameter so anumator will change the anumations that's played
                animator.SetBool("isWalking", true);

                // Calculates the movement for a RigidBody2D
                // To Make sure that object speed is independent of frame rate, multiply the speed by Time.deltaTime
                Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed * Time.deltaTime);

                // Move the RigidBody2D
                rb2d.MovePosition(newPosition);

                // Update the distance remaining
                remainingDistance = (transform.position - endPosition).sqrMagnitude;
            }

            // Pause execution until the next Fixed Frame Update
            yield return new WaitForFixedUpdate();
        }
        animator.SetBool("isWalking", false);
    }

    // Called when player enters the circle collider for the enemy
    private void OnTriggerEnter2D(Collider2D collision)
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
    private void OnTriggerExist2D(Collider2D collision)
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