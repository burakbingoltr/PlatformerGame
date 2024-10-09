using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7.5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float jumpTime = 0.5f;

    [Header("TurnCheck")]
    [SerializeField] private GameObject rightLeg;
    [SerializeField] private GameObject leftLeg;
    [HideInInspector] private bool isFacingRight;

    [Header("Gorund Check")]
    [SerializeField] private float extraHeight = 0.25f;
    [SerializeField] private LayerMask whatIsGround;

    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;
    private float moveInput;

    private bool isJumping;
    private bool isFalling;
    private float jumpTimeCounter;
    private RaycastHit2D gorundHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        startDirectionCheck();
    }

    private void Update()
    {
        Move();
        Jump();
    }


    #region Movement Functions
    private void Move()
    {
        moveInput = UserInput.instance.moveInput.x;
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput > 0 || moveInput < 0)
        {
            anim.SetBool("isWalking", true);
            turnCheck();
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    #endregion
    #region Turn Check Functions
    private void startDirectionCheck()
    {
        if (rightLeg.transform.position.x > leftLeg.transform.position.x)
        {
            isFacingRight = true;
        }
        else
        {
            isFacingRight = false;
        }
    }

    private void turnCheck()
    {
        if (UserInput.instance.moveInput.x > 0 && !isFacingRight)
        {
            Turn();
        }
        else if (UserInput.instance.moveInput.x < 0 && isFacingRight)
        {
            Turn();
        }
    }

    private void Turn()
    {
        if (isFacingRight)
        {
            Vector3 rotator = new Vector3(transform.position.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
        else
        {
            Vector3 rotator = new Vector3(transform.position.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
    }
    #endregion
    #region Jump Functions
    private void Jump()
    {
        //Button JUST PRESSED
        if (UserInput.instance.controls.Jumping.Jump.WasPressedThisFrame() && isGorunded())
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            anim.SetTrigger("jump");
        }
        //Button BEING HELD
        if (UserInput.instance.controls.Jumping.Jump.IsPressed())
        {
            if (jumpTimeCounter > 0 && isJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else if (jumpTimeCounter == 0)
            {
                isFalling = true;
                isJumping = false;
            }
            else
            {
                isJumping = false;
            }

        }
        // Buton was RELEASED THİS FRAME
        if (UserInput.instance.controls.Jumping.Jump.WasReleasedThisFrame())
        {
            isFalling = true;
            isJumping = false;
        }
        if (!isJumping && CheckForLand())
        {
            anim.SetTrigger("land");
        }


    }
    #endregion
    #region Ground Check - Raycast - Landed Check
    private bool isGorunded()
    {
        gorundHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, extraHeight, whatIsGround);
        if (gorundHit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckForLand()
    {
        if (isFalling)
        {
            if (isGorunded())
            {
                //PLAYER HAS LANDED
                isFalling = false;

                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    #endregion

}
