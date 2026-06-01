using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float crtSpeed;
    public float rotSpeed = 10f;
    public bool isHidden = false;
    float ogheight, cheight;
    bool isCrouching;
    Vector3 moveDir;  
    Vector3 ogScale;
    Rigidbody rb;
    CapsuleCollider col;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        ogScale = transform.localScale;   //stores original size (mesh)
        ogheight = col.height;   //stores original size (collider)
        cheight = ogheight * 0.5f;  //stores crouch size (collider)
    }

    // Update is called once per frame
    void Update()
    {
        isCrouching = Input.GetKey(KeyCode.LeftControl);   
        crtSpeed = speed;
        if(isCrouching)   //check for crouch and applies conditions
        {
            transform.localScale = new Vector3(ogScale.x, ogScale.y * 0.5f, ogScale.z);
            col.height = cheight;   //makes it a dwarf
        }
        else   //reverts back to original conditions
        {
            transform.localScale = ogScale;
            col.height = ogheight;   //back to tall boi
        }
        float h = Input.GetAxisRaw("Horizontal");  //gets Input to control player (bean)
        float v = Input.GetAxisRaw("Vertical");    // this as well
        moveDir = new Vector3(h, 0f, v).normalized;   // Unit vector of resultant direction of input
    }

    void FixedUpdate()   //uses physics to give movement (and control speed based on if it's crouching)
    {
        rb.MovePosition(moveDir * crtSpeed * Time.fixedDeltaTime + rb.position);
        crtSpeed = isCrouching ? speed * 0.5f : speed;
        if(moveDir.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotSpeed * Time.fixedDeltaTime));
        }
        
    }

    void OnTriggerEnter(Collider spot)  //collider conditions for entry in collider
    {
        if(spot.gameObject.layer == LayerMask.NameToLayer("HideSpot")) isHidden = true;  //for hidden spot collider
        if (spot.gameObject.layer == LayerMask.NameToLayer("WinPos")) GameManager.Instance.TriggerWin();  //for win position collider
    }

    void OnTriggerExit(Collider spot)
    {
        if(spot.gameObject.layer == LayerMask.NameToLayer("HideSpot")) isHidden = false;  //for exiting hidden spot 
    }
}