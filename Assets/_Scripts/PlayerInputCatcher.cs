using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public enum MoveInput { left, right, none }

 public delegate void PlayerInput(float moveVelocity);
 public delegate void PlayerMovement(MoveInput direction);
 public delegate void PlayerJump();
 public delegate void PlayerSit();
 public delegate void PressedActionButton();

public class PlayerInputCatcher : MonoBehaviour
{
    public static MoveInput WallJump = MoveInput.none;
    public static PlayerInput onPlayerInput;
    public static PlayerMovement onPlayerMoveDir;
    public static PlayerJump OnJump;

    public float moveVelocity;

    private void Start()
    {
        

    }

    private void Update()
    {
        Jump();
    }
    private void FixedUpdate()
    {
        HorizontalMove();
    }

   void HorizontalMove()
    {
        Debug.Log(WallJump);
        moveVelocity = Input.GetAxis("Horizontal");
        if (WallJump == MoveInput.left) if (moveVelocity < 0) moveVelocity = 0;
        if (WallJump == MoveInput.right) if (moveVelocity > 0) moveVelocity = 0;
        onPlayerInput(moveVelocity);

        if (moveVelocity > 0) onPlayerMoveDir(MoveInput.right);
        else if (moveVelocity < 0) onPlayerMoveDir(MoveInput.left);
        else onPlayerMoveDir(MoveInput.none);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)) OnJump();
    }
}
