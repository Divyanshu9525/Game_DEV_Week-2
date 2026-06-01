using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private MusicManager musicManager;
    [Header("UI Panels")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject winPanel;
    [SerializeField] private TMP_Text creditsText;
    bool isPaused;
    public int credits = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created/ Singleton guard — one instance only
    void Awake() 
    {
        musicManager = FindObjectOfType<MusicManager>();  //Loads to musicManager
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }
    // Pause
    public void TogglePause()
    {
        isPaused = !isPaused;  //toggles pause state
        Time.timeScale = isPaused ? 0 : 1;
        pausePanel.SetActive(isPaused);
    }
    // Credits
    public void AddCredits(int amount)
    {
        credits += amount;  //updates credits
        creditsText.text = credits.ToString();  //updated credits on hud
        Debug.Log($"Credits {credits}");
    }
    // Scenes
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");  //loads Start Menu
    }

    // YOUR CODE: pause the game
    public void TriggerGameOver() {
        if (musicManager != null)
        { 
            musicManager.PlayDeath();  //death music
        }
        Time.timeScale = 0f;        // freeze everything
        gameOverPanel.SetActive(true);
    }

    // YOUR CODE: restart
    public void RestartLevel() {
        MusicManager.instance.ResetRuntimeAudio();  //reset audio
        musicManager.PlaySceneMusic(musicManager.scene1Music);  //(was unable to play mainscene music after restart) So plays mainscene music
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    } 
    public void TriggerWin()
    {
        if (credits <= 0)  //check for if player has collected at least one treasure
        {
            Debug.Log("Need at least 1 credit to win!");
            TriggerGameOver();
            return;
        }
        FindObjectOfType<MusicManager>().PlayWin();  //play win music 
        Time.timeScale = 0f;
        winPanel.SetActive(true);  //enables win panel
    }

    public void PlayAgain()
    {
        MusicManager.instance.ResetRuntimeAudio();
        musicManager.PlaySceneMusic(musicManager.scene1Music);   //again plays scene music from here
        Debug.Log(Time.timeScale);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1Scene");   //loads from start (scene 1)
    }
}
