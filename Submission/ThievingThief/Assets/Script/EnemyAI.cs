using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public MonoBehaviour PlayerMovement;
    public Transform thief;
    PlayerMovement player;
    public NavMeshAgent agent;
    public Transform[] waypoints;
    int index = 0, dir = 1;
    public float visionRange = 8f; //visionRange and viewAngle are same to spotlight(except in scene2 changed viewangle to 60)
    public float viewAngle = 45f;
    float lastSeenTime;
    public float killDis = 1.5f;   //within this you die
    public bool playerDead = false;
    public float forgetTime = 2f;  //amnesia timer
    [SerializeField] LayerMask visionMask;   //will only sense of Layer Player
    [SerializeField] private GameManager gameManager;
    enum State { Patrol, Chase }
    State state = State.Patrol;  //original state

    void Start()
    {
        agent.Warp(waypoints[0].position);  // teleposts to starting Patrol location
        agent.SetDestination(waypoints[0].position);
        player = thief.GetComponent<PlayerMovement>();  //gets access to PlayerMovement script
    }

    void Update()
    {
        if (playerDead)
        {
            Patrol();
            return;
        }
        if (CanSeePlayer()) // if seen Chase!!
        {
            lastSeenTime = Time.time;  //checks for time passed
            state = State.Chase;
            FindObjectOfType<MusicManager>().StartChase();  //music of chase (its really low heartbeat sfx so hard to hear)
        }
        if (state == State.Chase && !player.isHidden)
        {
            agent.speed = 4f;  //increase speed when in Chase State
            agent.SetDestination(thief.position);
            if (Time.time - lastSeenTime > forgetTime)  //removes amnesia (only forgets about player after 2 seconds of not satisfying the conditiosn)
            {
                //back to patrol settings
                state = State.Patrol;   
                agent.speed = 2f;
                agent.SetDestination(waypoints[index].position);
            }
        }
        else
        {
            FindObjectOfType<MusicManager>().StopChase();  //stop heartbeat sfx
            Patrol();   //get back to patrol if lost player
        }
        checkKill();   //checks for kill
    }

    void checkKill()  //checks for player is dead
    {
        if(playerDead) return;
        float dist = Vector3.Distance(transform.position, thief.position);  //distance between enemy and position
        if(dist < killDis && state == State.Chase && !player.isHidden)
        {
            playerDead = true;
            Debug.Log("Player Dead!");
            if(PlayerMovement != null) PlayerMovement.enabled = false;  //disables movement script if player is dead
            agent.ResetPath();  //back to patrol
            
            gameManager.TriggerGameOver();  //triggers game over screen
        }
    }

    bool CanSeePlayer()  //player visibility check
    {
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 target = thief.position + Vector3.up * 1.0f;
        Vector3 dirVec = target - origin;
        float distance = dirVec.magnitude;
        Vector3 dir = dirVec.normalized;

        if (distance > visionRange)   //false if more than 8 units away
            return false;
        float angle = Vector3.Angle(transform.forward, dir);   //false if gets out of cone
        if (angle > viewAngle / 2f)
            return false;
        Debug.DrawRay(origin, dir * visionRange, Color.red);   //Idk Chatgpt told me to add this(just colors the rays)
        if (distance < 1.5f)  //takes care of error (if the person gets too close forgets about the player)
            return true;
        if (Physics.SphereCast(origin, 0.5f, dir, out RaycastHit hit, visionRange, visionMask))   //all conditions for enemy to detect player
        {
            if (hit.collider.transform.root.CompareTag("Bean"))
                return true;
        }
        return false;
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            index += dir;
            if (index >= waypoints.Length - 1)
                dir = -1;
            else if (index <= 0)
                dir = 1;
            agent.SetDestination(waypoints[index].position);  //loops through all waypoints positions(Patrols)
        }
    }
}