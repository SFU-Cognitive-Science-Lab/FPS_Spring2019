using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FindClosestSide : MonoBehaviour
{
    GameObject player;
    int RIGHT = 0, UP = 1, FORWARD = 2, NOSIDE = 3;
    string[] dirStrings = { "right", "up", "forward", "no side" };
    int previousSide;
    public int thresholdAngle = 56;
    public bool detailedLogging = false;
    public Text angleDisplay;
    public int measurements = 0;
    private ParticipantStatus ps = ParticipantStatus.GetInstance();

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Camera"); // this responds to head movements, Player doesn't
        //
        //Debug.Log("Player", player);
        previousSide = -1;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject cube = GameObject.FindGameObjectWithTag("Interactable Object");

        if (player != null && cube != null)
        {
            if (cube != null)
            {
                Transform transform = cube.GetComponent<Transform>();
                int[] angles = {
                    (int) Math.Abs(Math.Round(Vector3.Angle(player.transform.forward, transform.right)) - 90.0),
                    (int) Math.Abs(Math.Round(Vector3.Angle(player.transform.forward, transform.up)) - 90.0),
                    (int) Math.Abs(Math.Round(Vector3.Angle(player.transform.forward, transform.forward)) - 90.0),
                };
                int max = angles.Max();
                int visibleSide = NOSIDE;
                // basic idea: is the cube face angled towards us enough for us to see it?
                // we also want to check if the player's head is rotated up or to the side too far
                if (max > thresholdAngle)
                {
                    visibleSide = angles.ToList().IndexOf(max);
                }

                if (visibleSide != previousSide)
                {
                    // the last Vector3 is a placeholder for the cumulative movement of the head and hand controllers
                    DataFarmer.GetInstance().Save(
                        new DFFixation(
                            angles[UP], angles[FORWARD], angles[RIGHT],
                            dirStrings[visibleSide],
                            ps.DisplacementsToString()
                        )
                    );
                    previousSide = visibleSide;
                }
                if (detailedLogging)
                {
                    Debug.Log(
                        "correct answer is " + ps.GetCategory() + " " + ps.GetCube()
                           + "\nright " + player.transform.forward[RIGHT]
                           + " up " + player.transform.forward[UP]
                           + "\n" + dirStrings[UP] + " " + angles[UP]
                           + ", " + dirStrings[RIGHT] + " " + angles[RIGHT]
                           + ", " + dirStrings[FORWARD] + " " + angles[FORWARD]
                           + "\n" + dirStrings[visibleSide] + " is facing you");
                }
                measurements++;
            }
        }
    }
}
