﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var value = Input.GetAxis("ChoseNEXT");
        Debug.Log("~~ACTION~~:" + value);
        ;

    }
}
