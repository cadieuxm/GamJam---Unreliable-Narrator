using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class playercontroll : MonoBehaviour
{

    private bool isWallSliding;
    private float wallSlideSpeed = 2f;


    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);


    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;


    public InputSystem_Actions inputActions;
    public InputAction move, jump;
    float movementSpeed = 10f;
    Rigidbody2D rb;
    Vector2 horizInput, targetVelocity, currentVelocity;
    bool canJump = true;
    public float addedvelocity = 0f;
    private void Awake ()
    {
        inputActions = new InputSystem_Actions();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable ()
    {
        move = inputActions.Player.Move;
        move.Enable();
        jump = inputActions.Player.Jump;
        jump.Enable();    
    }

    private void OnDisable () 
    {
        move.Disable();
        jump.Disable();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        horizInput = move.ReadValue<Vector2>();
        targetVelocity = new Vector2(horizInput.x * movementSpeed + addedvelocity, rb.linearVelocity.y);
        rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, targetVelocity, ref currentVelocity, 0.05f); //change later for smoothing

        if (jump.IsPressed())
        {

            if (canJump == true)
            {
                rb.AddForceY(400.0f);
            }

        }


        {
            if (!isWallJumping)
            {
                rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
            }
        }
    }
    private void Update()
    {
        WallJump();
        WallSlide();
        flipSprite();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Moving Platform"))
        {
           // addedvelocity = collision.rigidbody.linearVelocityX;

        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Moving Platform"))
        {
            addedvelocity = 0;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //check if player collision leaves contact with ground
        if (collision.gameObject.CompareTag("Ground")){
            canJump = false;
            print("ungrounded,can'tjump");
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check for collision with ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            print("grounded");
            canJump = true;

        }
    }

    void flipSprite()
    {
        if(horizInput.x > 0f)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
            wallCheck = this.transform.GetChild(1);
        }
        else if(horizInput.x < 0f)
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
            wallCheck = this.transform.GetChild(0);
        }
    }

    private bool IsWalled()
    {

        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);

    }

    private void WallSlide()
    {
        if (IsWalled() && !canJump && horizInput.x != 0f){
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, Mathf.Clamp(rb.linearVelocityY, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }

    }
            
    //private void WallJump()
    //{
    //    if (isWallSliding)
    //    {
    //        if (wallCheck = this.transform.GetChild(1)) // checks if the player needs to go left
    //        {
    //            this.GetComponent<SpriteRenderer>().flipX = false;
    //            wallCheck = this.transform.GetChild(0);
    //            rb.linearVelocity = new Vector2(3f, 10f);
    //            StartCoroutine("WallJumpCooldown");
    //        }
    //        else if (wallCheck = this.transform.GetChild(0))
    //        {
    //            this.GetComponent<SpriteRenderer>().flipX = true;
    //            wallCheck = this.transform.GetChild(1);
    //            rb.linearVelocity = new Vector2(-3f, 10f);
    //            StartCoroutine("WallJumpCooldown");

    //        }

    //    }

    //}
    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (jump.IsPressed() && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            //if (transform.localScale.x != wallJumpingDirection) // this flips everything. maybe just
            //{
            //    isFacingRight = !isFacingRight;
            //    Vector3 localScale = transform.localScale;
            //    localScale.x *= -1f;
            //    transform.localScale = localScale;
            //}

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }


    IEnumerator WallJumpCooldown()
    {
        jump.Disable();
        move.Disable();

        yield return new WaitForSeconds(0.2f);

        jump.Enable();
        move.Enable();
    }
}
