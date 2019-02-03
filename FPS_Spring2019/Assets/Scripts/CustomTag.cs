using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTag : MonoBehaviour
{

    [SerializeField]
    private List<string> tags = new List<string>();

    public int count()
    {
        return tags.Count;
    }

    public List<string> getList()
    {
        return tags;
    }

    public string getTag(int index)
    {
        return tags[index];
    }

    /*[Methods to set and check (correct) tags for GameObjects]
    * 
    * Author: Rollin Poe
    * Created: October 2018
    * Last Edit: October 2018
    * 
    * Cognitive Science Lab, Simon Fraser University
    * Originally Created for: VR_Fall2018_1
    * 
    * Attached to each choice button in unity editor. Triggers On Hand Hover End()
    * call buttonTagBehaviour(), passing the choice as and argument (eg A, B, C, D);
    */

    public void setCorrect(string button)
    {
        /*Will receive button's result and test against object's tag
            if they match tags[1] is set to "CORRECT", otherwise it 
            will be set to "WRONG". To be read by data collector/printer
        */

        //Debug.Log("Tag is: " + tags[0] + " Choice was: " + button);
        if (tags[1] == "") // to prevent multiple guesses
        {
            if (tags[0] == button)
            {
                tags[1] = "CORRECT";
                //Debug.Log("CORECT");
            }
            else
            {
                tags[1] = "WRONG";
                //Debug.Log("WRONG");
            }
        }


    }

    public void buttonTagBehaviour(string choice)
    {
        //For use on choice buttons

        //Here we get a list of all the interactable objects (there should only be one)
        GameObject[] currentObjs = GameObject.FindGameObjectsWithTag("Interactable Object");

        foreach (GameObject cube in currentObjs)
        {
            //Call the object's set correct method
            cube.GetComponent<CustomTag>().setCorrect(choice);
        }
    }

    public bool hasTag(string tag)
    {
        return tags.Contains(tag);
    }
    public void setTag(int index, string value)
    {
        tags[index] = value;
    }

}