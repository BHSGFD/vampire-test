using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementOnVelocity : MonoBehaviour
{
    [SerializeField] float speed = 0.15f;
    [SerializeField] int jumpForce = 350;

    [SerializeField] BoxCollider2D Stay;
    [SerializeField] BoxCollider2D Sit;

    bool isSit = false;
    Animator anim;
    SpriteRenderer spriteRend;
    Rigidbody2D rb;

    bool isGrounded = false;
    bool isSlide = false;

    float MovementVelocity;
    float jumpVelocity = 0;

    private void Start()
    {

        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    float ForceVelocity = 0;
    private void Update()
    {
        Move();
        RaycastWalls();
        jumpPressed();
        MovementVelocity = Input.GetAxis("Horizontal") + ForceVelocity;
       
        //if (isLeft == true && isNearWall == true) if (MovementVelocity > 0) MovementVelocity = 0;
        //if (isLeft == false && isNearWall == true) if (MovementVelocity < 0) MovementVelocity = 0;

    }

    private void FixedUpdate()
    {

        if (ForceVelocity > 0) ForceVelocity -= 0.01f;
        if (ForceVelocity < 0) ForceVelocity += 0.01f;
        // gameObject.transform.Translate(new Vector3(MovementVelocity * speed * Time.fixedDeltaTime, jumpVelocity, 0));
        float HorForce = jumpVelocity - 0.1f;
        if (jumpVelocity > 0) jumpVelocity -= 0.01f;
        if (isGrounded == true) jumpVelocity = 0;
        if (rb.velocity.y < -0.1f && isGrounded == false) anim.SetBool("isFall", true);
        rb.MovePosition(transform.position + new Vector3(MovementVelocity * speed * Time.fixedDeltaTime, HorForce, 0));
        RaycastGround();


        Debug.Log(ForceVelocity);
    }

    private void Move()
    {

        if (Input.GetAxis("Horizontal") < 0) spriteRend.flipX = true;
        else if (Input.GetAxis("Horizontal") > 0) spriteRend.flipX = false;

        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.05f) anim.SetBool("IsStartRun", true);
        else anim.SetBool("IsStartRun", false);

        if (Input.GetKey(KeyCode.X))
        {
            isSit = true;
            anim.SetBool("isSitButton", true);
            Stay.enabled = false;
            Sit.enabled = true;
        }
        else { anim.SetBool("isSitButton", false); isSit = false; }


    }
    private void jumpPressed()
    {


        if (Input.GetKeyDown(KeyCode.Space) && isSlide == true && isGrounded == false)
        {
            if (isLeft == true)
            {
                //  rb.velocity = new Vector2(0, 0f);
                //  rb.AddForce(new Vector2(0.5f, 1) * jumpForce);
                ForceVelocity += 0.7f;
                Jump();           }
            if (isLeft == false)
            {
                ForceVelocity += -0.7f;
                Jump();
            }

        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true) Jump();
    }

    private void Jump()
    {
        //rb.AddForce(Vector2.up * jumpForce);
        jumpVelocity = 1f;
        isGrounded = false;
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.tag == "Ground") isGrounded = true;
    //     if (collision.tag == "Ground") anim.SetBool("isJump", false); anim.SetBool("isFall", false);
    // }
    // private void OnTriggerStay2D(Collider2D collision)
    // {
    //     if (collision.tag == "Ground") isGrounded = true;
    // }
    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     Debug.Log("its exit");
    //    isGrounded = false;
    //         anim.SetBool("isJump", true);
    // }

    bool StayOnGround = false;
    


    
    private void RaycastGround()
    {
        RaycastHit2D[] raycastHit2D = new RaycastHit2D[3];

        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 1.34f), new Vector2(0, -0.1f));
        Debug.DrawRay(new Vector2(transform.position.x + 0.58f, transform.position.y - 1.34f), new Vector2(0, -0.1f));
        Debug.DrawRay(new Vector2(transform.position.x - 0.50f, transform.position.y - 1.34f), new Vector2(0, -0.1f));


        raycastHit2D[0] = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 1.34f), new Vector2(0, -0.1f), 0.01f);
        raycastHit2D[1] = Physics2D.Raycast(new Vector2(transform.position.x + 0.58f, transform.position.y - 1.34f), new Vector2(0, -0.1f), 0.01f);
        raycastHit2D[2] = Physics2D.Raycast(new Vector2(transform.position.x - 0.50f, transform.position.y - 1.34f), new Vector2(0, -0.1f), 0.01f);

        for (int i = 0; i < raycastHit2D.Length; i++)
        {
            if (StayOnGround == false)
            {
               // if (raycastHit2D[i].distance < 0.01f)
               // {
                    if (raycastHit2D[i].collider != null && raycastHit2D[i].collider.tag == "Ground" )
                    {
                        if (isSit == false)
                        {
                            Stay.enabled = true;
                            Sit.enabled = false;
                        }
                        isGrounded = true;
                        anim.SetBool("isJump", false); anim.SetBool("isFall", false);
                        StayOnGround = true;
                //    }
                }
                else
                {
                    Stay.enabled = false;
                    Sit.enabled = true;
                    isGrounded = false;
                    anim.SetBool("isJump", true);
                }
            }
        }

        StayOnGround = false;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ForceVelocity = 0;
    }
    bool isLeft;
    bool isNearWall = false;


    private void RaycastWalls()
    {
        RaycastHit2D[] raycastHitLeftWall = new RaycastHit2D[2];
        RaycastHit2D[] raycastHitRightWall = new RaycastHit2D[2];

        Debug.DrawRay(new Vector2(transform.position.x - 0.52f, transform.position.y + 0.65f), new Vector2(-0.1f, 0));
        Debug.DrawRay(new Vector2(transform.position.x - 0.52f, transform.position.y), new Vector2(-0.1f, 0));

        raycastHitLeftWall[0] = Physics2D.Raycast(new Vector2(transform.position.x - 0.52f, transform.position.y + 0.65f), new Vector2(-0.1f, 0), 0.005f);
        raycastHitLeftWall[1] = Physics2D.Raycast(new Vector2(transform.position.x - 0.52f, transform.position.y), new Vector2(-0.1f, 0), 0.01f);

        Debug.DrawRay(new Vector2(transform.position.x + 0.6f, transform.position.y + 0.65f), new Vector2(+0.1f, 0));
        Debug.DrawRay(new Vector2(transform.position.x + 0.6f, transform.position.y), new Vector2(+0.1f, 0));

        raycastHitRightWall[0] = Physics2D.Raycast(new Vector2(transform.position.x + 0.6f, transform.position.y + 0.65f), new Vector2(+0.1f, 0), 0.005f);
        //raycastHitRightWall[1] = Physics2D.Raycast(new Vector2(transform.position.x + 0.52f, transform.position.y), new Vector2(+0.1f, 0));
        if (raycastHitLeftWall[0].collider != null)
        {
            isLeft = true;
            if (raycastHitLeftWall[0].collider.tag == "Wall") { rb.velocity = new Vector2(0, -0.2f);
                isSlide = true;
                isNearWall = true; }
        }
        else

        if (raycastHitRightWall[0].collider != null)
        {
            isLeft = false;
            if (raycastHitRightWall[0].collider.tag == "Wall") { rb.velocity = new Vector2(0, -0.2f); isSlide = true; isNearWall = true; }
        }
        else { isNearWall = false; isSlide = false; }
        //  raycastHit2D[0] = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 1.5f), new Vector2(0, -0.1f));
        //  raycastHit2D[1] = Physics2D.Raycast(new Vector2(transform.position.x + 0.47f, transform.position.y - 1.5f), new Vector2(0, -0.1f));
        //  raycastHit2D[2] = Physics2D.Raycast(new Vector2(transform.position.x - 0.47f, transform.position.y - 1.5f), new Vector2(0, -0.1f));

    }
    
   
    Vector2 startPos;
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     Vector2 newVec = (transform.position - collision.collider.bounds.center) * 0.001f;
    //
    //     transform.position = new Vector2(transform.position.x + newVec.x * collision.collider.bounds.size.x / 2, transform.position.y + newVec.y * collision.collider.bounds.size.y / 2);
    //     
    // }

    // private void OnCollisionStay2D(Collision2D collision)
    // {
    //     transform.position = startPos;
    // }
}
