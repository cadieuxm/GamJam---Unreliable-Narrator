using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{

    GameObject spawnPoint,playerCharacter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPoint = GameObject.FindWithTag("Respawn");
        playerCharacter = Resources.Load("Isabelle").GameObject();
        Instantiate(playerCharacter, spawnPoint.transform.position,Quaternion.identity);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
