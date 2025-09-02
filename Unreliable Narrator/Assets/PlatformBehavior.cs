using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class PlatformBehavior : MonoBehaviour
{



    float targetposition, startposition;
    [SerializeField]
    public float amplitude,currentvelocity;
    bool canStop = true;
    bool playerincontact = false;
    playercontroll playercontroller = null;
   
    //make it to where when player is in contact with moving platform tag, it adds the velocity of the platform to the player as well, and when they leave contact, the player's velocity goes back to normal
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startposition = this.transform.localPosition.x;
        targetposition = startposition + amplitude;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocityX = currentvelocity;
       
        if (Mathf.Abs(rb.position.x - targetposition) <= 0.05f)
        {
            if (canStop == true)
            StartCoroutine("PlatformWait");
        }
        if (playerincontact)
        {
            playercontroller.addedvelocity = currentvelocity;

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       playerincontact = true;
       playercontroller = collision.gameObject.GetComponent<playercontroll>();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        playerincontact = false;

    }

    IEnumerator PlatformWait()
    {
        canStop = false;
        float tempVel = currentvelocity;
        currentvelocity = 0f;
        yield return new WaitForSeconds(2f);
        amplitude = -amplitude;
        targetposition += 2 * amplitude;
        currentvelocity = -tempVel;
        yield return new WaitForSeconds(0.5f);
        canStop = true;
    }
}
