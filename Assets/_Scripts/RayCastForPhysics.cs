using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void FoundCollider(RaycastHit2D hit2D);
public class RayCastForPhysics : MonoBehaviour
{
    public static event FoundCollider stayOnGround;
    public static event FoundCollider touchWall;
    RaycastHit2D[] hit2DGround = new RaycastHit2D[3];

    private void FixedUpdate()
    {
       // FindGound();
    }
 

    void FindGound()
    {

        Debug.DrawRay(new Vector2(transform.position.x - 0.47f, transform.position.y - 1.35f), new Vector2(0, -0.01f));
        hit2DGround[0] = Physics2D.Raycast(new Vector2(transform.position.x - 0.47f, transform.position.y - 1.35f), new Vector2(0, -1), 0.001f);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 1.35f), new Vector2(0, -0.01f));
        hit2DGround[1] = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 1.35f), new Vector2(0, -1), 0.001f);
        Debug.DrawRay(new Vector2(transform.position.x + 0.58f, transform.position.y - 1.35f), new Vector2(0, -0.01f));
        hit2DGround[2] = Physics2D.Raycast(new Vector2(transform.position.x + 0.58f, transform.position.y - 1.35f), new Vector2(0, -1), 0.001f);

        foreach (var hit2D in hit2DGround)
        {
            if (hit2D.collider != null && hit2D.collider.tag == "Ground") stayOnGround(hit2D);
        }
        
    }

}
