using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Type0Move : MonoBehaviour
{
    //Type 0 move is very basic, and was only ever intended to debug.
    //Set a location and a navmesh agent will go there. That's it.
    [SerializeField] Transform targetPoint;

    NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, targetPoint.position) <= 0.65f)
        {
            Debug.Log("Made it!");
        }
    }

    private void SetDestination()
    {
        if(targetPoint != null)
        {
            Vector3 targetVector = targetPoint.transform.position;
            navMeshAgent.SetDestination(targetVector);
        }
    }
}
