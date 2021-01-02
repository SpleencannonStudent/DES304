using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene1to2 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided");
        if (other.tag == "Player")
        {
            Debug.Log("Hi!");
            SceneManager.LoadScene("BigSceneNoBugs");
        }
    }
}
