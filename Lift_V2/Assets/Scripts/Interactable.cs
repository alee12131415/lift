﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

	// TODO:
	// - turn off gesture component
	// - set them down in correct place depending on object

	// Placed on objects that are interactable
	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
	 
	[SerializeField]
	public SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device device { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

	public static bool inRangeID;
	public static bool inRangeHat;
	public static bool inRange;
	bool holding = false;
	static bool holdingID;
	static bool holdingHat;

	public static bool wearingID;
	public static bool wearingHat; 

	//public ItemSlot slot;
	//public enum ItemSlot {
	//	HAT,
	//	ID,
	//	GLASSES
	//}
	public GameObject thisObj;

	public GameObject hatHolder;
	public GameObject glassesHolder;
	public GameObject idHolder;

	public GameObject camRig;

	private bool nearObj;


	// Use this for initialization
	void Start () {
		//trackedObj = GetComponent<SteamVR_TrackedObject>();


		hatHolder = GameObject.FindGameObjectWithTag("hatHolder");
		glassesHolder = GameObject.FindGameObjectWithTag("glassesHolder");
		idHolder = GameObject.FindGameObjectWithTag("idHolder");
		camRig = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

		// pcik up when trigger is pressed
		if (device.GetPressDown (triggerButton) && (Vector3.Distance (trackedObj.transform.position, this.transform.position) < .2f)) {
			// parent object to controller
			DisableGesture.turnOff (camRig);
			this.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
			this.transform.SetParent (trackedObj.transform);
			holding = true;
		} else if (!(Vector3.Distance (trackedObj.transform.position, this.transform.position) < .17f)) {
			if (!DisableGesture.isComponentEnabled (camRig)) {
				DisableGesture.turnOn (camRig);
			}
		}

		// let's check to see what you're holding though 
		if (holding) {
			if (this.tag == "id")
				holdingID = true;
			if (this.tag == "hat")
				holdingHat = true;
		} else if (!holding) {
			if (this.tag == "id") 
				holdingID = false;
			if (this.tag == "hat") 
				holdingHat = false;
		}


		// if you are holding it and the trigger is released
		// check where the object is
		// if near end point, then go there

		if (holding && device.GetPressUp(triggerButton)) { // || near the end location??)
			// unless it is near the intended location,
			// the object will fall on the ground 
			switch (this.tag) {
				case "id":
				// allow an object to go on players torso
					if (Vector3.Distance (this.transform.position, idHolder.transform.position) < .3f) {
						holding = false;
						this.transform.SetParent (idHolder.transform);
						wearingID = true;
					} else {
						holding = false;
						dropObj (this.gameObject);
						wearingID = false;
					}
					break;

				case "hat":
				// allow an object to go on players head
					//parent to camera
					if (Vector3.Distance(this.transform.position, hatHolder.transform.position)<.3f) {
						holding = false;
						this.transform.SetParent(hatHolder.transform);
						wearingHat = true;
					}  else {
						holding = false;
						dropObj (this.gameObject);
						wearingHat = false;
					}
					break;

				default:
					//holding = false;
					break;
			}
		}
	}

	public void attemptGrab() {
		if (inRange) {
			nearObj = true;
			DisableGesture.turnOff (camRig);
			holding = true;

		} else {
			nearObj = false;
			if (!DisableGesture.isComponentEnabled (camRig)) {
				DisableGesture.turnOn (camRig);
			}
		}
	}

	public void attemptRelease() {
		holding = false;
	}

	private void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "grabPoint"){
			inRange = true;
		//	if (slot == ItemSlot.ID) 
		//		inRangeID = true;
		//	if (slot == ItemSlot.HAT)
		//		inRangeHat = true;
		}
	}

	private void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "grabPoint") {
			inRange = false;
			//if (slot == ItemSlot.ID) 
		//		inRangeID = false;
		//	if (slot == ItemSlot.HAT)
		//		inRangeHat = false;
		}
	}

	void dropObj(GameObject item) {
		// de parent from controller
		this.transform.parent = null;
		// turn on gravity
		this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
	}


	// FUNCTIONS TO DETECT IF OBJECTS ARE USED

	static bool isHoldingHat() {
		if (holdingHat) {
		//	Debug.Log ("Holding Hat");
			return true; 
		}			
		//Debug.Log ("not Holding hat");
		return false;
	}

	static bool isHoldingID() {
		if (holdingID) {
	//		Debug.Log ("Holding ID");
			return true;
		}
	//	Debug.Log ("not Holding ID");
		return false;
	}

	static bool isWearingHat() {
		if (wearingHat) {
		//	Debug.Log ("wearing hat");
			return true; 
		}
	//	Debug.Log ("not wearing hat");
		return false;
	}

	static bool isWearingID() {
		if (wearingID) {
		//	Debug.Log ("wearimg ID");
			return true;
		}
		//Debug.Log ("not wearing ID");
		return false;
	}
}