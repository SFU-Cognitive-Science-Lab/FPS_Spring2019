using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
* 
* Author: Atticus
* Created: January 2019
* Last Edit: January 2019
* 
* Cognitive Science Lab, Simon Fraser University
* Originally Created for: FPS_Spring2019
* 
* Show instruction texts in turn and finally load main scene.
*/

public class ControllInfo : MonoBehaviour {
    public List<GameObject> texts;
    public List<string> buttons;

    private int textNumber; //index of text being shown
    private bool permitChoice;
    private int count;

	// Use this for initialization
	void Start ()
    {
        textNumber = 0;
        permitChoice = false;
        count = 0;
	}

    private void OnEnable()
    {
        texts[0].SetActive(true);
    }

    // Update is called once per frame
    void Update ()
    {
        count++;
        if(count==59)
        {
            count = 0;
        }
        if(count%60 == 0)
        {
            permitChoice = true;
        }
	    if(permitChoice==true)
        {
            if (textNumber < texts.Count)
            {
                var value = Input.GetAxis(buttons[textNumber]);
                if (value > 0.5)
                {
                    texts[textNumber].SetActive(false);
                    textNumber++;
                    if (textNumber == texts.Count)
                    {
                        SceneManager.LoadScene("Main", LoadSceneMode.Single);
                        this.enabled = false;
                    }
                    else
                    {
                        texts[textNumber].SetActive(true);
                        permitChoice = false;
                        count = 1;
                    }
                }
            }
        }
	}
}
