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
* Change materials of "this".
*/

public class ChangeMaterial : MonoBehaviour {

    public Material correctMat, wrongMat, neutralMat, nextMat, noChoiceMat;

    public void SetCorrect()
    {
        this.GetComponent<Renderer>().material = correctMat;
    }

    public void SetWrong()
    {
        this.GetComponent<Renderer>().material = wrongMat;
    }

    public void SetNeutral()
    {
        this.GetComponent<Renderer>().material = neutralMat;
    }

    public void SetNext()
    {
        this.GetComponent<Renderer>().material = nextMat;
    }

    public void SetNoChoice()
    {
        this.GetComponent<Renderer>().material = noChoiceMat;
    }
}
