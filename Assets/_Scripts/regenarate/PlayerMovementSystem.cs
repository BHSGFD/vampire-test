using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSystem : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float smothTime = 0.05f;

    Rigidbody2D rb;

    float playerVelocity;
    PlayerInputSystem inputSystem;

    //velocity
    float WallJumpVelocity = 0;
    Vector2 m_velocity = Vector2.zero;

    //jump
    bool grounded = false;
    float timer = 0;
    [SerializeField] float jumpSpeed = 350;
    float stabilizer = 0.19622f;
    bool isWallJUmpActive= false;
    bool isLeft = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //////////////////////////////////

        PlayerInputSystem.onMoveInput += OnPlayerMove;
        PlayerInputSystem.onJumpButtonPressed += OnJump;
        PlayerRaycasts.OnGrounded += OnGrounded;
        PlayerRaycasts.SetOnGrounded += SetOnGrounded;
        PlayerRaycasts.IsLeftWall += OnWallTouch;
    }


    private void FixedUpdate()
    {
        if (WallJumpVelocity < 0) WallJumpVelocity += 0.25f; else if (WallJumpVelocity > 0) WallJumpVelocity -= 0.25f;
        if (timer > 0) timer--;
        OnPlayerMove();
    }

    void OnPlayerMove()
    {
        Vector2 targetVelocity = new Vector2(playerVelocity * speed + WallJumpVelocity, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref m_velocity, smothTime);
     
    }

    void OnGrounded()
    {
        grounded = true;
    }

    void SetOnGrounded(bool parametr)
    {
        grounded = parametr;
    }

    void OnJump()
    {
      
      if (timer == 0)
      {
          if (grounded) {
              rb.AddForce(Vector2.up * jumpSpeed);
              timer = 20;
          }
          else if (isWallJUmpActive == true ) if(isLeft) { rb.AddForce(new Vector2(0,1) * jumpSpeed); WallJumpVelocity = 10; timer = 20; }
          else { rb.AddForce(new Vector2(0, 1) * jumpSpeed); WallJumpVelocity = -10; timer = 20; }
     
         
      }
      grounded = false;
      isWallJUmpActive = false;
       
    }

    void OnWallTouch(bool isLeftwall)
    {
        if (rb.velocity.y < 0) { rb.velocity = new Vector2(rb.velocity.x, stabilizer - 0.5f); isWallJUmpActive = true; isLeft = isLeftwall; }

    }
    void OnPlayerMove(float moveVelocity)
    {
       playerVelocity = moveVelocity;
    }
}
