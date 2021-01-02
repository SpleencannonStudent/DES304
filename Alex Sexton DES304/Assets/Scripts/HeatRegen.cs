using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatRegen : MonoBehaviour
{
    //When the player enters a heat field, it creates an object with this script on it.
    //This script will restore heat for a set period of time, with intensity based on how long the heat field had been active.
    //The value is read from the player, removing the need for any additional dependencies.
    //After this period, it destroys itself.
    #region Game objects and scripts
    [SerializeField] private GameObject player;
    FPS_Player playerScript;
    #endregion

    #region Variables
    private float heatGain = 0.0f;
    private float heatTime = 1.6f; //Down from 2.0f;
    private float timer = 0.0f;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
        {
            Debug.Log("Could not find player");
        }
        playerScript = player.GetComponent<FPS_Player>();
        heatGain = playerScript.heatGain;
    }

    // Update is called once per frame
    void Update()
    {
        //heatGain used to not be divided by 1.2
        if (timer < heatTime)
        {
            playerScript.heatCurrent += ((heatGain / 1.2f) / heatTime) * Time.deltaTime;
            timer += Time.deltaTime;
            //Debug.Log("Adding heat");
        }

        if (timer > heatTime)
        {
            //Debug.Log("Deleting");
            Destroy(gameObject);
        }
    }
}
