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
* When button is clicked, function is called by the clicked button. Then change buttons' material
*/


public class ChoiceBehavior : MonoBehaviour
{

    public List<GameObject> choices;
    public Material correctMat, wrongMat, neutralMat, nextMat, noChoiceMat;

    private bool firstChoice;
    private ParticipantStatus ps = ParticipantStatus.GetInstance();

    //called by buttons' OnClickButton 
    public void Clicked(string clickedTag)
    {
        if (clickedTag == "NEXT")
        {
            //Debug.Log("nex~~~~~");
            //collect data
            ps.IncTrial();
            //Debug.Log(string.Format("Starting trial {0} at {1}", ps.GetTrial(), Time.time));
            //Despawn current box
            this.GetComponent<DespawnObject>().buttonDespawn();
            for (int i = 0; i < choices.Count; i++)
            {
                if (choices[i].GetComponent<CustomTag>().getTag(0) != "NEXT")
                {
                    choices[i].GetComponent<Renderer>().material = noChoiceMat;
                }
                else
                {
                    choices[i].GetComponent<Renderer>().material = neutralMat;
                }
            }
            //Spawn in new box
            this.GetComponent<SpawnRandomBox>().spawn();
            firstChoice = true;
        }
        else
        {
            if (firstChoice == true)
            {
                //collect data
                if (ps.SetChoice(clickedTag))
                {
                    ps.GetDataFarmer().Save(new DFAnswerSelection());
                    //Debug.Log(string.Format("Answer selected: {0}", ps.GetLastChoice()));
                }

                firstChoice = false;
                GameObject[] currentObjs = GameObject.FindGameObjectsWithTag("Interactable Object");
                foreach (GameObject cube in currentObjs)
                {
                    string chosenTag = ParticipantStatus.GetInstance().GetCategory();
                    ParticipantStatus.GetInstance().SetChoice(chosenTag);

                    if (chosenTag == clickedTag)
                    {
                        for (int i = 0; i < choices.Count; i++)
                        {
                            if (choices[i].GetComponent<CustomTag>().getTag(0) != "NEXT")
                            {
                                if (choices[i].GetComponent<CustomTag>().getTag(0) != chosenTag)
                                {
                                    choices[i].GetComponent<Renderer>().material = neutralMat;
                                }
                                else
                                {
                                    choices[i].GetComponent<Renderer>().material = correctMat;
                                }
                            }
                            else
                            {
                                choices[i].GetComponent<Renderer>().material = nextMat;
                            }
                        }
                    }
                    else
                    {

                        for (int i = 0; i < choices.Count; i++)
                        {
                            if (choices[i].GetComponent<CustomTag>().getTag(0) != "NEXT")
                            {
                                if (choices[i].GetComponent<CustomTag>().getTag(0) != chosenTag)
                                {
                                    choices[i].GetComponent<Renderer>().material = neutralMat;
                                }
                                else
                                {
                                    choices[i].GetComponent<Renderer>().material = correctMat;
                                }
                                if (choices[i].GetComponent<CustomTag>().getTag(0) == clickedTag)
                                {
                                    choices[i].GetComponent<Renderer>().material = wrongMat;
                                }
                            }
                            else
                            {
                                choices[i].GetComponent<Renderer>().material = nextMat;
                            }
                        }
                    }
                }
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        firstChoice = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
