using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ChangeVelocity(float moveInput);
public delegate void ButtonPressed();

public class PlayerInputSystem : MonoBehaviour
{
    public static ChangeVelocity onMoveInput;
    public static ButtonPressed onJumpButtonPressed;
    public static ButtonPressed onEPressed;

    void Update()
    {
         if (Input.GetAxis("Jump") > 0.5f) onJumpButtonPressed();
        // if (Input.GetAxis("Action Button") > 0.5f) onEPressed();
    }

    private void FixedUpdate()
    {
         onMoveInput(Input.GetAxis("Horizontal"));
    }


}
