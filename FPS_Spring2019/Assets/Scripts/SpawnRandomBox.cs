using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*[public method to spawn boxes.]
 * 
 * Author: Rollin Poe(VR version), Atticus
 * Created: October 2018
 * Last Edit: December 2018
 * 
 * Cognitive Science Lab, Simon Fraser University
 * Originally Created for: FPS_Spring2019
 * 
 * Called by 
 */

public class SpawnRandomBox : MonoBehaviour {

    public Transform boxPrefab;
    public Transform spawnPoint;

    public void spawn()
    {
        if (boxPrefab != null && spawnPoint != null)
        {
            Transform randomBox = Instantiate(boxPrefab);
            randomBox.position = spawnPoint.position;
            randomBox.rotation = spawnPoint.rotation * Quaternion.Euler(60, 0, 45);
            randomBox.RotateAround(randomBox.position, randomBox.right, 0);

        }

    }
}
