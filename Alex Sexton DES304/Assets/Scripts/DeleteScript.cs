using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteScript : MonoBehaviour
{
    //Used to delete particle effects on a timer. Utilised when a particle effect needs to be disabled after it completes.
    [SerializeField] private float deleteTimer;
    private float deleteCounter = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deleteCounter += Time.deltaTime;

        if(deleteCounter >= deleteTimer)
        {
            Destroy(gameObject);
        }
    }
}
