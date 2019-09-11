﻿using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    /* 
     * --- C# TIP ---
     * Use SerializeField to expose private variables
     * Private variables are not accessible through other scripts but will display in the Inspector
    */

    // Member Variables
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = true;                         // Whether or not a player can steer while jumping
    [SerializeField] private bool m_StickToSlopes = true;                         // Whether or not a player can stick to slopes
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private LayerMask m_WhatIsLadder;                          // A mask determining what is ladder to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_LadderCheck;                           // A position marking where the starting point of ladder ray is.
    [SerializeField] private float m_LadderRadius = .05f;                      // Radius of the overlap circle to determine if grounded 
    [SerializeField] private float m_GroundedRadius = .05f;                      // Radius of the overlap circle to determine if grounded 
    [SerializeField] private float m_GroundRayLength = .2f;                     // Length of the ray beneath controller
    [SerializeField] private float m_LadderRayLength = .5f;                     // Length of the ray above controller


    private float m_OriginalGravityScale;

    [Header("Events")]
    public UnityEvent OnLandEvent;

    // Public Getters / Setters (Parameters)
    public bool IsGrounded { get; private set; }
    public bool IsFacingRight = true;
    public float JumpAngle;

    public bool IsDead { get; private set; } = false;
    public Rigidbody2D Rigidbody { get; private set; }
    public Animator Anim { get; private set; }

    public bool HasParameter(string paramName, Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }


    // Internal Methods
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        m_OriginalGravityScale = Rigidbody.gravityScale;
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }
    private void OnDrawGizmos()
    {
        // Ground gizmos
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(m_GroundCheck.position, m_GroundedRadius);
        Ray groundRay = new Ray(transform.position, Vector3.down);
        Gizmos.DrawLine(groundRay.origin, groundRay.origin + groundRay.direction * m_GroundRayLength);

        // Ladder Gizmos
        Gizmos.color = Color.red;
        Ray ladderRay = new Ray(m_LadderCheck.position, Vector3.up);
        Gizmos.DrawLine(ladderRay.origin, ladderRay.origin + ladderRay.direction * m_LadderRayLength);
    }


    private void FixedUpdate()
    {

        bool wasGrounded = IsGrounded;
        IsGrounded = false;
        Anim.SetBool("IsGrounded", false);
        Anim.SetBool("IsRunning", false);
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, m_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                IsGrounded = true;


                DoubleJump = true;
                Anim.SetBool("IsGrounded", true);
                Anim.SetBool("IsJumping", false);


            }
        }

        // Need to make one for ladders.
    }


    public void Attack()
    {
        Anim.SetTrigger("Attack");
        if (IsFacingRight) // ITS ACTUALLY FACING LEFT
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 2f, ~WhatIsMe);
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<EnemyDeath>() != null)
                {
                    hit.collider.SendMessage("OnDeath");
                }

                else if (hit.collider.GetComponent<Bullet>() != null)
                {
                    hit.collider.SendMessage("Punched");
                }
            }
        }

        else if(!IsFacingRight) // ITS ACTUALLY FACING RIGHT
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 2f, ~WhatIsMe);
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<EnemyDeath>() != null)
                {
                    hit.collider.SendMessage("OnDeath");
                }

                else if (hit.collider.GetComponent<Bullet>() != null)
                {
                    hit.collider.SendMessage("Punched");
                }
            }
        }
        

    }


    public void Flip()
    {
        // Switch the way the player is labelled as facing.
        IsFacingRight = !IsFacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // >> Custom methods go here <<

    public void Climb(float offsetY)
    {
        if (HasParameter("ClimbSpeed", Anim))
            Anim.SetFloat("ClimbSpeed", offsetY);

        //RaycastHit2D ladderHit = Physics2D.Raycast(m_LadderCheck.position, Vector2.up, m_LadderRayLength, m_WhatIsLadder);
        if (IsFrontBlocked || IsTopBlocked)
        {
            if (offsetY != 0)
            {
                IsClimbing = true;
                Anim.SetBool("IsClimbing", true);

            }
        }
        


        else
        {
            IsClimbing = false;
            Anim.SetBool("IsClimbing", false);

        }

        if (IsClimbing)
        {
            Rigidbody.gravityScale = 0;
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, offsetY);
        }
        else
        {
            Rigidbody.gravityScale = m_OriginalGravityScale;
        }
    }
    public void Jump(float height)
    {

        // If the player should jump...
        if (IsGrounded)
        {
            // Add a vertical force to the player.
            IsGrounded = false;
            Rigidbody.AddForce(new Vector2(0f, height), ForceMode2D.Impulse);
            Anim.SetBool("IsJumping", true);


        }
        else if (DoubleJump && !IsGrounded)
        {
            DoubleJump = false;
            Rigidbody.AddForce(new Vector2(0f, height), ForceMode2D.Impulse);

            Anim.SetTrigger("DoubleJump");


        }
        if (IsFrontBlocked)
        {
            Rigidbody.gravityScale = 0;


            if (IsFacingRight)
            {
                Rigidbody.AddForce(new Vector2(0f, height), ForceMode2D.Impulse);
                Rigidbody.AddForce(new Vector2(-height, 0f), ForceMode2D.Impulse);
            }
            if (!IsFacingRight)
            {
                Rigidbody.AddForce(new Vector2(0f, height), ForceMode2D.Impulse);
                Rigidbody.AddForce(new Vector2(height, 0f), ForceMode2D.Impulse);
            }
            Anim.SetBool("IsJumping", true);
        }
    }
    // Make a function for climbing ladders.
    
    // Move must be called last!
    public void Move(float offsetX)
    {
        if (offsetX != 0)
        {
            Anim.SetBool("IsRunning", true);
        }

        //only control the player if grounded or airControl is turned on
        if (IsGrounded || m_AirControl)
        {
            if (m_StickToSlopes)
            {
                Ray groundRay = new Ray(transform.position, Vector3.down);
                RaycastHit2D groundHit = Physics2D.Raycast(groundRay.origin, groundRay.direction, m_GroundRayLength, m_WhatIsGround);
                if (groundHit.collider != null)
                {
                    Vector3 slopeDirection = Vector3.Cross(Vector3.up, Vector3.Cross(Vector3.up, groundHit.normal));
                    float slope = Vector3.Dot(Vector3.right * offsetX, slopeDirection);

                    offsetX += offsetX * slope;

                    float angle = Vector2.Angle(Vector3.up, groundHit.normal);
                    if (angle > 0)
                    {
                        Rigidbody.gravityScale = 0;
                        Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0f);
                    }
                }
            }

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(offsetX, Rigidbody.velocity.y);

            Vector3 velocity = Vector3.zero;
            // And then smoothing it out and applying it to the character
            Rigidbody.velocity = Vector3.SmoothDamp(Rigidbody.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (offsetX > 0 && IsFacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (offsetX < 0 && !IsFacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }

    }

    public void Death()
    {
        Anim.SetTrigger("Dying");
        IsDead = true;
        UIManager.Instance.ActivateRespawnButton();
        GameManager.Instance.GameOver();
        // UIManager - Activate red border
        // Place Redborder sprite as a child into the Respawn button.        
        IsHurt = true;

        SoundManager.Instance.PlaySound("PlayerDeath");
    }

    // CTRL + M + O = Folds Code
    // CTRL + M + P = UnFolds Code
}