using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Type1Move : MonoBehaviour
{
    //This time working from https://www.youtube.com/watch?v=NGGoOa4BpmY and https://www.youtube.com/watch?v=CHV1ymlw-P8 series.
    //Altered so individual targets have their own locations to patrol between.

    //The Random.insideUnitSphere comes from a couple of places. Notably: https://answers.unity.com/questions/475066/how-to-get-a-random-point-on-navmesh.html
    //I'd wanted to use this in isolation as an alternate pathing method, but patrol points was working just fine.
    //Fortunately, I had another excuse.

    #region original testing code
    /*From the initial testing to check the AI could navigate the map.
    [SerializeField]
    Transform targetPoint;*/
    #endregion

    NavMeshAgent navMeshAgent;

    
    private bool isWaiting = false;
    private float waitTime = 2.0f;
    private float waitCurrent = 0.0f;
    private bool destinationReached = false;
    private int maxRandom;
    private int destinationRandom;
    private bool runCheck = false;
    private int destinationCurrent = 0;
    int wait = 0;
    private Vector3 randomPoint;
    private float fearRadius = 4f;

    public bool fleeing = false;
    public bool fleeSecondary = false;
    private float fleeRetarget = 2.0f;
    private float fleeTimer = 0.0f;


    //Tracking the transform component. Compared to TableFlip's tut, fewer moving parts = fewer things to go wrong.
    [SerializeField] private List<Transform> destinations;
    [SerializeField] private Transform fleeTarget;
    public Transform playerLocation;
    private Vector3 altFleeTarget;

    //For pathing when encountering the edge of a navmesh
    [SerializeField] private Transform fleeBackup;
    private Vector3 checkDistance;

    Transform targetPoint;

    // Start is called before the first frame update
    void Start()
    {
        maxRandom = destinations.Count;
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetDestination();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isWaiting== true)
        {
            waitCurrent += Time.deltaTime;
            if(waitCurrent >= waitTime)
            {
                isWaiting = false;
                SetDestination();
            }
        }
        //Courtesy of https://answers.unity.com/questions/324589/how-can-i-tell-when-a-navmesh-has-reached-its-dest.html?page=2&pageSize=5&sort=votes
        if (Vector3.Distance(transform.position, targetPoint.position) <= 0.8f && destinationReached == false)
            {
                //Debug.Log("Made it");

            wait = Random.Range(0, 2);

            if (wait == 0)
                {
                isWaiting = true;
                waitCurrent = 0.0f;
                destinationReached = true;
                }
            else
                {
                    SetDestination();
                }
                //destinationReached = true;
            }
        //Gives it a little more room for error when fleeing
        else if (fleeing == true && Vector3.Distance(transform.position, targetPoint.position) <= 1.0f && destinationReached == false)
        {
            SetDestination();
        }

        //Making sure I'm not doubling back
        if (runCheck == true)
        {
            if (destinationRandom == destinationCurrent)
            {
                SetDestination();
            }
            else
            {
                runCheck = false;
                destinationCurrent = destinationRandom;
                Vector3 targetVector = targetPoint.transform.position;
                navMeshAgent.SetDestination(targetVector);
                destinationReached = false;
            }
        }
    }

    public void SetDestination()
    {
        if (fleeing == false)
        {
            //First, makes sure its speed values are reset (in case it was running away)
            navMeshAgent.speed = 3.5f;
            navMeshAgent.angularSpeed = 120;
            navMeshAgent.acceleration = 8;
            destinationRandom = Random.Range(0, maxRandom);
            //Debug.Log(destinationRandom);
            targetPoint = destinations[destinationRandom];
            runCheck = true;
        }
        else
        {
            //Making sure it was triggering at the right time.
            //Debug.Log("Oh, no. A scary player!");
            navMeshAgent.speed = 7f;
            navMeshAgent.angularSpeed = 240;
            navMeshAgent.acceleration = 12;
            randomPoint = Random.insideUnitSphere * fearRadius;
            if (fleeSecondary == true)
            {
                //If the navmesh actor has already encountered the player during the fleeing period, uses this to designate a random flee location
                //ahead of itself
                Debug.Log("Forward target");
                targetPoint.position = randomPoint + fleeTarget.position;
                checkDistance = new Vector3(transform.position.x - targetPoint.position.x, transform.position.y - targetPoint.position.y, transform.position.z - targetPoint.position.z);
                //Checks the new destination isn't already reached. If so, designates a point behind itself to flee to, instead.
                if (checkDistance.magnitude < 1.0f || targetPoint.position == null)
                {
                    Debug.Log("Reverse target");
                    //A note on the object "Run1": I had originally intended to use this for the navmesh actor's initial attempt to flee the player
                    //Before realising I couldn't just assume the actor was travelling directly at the player
                    //Kept it around in case I needed it for anything else. Turns out, I did.
                    randomPoint = Random.insideUnitSphere * fearRadius;
                    targetPoint.position = randomPoint + fleeBackup.position;
                }
            }
            else
            {
                //Gets the difference between the player's position and itself
                altFleeTarget = playerLocation.position - transform.position;

                //Then removes the Z co-ord and sets it eight units from the enemy
                altFleeTarget.y = 0;
                altFleeTarget = altFleeTarget.normalized*8;

                //Then designates a spot on the opposite side of the enemy
                altFleeTarget.x = transform.position.x - altFleeTarget.x;
                altFleeTarget.y = transform.position.y;
                altFleeTarget.z = transform.position.z - altFleeTarget.z;

                //And specifies the run-to point as relative to that, so the enemy runs away from the player
                targetPoint.position = randomPoint + altFleeTarget;
                fleeSecondary = true;
            }
            navMeshAgent.SetDestination(targetPoint.transform.position);
            runCheck = false;
            destinationReached = false;
        }

        /*if(targetPoint != null)
        {
            Vector3 targetVector = targetPoint.transform.position;
            navMeshAgent.SetDestination(targetVector);
        }*/
    }
}
