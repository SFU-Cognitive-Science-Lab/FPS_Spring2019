using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnObject : MonoBehaviour
{
    /*[public method to despawn boxes.]
    * 
    * Author: Rollin Poe, Atticus
    * Created: December 2018
    * Last Edit: December 2018
    * 
    * Cognitive Science Lab, Simon Fraser University
    * Originally Created for: VR_Fall2018_1
    * 
    * Attached to next button in unity editor. Triggers On Hand Hover Begin()
    */

    public void buttonDespawn()
    {
        GameObject[] currentObjs = GameObject.FindGameObjectsWithTag("Interactable Object");

        foreach (GameObject cube in currentObjs)
        {

            //Debug.Log("Destroyed " + cube);
            Destroy(cube);
        }

    }

}
