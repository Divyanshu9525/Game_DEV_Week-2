using UnityEngine;

public class LootPickup : MonoBehaviour
{
    public LootDataSO data;
    void Awake()
    {
        GetComponent<Renderer>().material.color = data.meshColor;
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Bean")) {
            // YOUR CODE: tell the GameManager & destroy self
            GameManager.Instance.AddCredits(data.value);  //credits update using value info of coin
            FindObjectOfType<MusicManager>().PlayCoin();  //coin collection music played
            Destroy(gameObject);   //removes (destroys coin)
        }
    }
    void Start()
    {
        Debug.Log("LOOT START ON: " + gameObject.name);  //used to debug (was really buggy)
    }
}
