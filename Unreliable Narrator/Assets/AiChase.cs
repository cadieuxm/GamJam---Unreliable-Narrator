using UnityEngine;

// referenced MoreBBlakeyyy tutorial for enemy chase logic:
// https://www.youtube.com/watch?v=2SXa10ILJms&ab_channel=MoreBBlakeyyy
public class AiChase : MonoBehaviour
{
    public GameObject player;
    public float speed;
    private float distance;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //check how far away enemy is
        distance = Vector2.Distance(transform.position, player.transform.position);
        //find direction for enemy to get closer to player
        Vector2 direction = player.transform.position - transform.position;

        
        //get closer if far away
        if (distance > 3 && distance < 10)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }

        if (distance <=3)
        {
            //attack
        }
    }
}
