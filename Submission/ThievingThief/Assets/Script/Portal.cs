using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string sceneToLoad;
    //transitions from scene1 to scene2
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bean"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}