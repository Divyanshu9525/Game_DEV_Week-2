using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    //Stored all states of menu
    public MonoBehaviour PlayerMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Resume()
    {
        PlayerMovement.enabled = true;
        GameManager.Instance.TogglePause();
    }
    // Update is called once per frame
    public void MainMenu()
    {
        gameManager.LoadMainMenu();
    }

    public void Restart()
    {
        gameManager.RestartLevel();
    }

    public void playAgain()
    {
        gameManager.PlayAgain();
    }

    void Update()  //esc key to pause or unpause scene
    {
        if(Input.GetKeyDown(KeyCode.Escape)) GameManager.Instance.TogglePause();
    }
}
