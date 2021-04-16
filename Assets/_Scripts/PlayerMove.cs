using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum LeftOrRight { Left, Right, none }
public delegate void animcontroll(float Value);

public class PlayerMove : MonoBehaviour
{
    public static event animcontroll Movementcontroll;
    public static event animcontroll Jump;
    public static event animcontroll fall;
    public static event animcontroll Ground;
    // public static event animcontroll Movementcontroll3;
    // public static event animcontroll Movementcontroll4;

    [SerializeField]
    ForceMode2D force1;
    [SerializeField]
    private Transform groundCheckPos;
    [SerializeField]
    private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask m_WhatIsGround;
    MoveInput moveDir;
    [SerializeField]
    private float speed = 2;
    [SerializeField]
    private float jumpspeed = 2;
    Vector2 m_velocity = Vector2.zero;
    [SerializeField]
    private float smothTime = 0.05f;
    Rigidbody2D rb;
    LeftOrRight lr;
    [SerializeField]
    Transform LeftWall;
    [SerializeField]
        Transform RightWall;
    SpriteRenderer spriteRend;

    bool nearWall = false;
    bool grounded = false;

    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        PlayerInputCatcher.onPlayerInput += OnMove;
        PlayerInputCatcher.onPlayerMoveDir += OnWhatDir;
        PlayerInputCatcher.OnJump += OnJump;
        RayCastForPhysics.stayOnGround += OnGround;
    }

    Collider2D[] HitColliders;
    Collider2D[] HitWallCollidersLeft;
    Collider2D[] HitWallCollidersRight;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) OnAction();
        
       
    }
    private void FixedUpdate()
    {
      
        Jump(rb.velocity.y);    
        lr = LeftOrRight.none;
       grounded = false;
        nearWall = false;
        // Collider2D[] HitColliders =  Physics2D.OverlapCircleAll(groundCheckPos.position, groundCheckRadius, m_WhatIsGround);
        // foreach (var hitCollider in HitColliders)
        // {
        //     Debug.Log(hitCollider);
        // }
        HitColliders = Physics2D.OverlapBoxAll(groundCheckPos.position, new Vector2(0.7f,0.2f),0, m_WhatIsGround);
        HitWallCollidersLeft = Physics2D.OverlapBoxAll(LeftWall.position, new Vector2(0.1f, 1), 0, m_WhatIsGround);
        HitWallCollidersRight = Physics2D.OverlapBoxAll(RightWall.position, new Vector2(0.1f, 1), 0, m_WhatIsGround);
      

        foreach (var hitCollider in HitColliders)
        {
            grounded = true;
            Ground(0f);
        }

        foreach (var hitCollider in HitWallCollidersLeft)
        {
            lr = LeftOrRight.Left;
            nearWall = true;
        }
        foreach (var hitCollider in HitWallCollidersRight)
        {
            lr = LeftOrRight.Right;

            nearWall = true;
        }
        

    }
    float yVelocityNull = 0;
    bool whatNow = true;
    Collider2D saveCollide;

    Collider2D[] CircleColliders;
    [SerializeField] private LayerMask whatIsButton;
    void OnAction() {
        
        foreach (var hitCollider in HitWallCollidersLeft)
        {
            if (hitCollider.tag == "box") hitCollider.GetComponent<IOnItemAction>().OnAction(whatNow);

        }
        foreach (var hitCollider in HitWallCollidersRight)
        {
            if (hitCollider.tag == "box") hitCollider.GetComponent<IOnItemAction>().OnAction(whatNow);
        }
        if (whatNow == true) whatNow = false; else whatNow = true;


        CircleColliders = Physics2D.OverlapCircleAll(transform.position, 3f, whatIsButton);
        foreach (var CColider in CircleColliders)
        {
            CColider.GetComponent<IOnButtonUse>().OnAction();
        }
    }
    public static Vector2 VectorGetter;


    void OnMove(float moveVelocity)
    {
        if (moveVelocity > 0) spriteRend.flipX = false; else if (moveVelocity < 0) spriteRend.flipX = true;

                Vector2 targetVelocity= new Vector2(moveVelocity * speed + WallJumpVelocity, rb.velocity.y + yVelocityNull);
        VectorGetter = targetVelocity;
        yVelocityNull = 0;
      //  if (WallJumpVelocity < 0) WallJumpVelocity = WallJumpVelocity + 1;
     rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity , ref m_velocity, smothTime);
        Movementcontroll(moveVelocity);
    }

    void OnWhatDir(MoveInput direction)
    {
       
        moveDir = direction;
    }

    float WallJumpVelocity;
    float VelCurr = 0;
    [SerializeField]
    float VsmothTime = 2f;
    [SerializeField]
    float AnotherJumpForce = 10;
    void OnJump()
    {
        Debug.Log("main log    " + grounded);
        if (grounded)
            rb.AddForce(Vector2.up * jumpspeed);
        else
        if (nearWall == true)
        {
            rb.velocity = new Vector2(0, 0);

            StopCoroutine(wallJumpVelocity());
            StartCoroutine(wallJumpVelocity());
            rb.AddForce(new Vector2(0, 1f) * AnotherJumpForce, force1);
        }
    }

    float JumpVelocity = 0;

    [SerializeField]
    float JumpForce = 0.4f;

    bool isJumped = false;

   IEnumerator wallJumpVelocity()
    { if (lr == LeftOrRight.Right)
        {
            WallJumpVelocity = -10;
            do
            {
              //  Debug.Log(grounded);
                if (WallJumpVelocity < -2)
                {
                    PlayerInputCatcher.WallJump = MoveInput.right;
                    WallJumpVelocity += 0.25f;
                }
                else
                {
                    PlayerInputCatcher.WallJump = MoveInput.none;
                    WallJumpVelocity += 0.01f;
                    if (nearWall == true) break;
                }
                yield return new WaitForFixedUpdate();
                if (grounded) { Debug.Log(grounded); break;
               }
            }
            while (WallJumpVelocity < 0);
            WallJumpVelocity = 0;
            lr = LeftOrRight.none;
            PlayerInputCatcher.WallJump = MoveInput.none;

        }

        if (lr == LeftOrRight.Left)
        {
            WallJumpVelocity = 10;
            do
            {
               // Debug.Log(grounded);
                if (WallJumpVelocity > 2)
                {
                    PlayerInputCatcher.WallJump = MoveInput.left;
                    WallJumpVelocity -= 0.25f;
                }
                else
                {
                    PlayerInputCatcher.WallJump = MoveInput.none;
                    WallJumpVelocity -= 0.01f;
                    if (nearWall == true) break;
                }
                yield return new WaitForFixedUpdate();
                if (grounded) { Debug.Log(grounded); break; }
            }
            while (WallJumpVelocity > 0);
            WallJumpVelocity = 0;
            lr = LeftOrRight.none;
            PlayerInputCatcher.WallJump = MoveInput.none;

        }


    }



    void OnGround(RaycastHit2D hit2D)
    {
        //Debug.Log(hit2D.collider);
        //
        //isJumped = false;
       // JumpVelocity = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {//if (collision.collider.tag == "Ground")
     //  {
     //      Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 1.7f), new Vector2(0, 1));
     //      RaycastHit2D hit2D1 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1.7f), new Vector2(0, 1), 0.001f);
     //      RaycastHit2D hit2D2 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 1.35f), new Vector2(0, -1), 0.001f);
     //      if (hit2D2.collider != null)
     //      {
     //          JumpVelocity = 0;
     //          isJumped = false;
     //      }
     //      if (hit2D1.collider != null)
     //      {
     //          JumpVelocity = 0;
     //
     //      }
     //  }
     //  }
    }
}
