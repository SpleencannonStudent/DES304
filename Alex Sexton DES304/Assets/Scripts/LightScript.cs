using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    private Light light;
    private bool luminosity = false;
    private float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        light.range += 60*Time.deltaTime;
        if (luminosity == false)
        {
            if (timer <= 0.05f)
            {
                timer += Time.deltaTime;
                light.intensity = Mathf.Lerp(0, 5, timer / 0.05f);
            }
            else
            {
                luminosity = true;
                timer = 0.0f;
            }
        }
        if(luminosity ==  true)
        {
            if (timer <= 0.1f)
            {
                timer += Time.deltaTime;
                light.intensity = Mathf.Lerp(5, 0, timer / 0.1f);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
