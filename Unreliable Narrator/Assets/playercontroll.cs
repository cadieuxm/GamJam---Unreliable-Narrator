using UnityEngine;
using UnityEngine.InputSystem;

public class playercontroll : MonoBehaviour
{
    public InputSystem_Actions inputActions;
    public InputAction move, jump;
    float movementSpeed = 10f;
    Rigidbody2D rb;
    Vector2 horizInput, velocity, targetVelocity, currentVelocity;

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
        targetVelocity = new Vector2(horizInput.x * movementSpeed, rb.linearVelocity.y);
        rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, targetVelocity, ref currentVelocity, 1); //change later for smoothing
    }



}
