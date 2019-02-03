using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* 
* Author: Atticus
* Created: December 2018
* Last Edit: December 2018
* 
* Cognitive Science Lab, Simon Fraser University
* Originally Created for: FPS_Spring2019
* 
* When click on cube and drag, the cube rotates.
*/


public class RotateCube : MonoBehaviour {

    private Camera cam;
    bool validBegin;
    private float axisX;//the distance mouse moves in X axis
    private float axisY;//the distance mouse moves in Y axis
    private Vector3 startPos;//mouse's position when click the button
    private Vector3 nowPos;//mouse's positon at current frame
    private Vector3 latePos;//mouse's position at last frame
    Transform targetTransform;//the object pointed by mouse
    private float tarRayLength = Mathf.Infinity;

    private void Start()
    {
        axisX = 0;
        axisY = 0;
        targetTransform = null;
        validBegin = false;
        cam = GameObject.FindGameObjectWithTag("Camera").GetComponent<Camera>() as Camera;
    }

    void Update ()
    {
        float hori = 0;
        float verti = 0;
        verti = Input.GetAxis("Control_Vert");
        hori = Input.GetAxis("Control_Hori");
        if(Mathf.Abs(verti)>0 || Mathf.Abs(hori) > 0)
        {
            axisX = -hori* 3f;
            axisY = verti * 3f;
            this.transform.Rotate(new Vector3(axisY, axisX, 0), Space.World);
        }



        ////parameter 0 refers to left button
        //if (Input.GetMouseButtonDown(0))
        //{
        //    startPos = Input.mousePosition;
        //    validBegin = false;
        //    //if mouse points to target cube when left button is clicked, rotation is valid
        //    validBegin = TarRayCast();
        //}
        //if (validBegin && Input.GetMouseButton(0))
        //{
        //    nowPos = Input.mousePosition;
        //    if (nowPos != startPos)
        //    {
        //        if (nowPos != latePos)
        //        {
        //            axisX = -(nowPos.x - startPos.x) * Time.deltaTime;
        //            axisY = (nowPos.y - startPos.y) * Time.deltaTime;
        //        }
        //        else
        //        {
        //            axisX = 0;
        //            axisY = 0;
        //        }
        //    }
        //    else
        //    {
        //        axisX = 0;
        //        axisY = 0;
        //    }
        //}
        //else
        //{
        //    axisX = 0;
        //    axisY = 0;
        //}
        //this.transform.Rotate(new Vector3(axisY, axisX, 0), Space.World);	
	}

    void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            latePos = Input.mousePosition;
        }
    }

    //checks if mosue points to target cube
    public bool TarRayCast()
    {
        nowPos = Input.mousePosition;
        targetTransform = null;
        if (cam != null)
        {
            RaycastHit hitInfo;
            Ray ray = cam.ScreenPointToRay(new Vector3(nowPos.x, nowPos.y, 0f));
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo, tarRayLength))
            {
                targetTransform = hitInfo.collider.transform;
            }
        }

        if(this.transform==targetTransform)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
