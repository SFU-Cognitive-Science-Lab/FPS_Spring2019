using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonDoStuff : MonoBehaviour
{

    public Button apply;
    // public InputField participantID;
    public InputField cubeset;
    public InputField arrangement;
    public GameObject menu;
    public GameObject controInfo;

    private ParticipantStatus ps = ParticipantStatus.GetInstance();

    // Use this for initialization
    // for the ui to work properly the canvas must use Render Mode "Screen Space - Overlay"
    // this is causing a warning to show up in unity's editor but this has no effect as 
    // we don't want the participant to see the UI
    void Start()
    {
        apply.onClick.AddListener(SetParticipantCondition);
        // Cal says: all this is going to be done manually
        // participantID.text = ps.GetParticipantAsString();
        // ps.ConditionFromParticipant();
        // ParticipantStatus.Condition cond = ps.GetCondition();
        cubeset.text = "";
        arrangement.text = "";
    }

    // in order for this to work we need an EventSystem component 
    void SetParticipantCondition()
    {
        ps.SetCondition(int.Parse(cubeset.text), int.Parse(arrangement.text)).BuildParticipantFromCondition();
        if (ps.GetParticipant() > 0)
        {
            menu.SetActive(false);
            controInfo.SetActive(true);
            Debug.Log("condition " + ps.GetCondition() + " for " + ps.GetParticipant());
        }
    }

}
