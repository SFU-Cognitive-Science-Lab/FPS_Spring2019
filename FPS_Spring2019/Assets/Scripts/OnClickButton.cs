using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* 
* Author: Atticus
* Created: December 2018
* Last Edit: December 2018
* 
* Cognitive Science Lab, Simon Fraser University
* Originally Created for: FPS_Spring2019
* 
* Handle the click event on selections A B C D.
*/


public class OnClickButton : MonoBehaviour {
    
    public Camera cam;
    public GameObject gameManager;
    public string choice;

    private Vector3 nowPos;//mouse's positon at current frame
    Transform targetTransform;//the object pointed by mouse
    private float tarRayLength = Mathf.Infinity;
    bool everChoose;
    float value;
    string buttonName;

    // Use this for initialization
    void Start ()
    {
        everChoose = false;
        value = 0;
        buttonName = "Chose" + this.GetComponent<CustomTag>().getTag(0);
    }
	
	// Update is called once per frame
	void Update () {
        if(buttonName == "ChoseNext")
        {
            value = Input.GetAxis("ChoseNEXT");
            if (value > 0.5 && !everChoose)
            {
                gameManager.GetComponent<ChoiceBehavior>().Clicked(this.GetComponent<CustomTag>().getTag(0));
                everChoose = true;
            }
            else if (value < 0.5 && everChoose)
            {
                everChoose = false;
            }
            else
            {
                ;
            }
        }
        else
        {
            value = Input.GetAxis(buttonName);
            if (value > 0.5 && !everChoose)
            {
                gameManager.GetComponent<ChoiceBehavior>().Clicked(this.GetComponent<CustomTag>().getTag(0));
                everChoose = true;
            }
            else if (value < 0.5 && everChoose)
            {
                everChoose = false;
            }
            else
            {
                ;
            }
        }




	}

    
}
