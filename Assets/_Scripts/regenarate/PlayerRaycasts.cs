using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void ObjectRaycast();
public delegate void ObjectRaycastWithBoolParametr(bool parametr);

public class PlayerRaycasts : MonoBehaviour
{
    Rigidbody2D rb;
    public static ObjectRaycast OnGrounded;
    public static ObjectRaycastWithBoolParametr SetOnGrounded;
    public static ObjectRaycastWithBoolParametr IsLeftWall;
    Vector2 GroundBoxSize = new Vector2(1.48f, 0.1f);
    Vector2 WallBoxSize = new Vector2(0.1f,1.2f);
    
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] LayerMask WallLayer;
    [SerializeField] BoxCollider2D boxCollider;
    Collider2D groundHit;
    Collider2D leftWallHit;
    Collider2D rightWallHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GroundBoxSize.x = boxCollider.bounds.extents.x * 2 -0.05f;
        WallBoxSize.y = boxCollider.bounds.extents.y;
    }

    private void FixedUpdate()
    {
        GroundCheck();
        wallChek();
    }

    void GroundCheck()
    {
        SetOnGrounded(false);
        groundHit = Physics2D.OverlapBox(transform.position + new Vector3(-boxCollider.offset.x, -boxCollider.bounds.extents.y + 0.12f, 0), GroundBoxSize, 0, GroundLayer);
 
 
        
        if (groundHit != null ) SetOnGrounded(true);

    }

    void wallChek()
    {
       
       leftWallHit = Physics2D.OverlapBox(transform.position + new Vector3(-boxCollider.bounds.extents.x, 0.9f, 0), WallBoxSize, 0, WallLayer);
        if (leftWallHit != null) IsLeftWall(true);
            rightWallHit = Physics2D.OverlapBox(transform.position + new Vector3(boxCollider.bounds.extents.x, 0.9f, 0), WallBoxSize, 0, WallLayer);
        if (rightWallHit != null) IsLeftWall(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + new Vector3(-boxCollider.offset.x, -boxCollider.bounds.extents.y + 0.12f, 0), GroundBoxSize);
        Gizmos.DrawCube(transform.position + new Vector3(boxCollider.bounds.extents.x, 0.9f, 0), WallBoxSize );
        Gizmos.DrawCube(transform.position + new Vector3(-boxCollider.bounds.extents.x, 0.9f, 0), WallBoxSize );
    }
}
