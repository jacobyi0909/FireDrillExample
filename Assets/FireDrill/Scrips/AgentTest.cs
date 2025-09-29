using UnityEngine;
using UnityEngine.AI;

public class AgentTest : MonoBehaviour
{
    NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        var player = GameObject.Find("Player");

        agent.SetDestination(player.transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
