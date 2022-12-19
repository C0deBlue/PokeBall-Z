using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float speedChange = 3.0f;
    public float moveSpeed = 5f;

    [Tooltip("The amount the animation speeds up when running")]
    public float animationSpeedMod = 1.0f;

    bool walking = true;

    public Rigidbody2D rb;
    public Animator animator;
    public TextMeshProUGUI walkRunText;

    Vector2 movement;

    bool disabled;

    public static Transform playerTransform;

    void OnEnable()
    {
        playerTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetFloat("AnimationMod", 1.0f + ((moveSpeed - walkSpeed) / (runSpeed - walkSpeed)) * animationSpeedMod);

        moveSpeed = Mathf.Clamp(moveSpeed + (walking ? -speedChange * Time.deltaTime : speedChange * Time.deltaTime), walkSpeed, runSpeed);

    }

    void FixedUpdate()
    {
        //Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
    
    public void SpeedButtonToggle()
    {
        walking = !walking;
        walkRunText.text = walking ? "Walk" : "Run";
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("TriggerEnter");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("CollisionEnter");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("CollisionExit");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("TriggerExit");
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("CollisionStay");
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("TriggerStay");
    }
}
