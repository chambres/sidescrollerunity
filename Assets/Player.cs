using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speedofplatforms = -3f;



    [Header("Horizontal Movement")]
    public float moveSpeed = 10f;

    public Vector2 direction;

    private bool facingRight = true;

    [Header("Vertical Movement")]
    public float jumpSpeed = 15f;

    public float jumpDelay = 0.25f;

    private float jumpTimer;

    [Header("Components")]
    public Rigidbody2D rb;

    public Animator animator;

    public LayerMask groundLayer;

    [Header("Physics")]
    public float maxSpeed = 7f;

    public float linearDrag = 4f;

    public float gravity = 1f;

    public float fallMultiplier = 5f;

    [Header("Collision")]
    public bool onGround = false;

    public float groundLength = 0.6f;

    public Vector3 colliderOffset;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("entered");

        //transform.SetParent(other.gameObject.transform);
    
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        Debug.Log("exited");
        this.transform.parent = null;
    }

    void Start()
    {

        animator.SetBool("Falling", true);

        speedofplatforms = -20f;

    }





    void breakRestraints()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public GameObject cam;

    public bool first = true;


    float timer;


    bool timed = false;

    bool allowbegin = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "death")
        {

            timed = false;

        }
    }

    bool restarting = false;

       

    void Update()
    {
        if (timed)
        {
            timer += Time.deltaTime;
        }

                
        Debug.Log(Physics2D
                .Raycast(transform.position - colliderOffset,
                Vector2.down,
                groundLength,
                groundLayer));
                
        bool wasOnGround = onGround;

        onGround =
            Physics2D
                .Raycast(transform.position + colliderOffset,
                Vector2.down,
                groundLength,
                groundLayer) ||
            Physics2D
                .Raycast(transform.position - colliderOffset,
                Vector2.down,
                groundLength,
                groundLayer);
        
        

        if (Input.GetButtonDown("Jump"))
        {
            jumpTimer = Time.time + jumpDelay;
        }

        if (Input.GetKeyDown(KeyCode.D))
        { 
        }

        if (Input.GetKeyDown(KeyCode.R) && !timed)
        {
            restarting = true;
        }


        if (Input.GetKeyUp(KeyCode.W))
        {
            Time.timeScale = 1f;

        }

        animator.SetBool("Grounded", onGround);
        direction =
            new Vector2(Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical"));

        if (onGround)
        {
            animator.SetBool("Jumping", false);
        }
    }

    void FixedUpdate()
    {
        moveCharacter(direction.x);
        if (jumpTimer > Time.time && onGround)
        {
            animator.SetBool("Jumping", true);
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            jumpTimer = 0;
        }

        animator.SetBool("Falling", rb.velocity.y < .1f);

        if (onGround && animator.GetBool("Falling"))
        {
            animator.SetBool("Falling", false);
        }

        // if(Mathf.Abs(rb.velocity.x) < .1f){
        //     rb.velocity = new Vector2(0, rb.velocity.y);
        //     animator.SetBool("Run", false);
        // }
        if (direction.x == 0 && onGround)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetBool("Run", false);
        }

        modifyPhysics();
    }

    

    void moveCharacter(float horizontal)
    {
        rb.AddForce(Vector2.right * horizontal * moveSpeed);

        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            //flip
            facingRight = !facingRight;
            transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
        }

        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity =
                new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed,
                    rb.velocity.y);
        }

        animator.SetBool("Run", Mathf.Abs(rb.velocity.x) >= .1f);
        //animator.SetFloat("Jumping",rb.velocity.y >= .1f);
    }

    void modifyPhysics()
    {
        bool changingDirections =
            (direction.x > 0 && rb.velocity.x < 0) ||
            (direction.x < 0 && rb.velocity.x > 0);

        if (onGround)
        {
            if (Mathf.Abs(direction.x) < 0.1f || changingDirections)
            {
                rb.drag = linearDrag;
            }
            else
            {
                rb.drag = 0f;
            }
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallMultiplier;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos
            .DrawLine(transform.position + colliderOffset,
            transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos
            .DrawLine(transform.position - colliderOffset,
            transform.position - colliderOffset + Vector3.down * groundLength);
    }
}
