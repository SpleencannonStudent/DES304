using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopParticles : MonoBehaviour
{
    [SerializeField] private GameObject partChild;
    private ParticleSystem particles;
    public bool timetrigger = false;
    private float counter = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        particles = partChild.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        //If the heat field is being deleted, the particle system stops emitting
        if(timetrigger == true)
        {
            Debug.Log("Culling timer");
            var main = particles.main;
            main.duration = counter;
            timetrigger = false;
        }
    }
}
