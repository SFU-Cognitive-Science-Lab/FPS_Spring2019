using UnityEngine;

public class GetChoice : MonoBehaviour
{
    public string choice;
    private ParticipantStatus ps = ParticipantStatus.GetInstance();

    void OnTriggerExit(Collider other)
    {
        if (ps.SetChoice(choice))
        {
            bool saveme = true;
            ps.GetDataFarmer().Save(new DFAnswerSelection(), saveme);
            Debug.Log(string.Format("Answer selected: {0}", ps.GetLastChoice()));
        }
    }
}
