using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RovingTargetScript : MonoBehaviour
{
    //Navmesh actor's parent. Originally sent them to their room, but now they do it themselves.
    //Disables them for a set amount of time after being shot.
    [SerializeField] private GameObject familyDisappointment;
    HasBeenShot HasBeenShot;
    Type1Move Type1Move;
    public bool disappointed = false;
    float timeoutTime = 10.0f;
    float timeoutCurrent = 0.0f;
    bool timeout = false;
    [SerializeField] private Transform hisRoom;
    void Start()
    {
        HasBeenShot = familyDisappointment.GetComponent<HasBeenShot>();
        Type1Move = familyDisappointment.GetComponent<Type1Move>();
    }

    // Update is called once per frame
    void Update()
    {
        if (disappointed == true)
        {
            Type1Move.fleeing = false;
            Type1Move.fleeSecondary = false;
            familyDisappointment.SetActive(false);
            timeout = true;
            disappointed = false;
        }

        if (timeout == true)
        {
            if (timeoutCurrent >= timeoutTime)
            {
                familyDisappointment.SetActive(true);
                HasBeenShot.reset = true;
                timeoutCurrent = 0.0f;
                timeout = false;
            }
            else
            {
                timeoutCurrent += Time.deltaTime;
            }
        }
    }
}
