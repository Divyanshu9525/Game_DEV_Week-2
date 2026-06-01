using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //Basic menu manager
    public void StartGame()
    {
        SceneManager.LoadScene("Level1Scene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}