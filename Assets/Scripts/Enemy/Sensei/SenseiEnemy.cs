using UnityEngine;
using UnityEngine.AI;

public class SenseiEnemy : MonoBehaviour
{
    enum State
    {
        Idle,
        Move
    }

    [SerializeField] private float distanceToDiscoverPlayer;

    [Header("For debug")]
    public GameObject player;

    private NavMeshAgent agent;

    private State currentState;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        currentState = State.Idle;
    }

    private void FixedUpdate()
    {
        switch(currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Move:
                Move();
                break;
        }
    }

    private void Idle()
    {
        agent.isStopped = true;
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < distanceToDiscoverPlayer)
            currentState = State.Move;
    }

    private void Move()
    {
        agent.isStopped = false;
        agent.destination = player.transform.position;
    }
}
