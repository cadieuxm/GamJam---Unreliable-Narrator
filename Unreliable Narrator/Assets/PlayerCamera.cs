using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    GameObject Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            this.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y,-10);
        }
        else
        {
            Player = GameObject.FindWithTag("Player");
        }
        
    }
}
