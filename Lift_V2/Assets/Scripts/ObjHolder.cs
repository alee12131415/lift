﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjHolder : MonoBehaviour {

	// Placed on objects that are interactable holders

	public static bool inRange;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
			
	}

	private void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "grabPoint"){
			inRange = true;
		}
	}

	private void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "grabPoint") {
			inRange = false;
		}
	}
}
