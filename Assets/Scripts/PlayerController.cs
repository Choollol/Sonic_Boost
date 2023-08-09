using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;

    //public GameManager gameManager;
    //public AudioManager audioManager;

    private Rigidbody2D rb;

    public bool isFrozen;
    public bool isJumping;

    public LayerMask groundLayer;

    private Vector3 velocity;
    private GameObject groundCheck;

    private float horizontalInput;

    public bool isGrounded;
    private float coyoteTime = 0.1f;
    private float coyoteTimeCounter = 0;
    private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter = 0;
    private int extraJumps = 0;
    private int extraJumpCounter = 0;

    private float jumpTimer = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        groundCheck = transform.GetChild(0).gameObject;
    }

    private void FixedUpdate()
    {
        if (!isFrozen && GameManager.isGameActive)
        {
            MovementUpdate();
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Physics2D.Raycast(groundCheck.transform.position, Vector2.down, 0.02f, groundLayer) && jumpTimer > 0.1f)
        {
            isGrounded = true;
            isJumping = false;
            extraJumpCounter = 0;
        }
        if (!isFrozen && GameManager.isGameActive)
        {
            JumpUpdate();
        }
        if (rb.velocity.y > jumpForce)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
    private void MovementUpdate()
    {
        velocity = new Vector3(horizontalInput, 0) * speed;

        //rb.velocity = new Vector3(velocity.x, rb.velocity.y);
        transform.position += new Vector3(velocity.x / 50, 0);
        //rb.AddForce(new Vector2(velocity.x, 0));
    }
    private void JumpUpdate()
    {
        float dt = Time.deltaTime;

        if (isGrounded || extraJumpCounter < extraJumps)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= dt;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= dt;
        }

        if (coyoteTimeCounter > 0 && jumpBufferCounter > 0)
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpBufferCounter = 0;
            if (!isGrounded)
            {
                extraJumpCounter++;
            }
            isGrounded = false;
            isJumping = true;
            jumpTimer = 0;
            AudioManager.PlaySound("Jump Sound");
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

            coyoteTimeCounter = 0;
        }
        jumpTimer += dt;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == groundLayer)
        {
            isGrounded = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boost"))
        {
            SonicBoost boost = collision.GetComponent<SonicBoost>();
            if (boost.canUse)
            {
                isGrounded = true;

                if (rb.velocity.y < 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                }
                rb.AddForce(boost.force * jumpForce + new Vector2(0, 2), ForceMode2D.Impulse);

                AudioManager.PlaySound("Boost Sound");

                boost.canUse = false;
            }
        }
    }
}
