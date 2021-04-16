using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    Animator anim;
    SpriteRenderer spriteRend;
    Rigidbody2D rb2d;

    private void Start()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        PlayerInputSystem.onMoveInput += ReverseOnMove;
        PlayerInputSystem.onMoveInput += OnMove;
        PlayerInputSystem.onJumpButtonPressed += onJump;
        PlayerRaycasts.SetOnGrounded += CheckGround;
    }
    private void FixedUpdate()
    {
        onJump();
    }

    void ReverseOnMove(float moveVelocity)
    {
        if (moveVelocity < 0) spriteRend.flipX = true;
        else if (moveVelocity > 0) spriteRend.flipX = false;
    }

    void OnMove(float moveVelocity)
    {

        if (Mathf.Abs(moveVelocity) > 0)
            anim.SetBool("IsStartRun", true);
        else anim.SetBool("IsStartRun", false);
    }

    void onJump()
    {
    //    if (rb2d.velocity.y > 0.01f) anim.SetBool("isJump", true); else if (rb2d.velocity.y < -0.01f) anim.SetBool("isFall", true); else 
    }

    void CheckGround(bool isGrounded)
    {
         anim.SetBool("isJump", false); anim.SetBool("isFall", false); 
        if (isGrounded == false) if (rb2d.velocity.y > 0.01f) anim.SetBool("isJump", true); else if (rb2d.velocity.y < -0.01f) anim.SetBool("isFall", true);
    }
}
