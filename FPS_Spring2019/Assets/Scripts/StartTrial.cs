using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrial : MonoBehaviour
{
    private ParticipantStatus ps = ParticipantStatus.GetInstance();

    public void OnTriggerExit(Collider other)
    {
        ps.IncTrial();
        Debug.Log(string.Format("Starting trial {0} at {1}", ps.GetTrial(), Time.time));
        ps.GetDataFarmer().Save(new DFAnswerSelection(DFAnswerSelection.START));
    }
}
