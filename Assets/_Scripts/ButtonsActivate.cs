using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsActivate : MonoBehaviour
{
    [SerializeField]
    LayerMask WhatIsButton;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) if (target != null) target.GetComponent<IOnButtonUse>().OnAction();
    }

    private void FixedUpdate()
    {
        FindButton(Physics2D.OverlapCircleAll(transform.position, 3f, WhatIsButton));
    }
    float distance;
    Collider2D target;

    void FindButton(Collider2D[] colliders) {
        if (target != null) target.GetComponent<IOnButtonUse>().OnLeave();
        target = null;
        distance = 100;
        
        foreach (var collider in colliders) 
        {
          
            if (distance > Vector2.Distance(transform.position, collider.transform.position))
            {
                distance = Vector2.Distance(transform.position, collider.transform.position);
                target = collider;
            }
           
        }
        if (target != null) target.GetComponent<IOnButtonUse>().OnEnter();
    }

    void OnEPressed() {



    }
}
