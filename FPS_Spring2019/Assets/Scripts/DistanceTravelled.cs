using UnityEngine;

public class DistanceTravelled : MonoBehaviour
{
    private Vector3 previousPosition;
    private float distanceTravelled;
    public string thing;

    // Use this for initialization
    void Start()
    {
        previousPosition = transform.position;
        distanceTravelled = 0;
    }

    // Update is called once per frame
    // keeping the cumulative displacement as this vs time might be useful later
    // i.e. calculating velocity and acceleration over the entire run
    void Update()
    {
        distanceTravelled += Vector3.Distance(transform.position, previousPosition);
        previousPosition = transform.position;
        ParticipantStatus.GetInstance().UpdateDisplacement(thing, distanceTravelled);
    }
}
