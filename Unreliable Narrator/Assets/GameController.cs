using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{

    GameObject spawnPoint, playerCharacter, playerInstance;
    public GameObject GameOverCanvas, changingTileMap;
    playercontroll PlayerController;

    //trying out shader shit
    TilemapRenderer tilemapRenderer;
    //

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPoint = GameObject.FindWithTag("Respawn");
        playerCharacter = Resources.Load("Isabelle").GameObject();
        playerInstance =  Instantiate(playerCharacter, spawnPoint.transform.position,Quaternion.identity);
       
        tilemapRenderer = changingTileMap.GetComponent<TilemapRenderer>();

        PlayerController = playerInstance.GetComponent<playercontroll>();
        PlayerController.PlayerDies += GameOver;
        PlayerController.SwitchNarrator += editTileMapColor;
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

    void editTileMapColor()
    {

        tilemapRenderer.sharedMaterial.color = new Color(1f,0.75f,1f);

    }

    private void OnApplicationQuit()
    {
        tilemapRenderer.sharedMaterial.color = Color.white;
    }
}
