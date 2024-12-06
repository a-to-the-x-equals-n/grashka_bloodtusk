using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Axe : MonoBehaviour
{
    // Singleton instance
    public static Axe Instance { get; private set; }

    [HideInInspector] public Animator animator;
    public GameObject axePrefab;
    private GameObject axeObject;
    private Rigidbody2D rb2d;
    private float positiveSlope, negativeSlope;
    private Camera localCamera;
    Coroutine damageCoroutine; // Reference to a running coroutine

    // Enums for directions
    private enum Quadrant { East, South, West, North }
    private enum SlopeLine { Positive, Negative }

    public float velocity;
    private bool isAttacking;
    private bool armed = true; // Ensures only one axe can be thrown

    // Called when the script is being loaded
    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        animator = GetComponent<Animator>();
        localCamera = Camera.main;

        // Instantiate the axe object
        axeObject = Instantiate(axePrefab);
        axeObject.SetActive(false);
    }

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

    // Called when the gameobject is destroyed
    private void OnDestroy()
    {
        
    }

    // Called each frame
    private void Update()
    {
        // Handle user input for throwing the axe
        if (Input.GetMouseButtonDown(0) && armed)
        {
            ThrowAxe();
        }
        UpdateState();
    }

    // Calculate the slope of a line, given two (x,y) points
    float GetSlope(Vector2 pointOne, Vector2 pointTwo)
    {
        return (pointTwo.y - pointOne.y) / (pointTwo.x - pointOne.x);
    }

    private void ThrowAxe()
    {
        armed = false;

        // Convert the mouse position from screen space to world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Enable and set position
        axeObject.SetActive(true);
        axeObject.transform.position = transform.position;

        // Start the travel arc
        if(axeObject != null)
        {
            // Calculate the amount of time for ammo travel
            // Example: if velocity is 2, then 1/2 = 0.5 or a half second to travel across the screen
            float travelDuration = 1.0f / velocity;
            StartCoroutine(AxeArcCycle(mousePosition, travelDuration));
        }
    }

    private IEnumerator AxeArcCycle(Vector3 destination, float duration)
    {
        // Get reference to the arc script
        AxeArc arcScript = axeObject.GetComponent<AxeArc>();

        // Execute the travel arc
        yield return StartCoroutine(arcScript.TravelArc(destination, duration));

        // Execute the return arc
        yield return StartCoroutine(arcScript.ReturnArc(transform.position, duration));

        // Deactivate the axe and re-arm
        axeObject.SetActive(false);
        armed = true;
    }

    private void TriggerAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            armed = false;

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
        armed = true;
    }

    private void UpdateState()
    {
        if (isAttacking)
        {
            Quadrant quadEnum = GetQuadrant();
            var quadrantVector = quadEnum switch
            {
                Quadrant.East => new Vector2(1.0f, 0.0f),
                Quadrant.South => new Vector2(0.0f, -1.0f),
                Quadrant.West => new Vector2(-1.0f, 1.0f),
                Quadrant.North => new Vector2(0.0f, 1.0f),
                _ => new Vector2(0.0f, 0.0f),
            };
            animator.SetBool("isAttacking", true);
            animator.SetFloat("fireXDir", quadrantVector.x);
            animator.SetFloat("fireYDir", quadrantVector.y);
            isAttacking = false;
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }
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
    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision is BoxCollider2D)
    //     {
    //         // Check that we have hit the box collider inside the enemy, and not it's circle collider
    //         if (collision.gameObject.CompareTag("Enemy"))
    //         {
    //             Debug.Log("Enemy hit!");

    //             // Retrieve the player script from the enemy object
    //             Enemy enemy = collision.gameObject.GetComponent<Enemy>();

    //             Debug.Log("Enemy script found, starting damage coroutine.");
    //             StartCoroutine(enemy.DamageCharacter(damageInflicted, 0.0f));
    //                 // damageCoroutine ??= StartCoroutine(enemy.DamageCharacter(damageInflicted, 0.0f));
    //         }
    //     }
    // }
}