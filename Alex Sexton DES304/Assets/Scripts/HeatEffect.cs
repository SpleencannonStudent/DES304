using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatEffect : MonoBehaviour
{
    //Script handling the creation of heat field particle systems.
    //While most are instantiated in the same place, one is not.
    //Additionally, several are destroyed in different ways or at different times.
    [SerializeField] private GameObject heatWarp;
    [SerializeField] private GameObject burst;
    [SerializeField] private GameObject myContact;
    [SerializeField] private GameObject whirling;
    [SerializeField] private GameObject boomLight;
    private GameObject player;
    private GameObject myWarp;
    private WarpScript myWarpScript;
    private int layerMask = ~(1 << 8);
    public Vector3 warpPoint;
    public Vector3 lightVector;
    public Vector3 lightPoint;
    Ray trajectory;
    RaycastHit targetpoint;
    public bool deleting = false;
    // Start is called before the first frame update
    void Start()
    {
        CastRay();
        player = GameObject.FindWithTag("Player");
        lightVector = new Vector3(transform.position.x - player.transform.position.x, transform.position.y - player.transform.position.y, transform.position.z - player.transform.position.z).normalized * 0.1f;
        lightPoint = transform.position - lightVector;
        Instantiate(burst, transform.position, Quaternion.identity);
        Instantiate(myContact, lightPoint, Quaternion.identity);
        Instantiate(whirling, transform.position, Quaternion.identity);
        Instantiate(boomLight, lightPoint, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if(deleting == true)
        {
            myWarpScript.deleting = true;
            Destroy(gameObject);
        }
    }

    void CastRay()
    {
        //using the unity manual here.
        //Ignores the heat volume, finds the ground below the target, instantiates that wibbly heat effect there.
        //If there's a problem with that, instantiates the wibbly heat effect at its location.
        //Needs a collider under the map to function.
        trajectory = new Ray(transform.position + new Vector3(0.0f, -1.0f, 0.0f), Vector3.down);
        if (Physics.Raycast(trajectory, out targetpoint, Mathf.Infinity, layerMask) && targetpoint.collider != null)
        {
            if (targetpoint.transform.gameObject.tag == "Target" && targetpoint.distance <= 2.0f)
            {
                warpPoint = targetpoint.point;
                warpPoint.y -= 0.2f;
                myWarp = Instantiate(heatWarp, warpPoint, Quaternion.identity/*, gameObject.transform*/);
            }
            else
            {
                myWarp = Instantiate(heatWarp, transform.position, Quaternion.identity/*, gameObject.transform*/);
            }
            myWarpScript = myWarp.GetComponent<WarpScript>();
        }
    }
}
