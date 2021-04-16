using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animtionController : MonoBehaviour
{
   

    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        PlayerMove.Movementcontroll += OnMove;
        PlayerMove.Jump += OnJump;
        PlayerMove.Ground += OnGround;

    }

    void OnMove(float value)
    {
        if (Mathf.Abs(value) > 0)
            anim.SetBool("IsStartRun", true);
        else anim.SetBool("IsStartRun", false);

    }

    void OnJump(float value)
    {
        if (anim.GetBool("isJump") == false)
        {
            if (value > 0) { anim.SetBool("isJump", true); anim.SetBool("isFall", false); }
        }
        else if (anim.GetBool("isFall") == false && value < 0) ;
            if (value < 0) { anim.SetBool("isJump", true); anim.SetBool("isFall", true); }
       // else { anim.SetBool("isJump", false); anim.SetBool("isFall", false); }
    }

    void OnGround(float value)
    {
        if(anim.GetBool("isJump") == true)
          anim.SetBool("isJump", false); anim.SetBool("isFall", false); 
    }
}
