using System.Collections;
using UnityEngine;

public class HellfireAxe : MonoBehaviour
{
    // Checks to see if axe is already thrown
    bool unarmed = false;
    
    // Reference to the axe prefab    
    public GameObject axePrefab;

    // Velocity of thrown axe
    public float axeVelocity;

    // Reference to the target
    public Transform target;

    // Reference to collider
    private Collider2D characterCollider;
    private Collider2D axeCollider;

    // Animator reference
    Animator animator;

    private Coroutine axeThrowingCoroutine; // Reference to the firing coroutine

    private void Awake()
    {
        // Cache this enemy's collider and animator
        characterCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }
}