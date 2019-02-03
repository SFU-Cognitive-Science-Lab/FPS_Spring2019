using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CasterofRays : MonoBehaviour
{
    // create a variable which allows us to specify which layers are interacted with
    public bool debug;
    public LayerMask mask;
    public static string ObjUnderReticle;
    // our current set up does update 90 times per second - use debug = true to recalibrate
    public static readonly int MAXOBJECTS = 90;
    private static int last = 0;
    private static string[] RecentObjects = new string[MAXOBJECTS];
    private string HitInfoString;
    private float lastUpdate = 0;
    private double frameCount = 0;

    private void Start()
    {
        lastUpdate = 0;
        frameCount = 0;
    }


    void Update()
    {
        // Cal: sanity check to see if MAXOBJECTS makes sense
        if (debug)
        {
            frameCount++;
            if (lastUpdate >= 0)
            {
                if (Time.time - lastUpdate >= 1)
                {
                    //Debug.Log(string.Format("Frame count was {0} at {1}", frameCount, lastUpdate));
                    lastUpdate = Time.time;
                    frameCount = 0;
                }
            }
        }

        // Updates the position of the player camera
        Ray ray = new Ray(transform.position, transform.forward);

        // Creates a variable for extracting info about the object hit by the ray
        RaycastHit HitInfo;

        // 4 layers to the raycast: when the ray goes out, the object hit must be within 100 meters, and must be on the specified layers ("mask")
        if (Physics.Raycast(ray, out HitInfo, 100, mask))
        {
            // Extract info about the object hit - maybe feed this into csv or table from here.
            // Cal: See FindClosestSide.cs 
            // print (HitInfo.collider.gameObject.name);
            //Debug.DrawLine(ray.origin, HitInfo.point, Color.red);
            AddObject(string.Format("CoR: {0}", HitInfo.collider));
        }
        else
        {
            //Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.green);
        }

        HitInfoString = HitInfo.collider + "";
        // Debug.Log(HitInfoString);
    }

    // because the last thing we looked at might be noise, save a sample of recent things we looked at
    // we assume that this gets invoked frequently (e.g. in the Update callback)
    private static void AddObject(string Obj)
    {
        RecentObjects[last] = Obj;
        last++;
        if (last == MAXOBJECTS) last = 0;
    }

    // this scans what we recently looked at and returns the object associated with the mode
    public static string MostCommonObject()
    {
        var map = new Dictionary<string, int>();
        int count = 0;
        for (int i = 0; i < MAXOBJECTS; i++)
        {
            var o = RecentObjects[i];

            if (map.ContainsKey(o)) count = map[o];
            else count = 0;

            count++;
            map[o] = count;
        }
        string mode = "";
        int modeVal = 0;
        foreach (var pair in map)
        {
            if (pair.Value > modeVal)
            {
                modeVal = pair.Value;
                mode = pair.Key;
            }
        }
        return mode;
    }
}

//        // Bit shift the index of the layer (8) to get a bit mask
// int layerMask = 1 << 8;

// This would cast rays only against colliders in layer 8.
// But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
// layerMask = ~layerMask;

// RaycastHit hit;
//        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
//        {
//            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
// Debug.Log("Did Hit");
//            Debug.Log(transform.name);
//        }
//        else
//       {
//           Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
//          Debug.Log("Did not Hit");
//       }