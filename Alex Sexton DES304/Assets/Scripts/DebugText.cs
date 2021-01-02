using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    //Heat counter display script.
    //Turns text sent from FPS_Player to ui text, nothing special.
    public float uiText;
    Text debugUi;

    // Start is called before the first frame update
    void Start()
    {
        debugUi = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        debugUi.text = uiText.ToString();
    }
}
