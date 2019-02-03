using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPlaceholder : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<InputField>().text = ParticipantStatus.GetInstance().GetParticipantAsString();
    }

}
