using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    GameObject spawnPoint, playerCharacter, playerInstance;
    public GameObject GameOverCanvas;
    playercontroll PlayerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPoint = GameObject.FindWithTag("Respawn");
        playerCharacter = Resources.Load("Isabelle").GameObject();
       playerInstance =  Instantiate(playerCharacter, spawnPoint.transform.position,Quaternion.identity);
       

        PlayerController = playerInstance.GetComponent<playercontroll>();
        PlayerController.PlayerDies += GameOver;
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GameOver()
    {
        //run this when a listener hears a death event from the playercontroll

        //call a game over screen with a restart button

        Destroy(playerInstance);
        GameOverCanvas.SetActive(true);
        PlayerController.PlayerDies -= GameOver;

        //respawn enemies

    }
    public void RetryButtonPressed()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
