using UnityEngine;

public class MovementController : MonoBehaviour
{
    public bool isFrozen = false;
    public float movementSpeed;

    // holds 2D points; used to represent a character's location in 2D space, or where it's moving to
    Vector2 movement = new();

    // holds reference to the animator component in the game object
    Animator animator;
    
    Rigidbody2D rb2D;

    // Start is called before the first frame update
    void Start()
    {
        // get references to game object component so it doesn't have to be grabbed each time we need it
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // update the animation state machine
        UpdateState();
    }

    private void UpdateState()
    {
        // Check to see if the movement vector is approximately equal to (0, 0) -- i.e. player is still standing still
        if (Mathf.Approximately(movement.x, 0) && Mathf.Approximately(movement.y, 0))
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }

        // Update teh animator with the new movement values
        animator.SetFloat("xDir", movement.x);
        animator.SetFloat("yDir", movement.y);


    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        if (!isFrozen)
        {
            // get user input
            // GetAxisRaw param allows us to specify which axis we're interested in
            // Returns 1 = right key or "d" (up key or "w")
            //        -1 = left key or "a" (down key or "s")
            //         0 = no key pressed
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            // keeps player moving at the same rate of speed, no matter which direction they are moving in
            movement.Normalize();

            rb2D.velocity = movement * movementSpeed;
        }
        else
        {
            rb2D.velocity = Vector2.zero;
        }
    }
}
