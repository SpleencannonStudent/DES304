using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunTrigger : MonoBehaviour
{
    //Script connected to an otherwise empty child of a navmesh actor, used to determine when a player enters a trigger volume.
    //The navmesh actor already had a comparable collider, didn't want conflicts.
    [SerializeField] private GameObject Agent;
    Type1Move AgentMove;
    private bool countdown = false;
    private float safeCount = 0.0f;
    private float safeMax = 6.0f;

    // Start is called before the first frame update
    void Start()
    {
        AgentMove = Agent.GetComponent<Type1Move>();
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown == true)
        {
            safeCount += Time.deltaTime;
            if (safeCount >= safeMax)
            {
                countdown = false;
                AgentMove.fleeing = false;
                AgentMove.SetDestination();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            AgentMove.fleeing = true;
            AgentMove.playerLocation = other.transform;
            AgentMove.SetDestination();
            countdown = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            safeCount = 0.0f;
            countdown = true;
        }
    }
}
