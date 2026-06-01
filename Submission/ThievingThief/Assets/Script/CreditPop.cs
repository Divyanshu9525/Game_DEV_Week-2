using UnityEngine;
using TMPro;
// dosent work (also tried ebugging with chatgpt)
public class CreditPop : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TMP_Text text;
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float lifeTime = 1f;
    private CanvasGroup canvas;

    void Awake() //get to the canvas (screen overlay?)
    {
        canvas = GetComponent<CanvasGroup>();
    }

    public void Setup(int amount)
    {
        text.text = "+" + amount;  //updates amaount and text(which will be displayed on hud)
    }

    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime; 
        lifeTime -= Time.deltaTime;
        if (canvas != null)
            canvas.alpha = lifeTime;

        if (lifeTime <= 0f)
            Destroy(gameObject);
    }
}
