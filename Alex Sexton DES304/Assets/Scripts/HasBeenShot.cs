using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasBeenShot : MonoBehaviour
{
    //Script on a NavMesh actor, communicating with the outside world.
    //This is its own script primarily so the pathing script can be subbed out without impacting the actor's behaviour when shot.
    //It won't be. But it could.
    [SerializeField] private GameObject parent;
    RovingTargetScript parentScript;
    Type1Move setNewTarget;
    public bool hasBeenShot = false;
    public bool reset = false;
    [SerializeField] private Transform myRoom;
    // Start is called before the first frame update
    void Start()
    {
        parentScript = parent.GetComponent<RovingTargetScript>();
        setNewTarget = GetComponent<Type1Move>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hasBeenShot == true)
        {
            //Debug.Log("I have been shot");
            transform.position = myRoom.position;
            hasBeenShot = false;
            parentScript.disappointed = true;
        }

        if (reset == true)
        {
            reset = false;
            setNewTarget.SetDestination();
        }
    }
}
