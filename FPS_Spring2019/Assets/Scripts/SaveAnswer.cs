using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAnswer : MonoBehaviour
{
    private ParticipantStatus ps;

    // Use this for initialization
    void Start()
    {
        ps = ParticipantStatus.GetInstance();
        ps.IncTrial();
    }


    public void OnTriggerExit(Collider other)
    {
        ps.GetDataFarmer().Save(new DFAnswerSelection());
        ps.IncTrial();
    }
}
