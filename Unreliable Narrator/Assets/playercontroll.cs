using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using System;

public class playercontroll : MonoBehaviour
{


    public float Health, MaxHealth;
    private bool isWallSliding,isHoldingJump;
    private float wallSlideSpeed = 2f;
    private int faceDirection = -1;


    public event Action PlayerDies,SwitchNarrator;
    GameObject respawnAnchor;


    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.1f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.1f;
    private Vector2 wallJumpingPower = new Vector2(10f, 10f);

    [SerializeField] private Transform wallCheck,groundCheck;
    [SerializeField] private LayerMask wallLayer,groundLayer;
    [SerializeField] private HealthBarUI HealthBar;

    public InputSystem_Actions inputActions;
    public InputAction move, jump,attack;
    float movementSpeed = 10f;
    float maxfallspeed = -20f;
    Rigidbody2D rb;
    Vector2 horizInput, targetVelocity, currentVelocity;
    public float addedvelocity = 0f;
    Animator animator,hitanimator;
    AnimatorStateInfo animStateInfo;

    private void Awake ()
    {
        inputActions = new InputSystem_Actions();
        respawnAnchor = GameObject.FindWithTag("RespawnAnchor");

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitanimator = this.GetComponentInChildren<Animator>();
        animStateInfo = hitanimator.GetCurrentAnimatorStateInfo(0);
        hitanimator.enabled = false;
        isHoldingJump = false;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 2f;
       // HealthBar.SetMaxHealth(MaxHealth);


    }

    private void OnEnable ()
    {
        move = inputActions.Player.Move;
        move.Enable();
        jump = inputActions.Player.Jump;
        jump.Enable();    
        attack = inputActions.Player.Attack;
        attack.Enable();
    }

    private void OnDisable () 
    {
        move.Disable();
        jump.Disable();
        attack.Disable();
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        if (isWallJumping) 
        {
            rb.linearVelocity = new Vector2(-horizInput.x *wallJumpingPower.x,wallJumpingPower.y);
        
        }
        else
        {
            //targetVelocity = new Vector2(horizInput.x * movementSpeed + addedvelocity, rb.linearVelocity.y);
            //rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, targetVelocity, ref currentVelocity, 0.05f); //change later for smoothing
            rb.linearVelocity = new Vector2(horizInput.x * movementSpeed + addedvelocity, rb.linearVelocity.y);
        }

         

            
            
        

        if (rb.linearVelocityY < maxfallspeed)
        {

            rb.linearVelocityY = maxfallspeed;
        }
    }
    private void Update()
    {
        horizInput = move.ReadValue<Vector2>();
        if (jump.IsPressed() && !isHoldingJump)
        {

            if (Grounded())
            {
                rb.linearVelocityY = 12.0f;
                isHoldingJump = true;
            }

            if (isWallSliding) //wall jump
            {
                isWallJumping = true;
                isHoldingJump = true;
                this.transform.localScale = new Vector3(transform.localScale.x * -1,transform.localScale.y,transform.localScale.z); //flip the character when wall jumping

                Invoke("StopWallJumping", wallJumpingDuration);
            }
        }

        if (jump.WasReleasedThisFrame())
        {
            isHoldingJump = false;
            rb.gravityScale = 3f;
            if (rb.linearVelocityY > 0f)
            {
                rb.linearVelocityY = 0f;
            }


        }

        if (!Grounded() && !isWallSliding)
        {
            if (rb.linearVelocityY < 0f)
            {
                rb.gravityScale = 3f;
            }
            else
            {
                rb.gravityScale = 2f;
            }
        }

        if (!isWallJumping)
        {
            flipSprite();
        }
        WallSlide();
        MoveAnchor();

        if (attack.IsPressed())
        {
            //GameObject attackhitbox = GameObject.FindGameObjectWithTag("Attack");
            //attackhitbox.GetComponent<BoxCollider2D>().enabled = true;
            //hitanimator.enabled = true;
            //hitanimator.Play("Entry");

            SwitchNarrator.Invoke();
        }

        if (hitanimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            GameObject attackhitbox = GameObject.FindGameObjectWithTag("Attack");
            attackhitbox.GetComponent<BoxCollider2D>().enabled = false;
            hitanimator.enabled = false;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            Health -= 1;

            
            transform.position = respawnAnchor.transform.position;

            PlayerDies.Invoke(); // NEED TO MAKE THIS ACTIVATE WHEN DYING, NEED TO MAKE A GOOD HEALTH SYSTEM.
            
            //if health ends up 0, call the "death/game over" function that hasn't been made yet

        
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


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check for collision with ground
        if (collision.gameObject.CompareTag("Ground"))
        {

        }
    }

    void flipSprite()
    {
        if(horizInput.x < 0f)
        {
            Vector3 localscale = transform.localScale;
            localscale.x = -1;
            transform.localScale = localscale;

        }
        else if(horizInput.x > 0f)
        {
            Vector3 localscale = transform.localScale;
            localscale.x = 1;
            transform.localScale = localscale;

        }
    }

    private bool IsWalled()
    {

        return Physics2D.OverlapCircle(wallCheck.position, 0.25f, wallLayer);

    }

    private void WallSlide()
    {
        if (IsWalled() && !Grounded() && horizInput.x != 0f){
            isWallSliding = true;
            this.GetComponent<SpriteRenderer>().color = Color.magenta;
            rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, -wallSlideSpeed, 2f);
        }
        else
        {
            this.GetComponent<SpriteRenderer>().color = Color.white;

            isWallSliding = false;
        }

    }
            

    private bool Grounded()
    {
        return Physics2D.OverlapBox(groundCheck.position,new Vector2(0.85f,0.3f), 0, groundLayer);
    }
  
    private void MoveAnchor()
    {
        if (Grounded())
        {
            respawnAnchor.transform.position = transform.position;

        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }



    public void SetHealth(float healthChange) 
    {
        Health += healthChange;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        HealthBar.SetHealth(Health);
    }
}
