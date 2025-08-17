using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class playercontroll : MonoBehaviour
{
    public InputSystem_Actions inputActions;
    public InputAction move, jump;
    float movementSpeed = 10f;
    Rigidbody2D rb;
    Vector2 horizInput, velocity, targetVelocity, currentVelocity;
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
        velocity = new Vector2();
        targetVelocity = new Vector2(horizInput.x * movementSpeed + addedvelocity, rb.linearVelocity.y);
        rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, targetVelocity, ref currentVelocity, 0.05f); //change later for smoothing

        if (jump.IsPressed())
        {

            if (canJump == true)
            {
                rb.AddForceY(400.0f);
            }
            



        }
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


}
