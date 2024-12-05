using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Axe : MonoBehaviour
{
    // Axe prefab assigned in inspector
    public GameObject axePrefab;
    // Is the player currently attacking
    bool isAttacking;
    // Checks to see if axe is already thrown
    bool unarmed = false;

    // Animator reference
    [HideInInspector] public Animator animator;
    // reference to the camera
    Camera localCamera;
    float positiveSlope, negativeSlope;

    public float velocity;

    // Enum to describe the direction the player is firing in
    enum Quadrant { East, South, West, North }
    enum SlopeLine { Positive, Negative }

    // Amount of damage the axe will inflict on an enemy
    public int damageInflicted;

    private void Start()
    {
        animator = GetComponent<Animator>();
        isAttacking = false;
        localCamera = Camera.main;

        // Create four Vectors to represent the four corners of the screen
        Vector2 lowerLeft = localCamera.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 upperRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 upperLeft = localCamera.ScreenToWorldPoint(new Vector2(0, Screen.height));
        Vector2 lowerRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0));

        // Calculate slopes of two lines to disect the screen into four quadrants
        // This is to help determine what direction the player should face when the mouse is clicked
        positiveSlope = GetSlope(lowerLeft, upperRight);
        negativeSlope = GetSlope(upperLeft, lowerRight);
    }

    // Calculate the slope of a line, given two (x,y) points
    float GetSlope(Vector2 pointOne, Vector2 pointTwo)
    {
        return (pointTwo.y - pointOne.y) / (pointTwo.x - pointOne.x);
    }

    // Called each frame
    private void Update()
    {
        // Check to see if user has clicked the mouse to fire the slingshot
        if (!unarmed && Input.GetMouseButtonDown(0)) // Parameter 0 checks for left mouse button; 1 checks for right
        {
            ThrowAxe();
        }
    }

    private void ThrowAxe()
    {
        // Convert the mouse position from screen space to world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Get a new ammo object located at the weapon's current position
        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotation = Quaternion.identity; // No rotation
        Instantiate(axePrefab, spawnPosition, spawnRotation);
        axePrefab.SetActive(true);

        // Get reference to the arc script
        Arc arcScript = axePrefab.GetComponent<Arc>();

        // Calculate the amount of time for ammo travel
            // Example: if velocity is 2, then 1/2 = 0.5 or a half second to travel across the screen
        float travelDuration = 1.0f / velocity;

        StartCoroutine(arcScript.TravelArc(mousePosition, travelDuration));
    }

    private void TriggerAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            unarmed = true;

            Quadrant attackDirection  = GetQuadrant();
            var quadrantVector = attackDirection  switch
            {
                Quadrant.East => new Vector2(1.0f, 0.0f),
                Quadrant.South => new Vector2(0.0f, -1.0f),
                Quadrant.West => new Vector2(-1.0f, 0.0f),
                Quadrant.North => new Vector2(0.0f, 1.0f),
                _ => new Vector2(0.0f, 0.0f),
            };

            Debug.Log($"Attacking in direction: {attackDirection} ({quadrantVector.x}, {quadrantVector.y})");

            // Pass the attack state and direction to the animator
            animator.SetBool("isAttacking", true);
            animator.SetFloat("AttackXDir", quadrantVector.x);
            animator.SetFloat("AttackYDir", quadrantVector.y);

            Invoke(nameof(ResetAttack), 0.4f); // Adjust delay as needed
        }
    }

    private void ResetAttack()
    {
        Debug.Log("Resetting attack");
        isAttacking = false;
        animator.SetBool("isAttacking", false);
        unarmed = false;
    }

    // Determines whether the input position is above a given sloped line
    bool AboveSlopeLine(SlopeLine compare, Vector2 inputPosition)
    {
        Vector2 playerPosition = transform.position;
        Vector2 mousePosition = localCamera.ScreenToWorldPoint(inputPosition);

        float slopeToCompare = compare == SlopeLine.Positive ? positiveSlope : negativeSlope;
        float yIntercept = playerPosition.y - (slopeToCompare * playerPosition.x);
        float inputIntercept = mousePosition.y - (slopeToCompare * mousePosition.x);

        return inputIntercept > yIntercept;
    }

    // Determines the quadrant (north, south, east, or west) of the mouse click relative to the player
    Quadrant GetQuadrant()
    {
        bool abovePositiveSlope = AboveSlopeLine(SlopeLine.Positive, Input.mousePosition);
        bool aboveNegativeSlope = AboveSlopeLine(SlopeLine.Negative, Input.mousePosition);

        if (abovePositiveSlope) return aboveNegativeSlope ? Quadrant.North : Quadrant.West;
        else return aboveNegativeSlope ? Quadrant.East : Quadrant.South;
    }

    // Called when another object enters the trigger collider attached to the ammo gameobject
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision is BoxCollider2D)
        {
            // Check that we have hit the box collider inside the enemy, and not it's circle collider
            if (collision.gameObject.CompareTag("Enemy"))
            {
                // Retrieve the player script from the enemy object
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();

                // Start the damage coroutine; 0.0f will inflict a one-time damage
                StartCoroutine(enemy.DamageCharacter(damageInflicted, 0.0f));
            }
        }
    }
}