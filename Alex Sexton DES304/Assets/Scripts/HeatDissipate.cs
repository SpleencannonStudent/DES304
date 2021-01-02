using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script controlling the size, lifespan and charge remaining in a heat volume
//Credit to https://answers.unity.com/questions/1211833/how-to-increase-the-size-of-the-collider.html
//https://en.wikipedia.org/wiki/Newton%27s_law_of_cooling
//https://gamedevbeginner.com/the-right-way-to-lerp-in-unity-with-examples/
//https://en.wikipedia.org/wiki/Heat_transfer
public class HeatDissipate : MonoBehaviour
{
    private GameObject dad;
    private HeatEffect dadEffect;
    private bool getTheCollider = false;
    private float startTime;
    private float scaleRate;
    private float heatCapacity;
    public float heatNow;
    private float heatMult;
    private float diffusionTime;
    public bool deleting = false;

    // Start is called before the first frame update
    void Start()
    {
        dad = transform.parent.gameObject;
        dadEffect = dad.GetComponent<HeatEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (getTheCollider == false)
        {
            getTheCollider = true;
            diffusionTime = 6.0f;
            heatCapacity = 30.0f;
            startTime = Time.time;
            scaleRate = 12.0f;
            transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
            StartCoroutine(Decay());
        }
        
        transform.localScale += new Vector3(scaleRate / (1 + Mathf.Pow(4*(Time.time - startTime), 2)), scaleRate / (1+ Mathf.Pow(4*(Time.time - startTime), 2)), scaleRate / (1+ Mathf.Pow(4*(Time.time - startTime), 2))) * Time.deltaTime;

        if((Time.time - startTime) >= diffusionTime)
        {
            dadEffect.deleting = true;
        }

        if(deleting == true)
        {
            dadEffect.deleting = true;
        }
    }

    IEnumerator Decay()
    {
        while (true)
        {
            //This is the original line of code. It scaled the wrong way, but that might be more fun for gameplay.
            //heatMult = 1 - Mathf.Lerp(0, 1, 1 - (Mathf.Pow((Time.time - startTime), 2)/ Mathf.Pow(diffusionTime, 2)));

            //This is the replacement line. It rapidly loses heat in an approximation of thermodynamic principles.
            //Albeit the scale is less dramatic.
            heatMult = 1 - Mathf.Lerp(0, 1, (Mathf.Pow(diffusionTime - (Time.time - startTime), 2)/ Mathf.Pow(diffusionTime, 2)));
            //Debug.Log(heatNow);
            yield return null;

            heatNow = heatCapacity - (heatMult * heatCapacity);
            //Debug.Log(heatNow);
        }
    }
}
