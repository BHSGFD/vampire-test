using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnItemAction{

    void OnAction(bool WhatNow);
   

}


public class OnItemAction : MonoBehaviour, IOnItemAction
{
    [SerializeField]
    PhysicsMaterial2D physicsMat2d;
    Vector2 m_velocity = Vector2.zero;
    [SerializeField]
    private float smothTime = 0.05f;
    [SerializeField]
    GameObject player;

    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
       
    }
    bool count = false;
    // Start is called before the first frame update
    Vector2 range = Vector2.zero;
    GameObject player1;
    private void FixedUpdate()
    {

      
        Vector2 targetVelocity = new Vector2(PlayerMove.VectorGetter.x, rb.velocity.y);
        if (count == true)
        {
            rb.sharedMaterial = physicsMat2d;
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref m_velocity, smothTime);
            if (Mathf.Abs(Vector2.Distance(transform.position, player.transform.position)) > 1.7f) count = false;
            Vector2.Distance(transform.position, player.transform.position);
        }
        else { rb.sharedMaterial = null; }
     
     
        
    }
    void IOnItemAction.OnAction(bool WhatNow)
    {
        //    if (count == true) count = false;
        //    else  count = true;
        count = WhatNow;
        rb.velocity = Vector2.zero;


        }
}
