using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioSource mainMusicSource;
    public AudioSource chaseMusicSource;
    public AudioSource sfxSource;
    public AudioClip chaseMusic;
    public AudioClip coinSound;
    public AudioClip pauseMusic;
    public AudioClip deathMusic;
    public AudioClip winMusic;
    public AudioClip mainMenuMusic;
    public AudioClip scene1Music;
    public static MusicManager instance;

    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);  //carries on to other scenes
        audioSource = mainMusicSource;
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayMusicForScene(SceneManager.GetActiveScene().name);  
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);  //plays appropriate music according to scene
    }

    void PlayMusicForScene(string sceneName)
    {
        AudioClip clip = null;  //sets audioclip to null

        switch (sceneName)
        {
            //selects correct clip for music according to scene
            case "StartMenu":
                clip = mainMenuMusic;
                break;
            case "Level1Scene":
            case "Level2":
                clip = scene1Music;
                break;
        }

        if (clip != null && audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    // overlay music (Coin, Chase, Death, Win)

    public void PlayCoin()
    {
        sfxSource.PlayOneShot(coinSound);
    }

    public void StartChase()
    {
        if (!chaseMusicSource.isPlaying)
        {
            chaseMusicSource.clip = chaseMusic;
            chaseMusicSource.loop = true;
            chaseMusicSource.Play();
        }
    }

    public void StopChase()
    {
        chaseMusicSource.Stop();
    }

    public void PlayDeath()
    {
        audioSource.Stop();
        chaseMusicSource.Stop();
        sfxSource.PlayOneShot(deathMusic);
    }

    public void PlayWin()
    {
        audioSource.Stop();
        chaseMusicSource.Stop();
        sfxSource.PlayOneShot(winMusic);
    }

    public void PlaySceneMusic(AudioClip clip)
    {
        if (clip == null) return;
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.volume = 1f;
        audioSource.Play();
    }

    public void ResetRuntimeAudio()  //resets runtime audio so pause, death and win menu music dont keep playing
    {
        chaseMusicSource.Stop();
        sfxSource.Stop();
        sfxSource.clip = null; 
        audioSource.Stop();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}