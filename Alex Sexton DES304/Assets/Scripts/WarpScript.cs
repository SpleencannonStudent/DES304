using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpScript : MonoBehaviour
{
    //Script utilised when an object needs to be destroyed by another object or script, independently of its other functions.
    public bool deleting = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (deleting == true)
        {
            Destroy(gameObject);
        }
    }
}
