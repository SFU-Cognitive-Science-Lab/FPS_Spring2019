using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text.RegularExpressions;


public class DataExtractor : MonoBehaviour
{
    // Declare our variables 
    public string HMDxPos_string;
    string HMDyPos_string;
    string HMDzPos_string;
    string HMDxRotate_string;
    string HMDyRotate_string;
    string HMDzRotate_string;
    string ObjUnderReticle;
    private int writetofile = 0;
    private int DataCacheThreshhold = 1000;
    private int HeadersCount = 3;
    private float distanceTravelled = 0;
    private Vector3 previousPosition;
    private double displacement;
    private double rotations;
    private double framecount = 0;

    // Use this for initialization
    void Start()
    {
        // Hey Cal. Since this runs only on the initial startup of the program, the distance travelled variable
        // is actually better described as net displacement from origin.
        // If we were wanting to grab the distance between fixations, we would need a different calculation - RCAB
        previousPosition = transform.position;

    }

    // class DataStructure
    // Update is called once per frame
    void Update()
    {
        framecount++;
        if (framecount % 100 == 0)
        {
            distanceTravelled += Vector3.Distance(transform.position, previousPosition);
            DataFarmer.GetInstance().Save(new DFPositionRotation(transform.position, transform.rotation, distanceTravelled));
            previousPosition = transform.position;
        }

        // Grab Vector3 Coordinates and Convert into string
        HMDxPos_string = transform.position.x + " ";
        HMDyPos_string = transform.position.y + " ";
        HMDzPos_string = transform.position.z + " ";
        HMDxRotate_string = transform.rotation.x + " ";
        HMDyRotate_string = transform.rotation.y + " ";
        HMDzRotate_string = transform.rotation.z + " ";
        ObjUnderReticle = " ";

        // Debug.Log(xstring + ystring + zstring);

        // Declare the array and insert elements on each update:

        var array = new List<string>();

        array.Add(HMDxPos_string);
        array.Add(HMDyPos_string);
        array.Add(HMDzPos_string);
        array.Add(HMDxRotate_string);
        array.Add(HMDyRotate_string);
        array.Add(HMDzRotate_string);
        var array2string = string.Join(Environment.NewLine, array.ToArray());

        String pattern = @"\s ";
        String[] data2csv = Regex.Split(array2string, pattern);
        //foreach (var element in data2csv)
        // Debug.Log(data2csv);


        string csv2file = String.Join(", ", data2csv);
        // Debug.Log(csv2file);

        // Add increment to Data Threshold Counter
        writetofile++;

        if (writetofile == DataCacheThreshhold)
        {
            // File.WriteAllLines(@"C:\Users\CSLUser\Desktop\WriteLines.csv", array.ConvertAll(Convert.ToString));
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\CSLUser\Desktop\WriteLines.csv", true))
            {
                // Adds array to data file on desktop
                file.WriteLine(csv2file);
            }
            // Declare that data has been sent to file
            Debug.Log("Data Cache Threshold has been reached");

            //Reset Data Threshold Counter
            writetofile = 0;
        }
    }

}