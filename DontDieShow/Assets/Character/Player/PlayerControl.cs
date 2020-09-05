using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    Animator anim;

    enum AnimatorState {
        Default = 0,
        PushPull = 1,
        Climbing = 2

    }

    [SerializeField]
    int animatorLayerIndex;

    [SerializeField]
    bool OnGround;

    [SerializeField]
    bool NearPushasbleObject;

    [SerializeField]
    bool FaceLeft;

    public float moveSpeed = 5;
    public float JumpForce = 200;
    public float climbUpSpeed;
    public float climbDownSpeed;
    //public int reducedSpeed = 4;

    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private LayerMask m_WhatIsPushable;

    // Start is called before the first frame update
    void Start()
    {
        animatorLayerIndex = (int)AnimatorState.Default;
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSpeed(float ReducedSpeed)
    {

        moveSpeed = ReducedSpeed;
        if (moveSpeed < 0) moveSpeed = 0;
    }

    public float GroundCheckDistance = 1.33f;
    bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        //float distance = 8.99f;

        //Debug.DrawRay(position, direction * GroundCheckDistance, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(position, direction, GroundCheckDistance, m_WhatIsGround);
        if (hit.collider != null)
        {
            return true;

        }

        return false;
    }

    public float PushableObjectCheckDistance = 0.95f;
    bool IsPushableObjectAhead() {

        Vector2 position = transform.position;

        Vector2 direction = Vector2.right;

        if (FaceLeft == true)
        {
            direction = Vector2.left;
        }
        else if (FaceLeft == false) {
            direction = Vector2.right;
        }

        Debug.DrawRay(position, direction * PushableObjectCheckDistance, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(position, direction, PushableObjectCheckDistance, m_WhatIsPushable);
        if (hit.collider != null)
        {
            return true;

        }

        return false;
    }

    void Move()
    {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");

        if (horizontalAxis > 0)
        {
            if (FaceLeft == true)
            {
                Vector3 newScale = this.transform.localScale;
                newScale.x *= -1;
                this.transform.localScale = newScale;
                FaceLeft = false;
            }
        }
        else if (horizontalAxis < 0)
        {
            if (FaceLeft == false)
            {
                Vector3 newScale = this.transform.localScale;
                newScale.x *= -1;
                this.transform.localScale = newScale;
                FaceLeft = true;
            }

        }

        this.transform.Translate(horizontalAxis * moveSpeed * Time.fixedDeltaTime, 0, 0);

        if (animatorLayerIndex == (int)AnimatorState.PushPull)
        {
            anim.SetFloat("Horizontal", horizontalAxis * horizontalAxis);
        }
        else {
            anim.SetFloat("Horizontal", horizontalAxis);
        }
       
    }

    void Jump() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (OnGround == true)
            {
                anim.SetTrigger("Jump");
                anim.ResetTrigger("Jump");
                Rigidbody2D rgbd = GetComponent<Rigidbody2D>();
                rgbd.AddForce(transform.up * JumpForce);
            }

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    private void FixedUpdate()
    {

        NearPushasbleObject = IsPushableObjectAhead();


        OnGround = IsGrounded();
        anim.SetBool("OnGround", OnGround);

        if (NearPushasbleObject)
        {
            animatorLayerIndex = (int)AnimatorState.PushPull;
            anim.SetLayerWeight(animatorLayerIndex, 1);
            GameObject.Find("PushCollider").GetComponent<BoxCollider2D>().enabled = true;
        }
        else {
            animatorLayerIndex = (int)AnimatorState.PushPull;
            anim.SetLayerWeight(animatorLayerIndex, 0);
            GameObject.Find("PushCollider").GetComponent<BoxCollider2D>().enabled = false;
        }


        Move();
        Jump();

        
    }
}
