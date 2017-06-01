﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creditsController : MonoBehaviour {

    public float timeOnFloor;

    private int nextFloor = 1;
    private int maxFloor = 6;

    private doorInteraction liftDoor;
    private ElevatorMovement eleMvmt;

	// Use this for initialization
	void Start () {
        liftDoor = GameObject.FindGameObjectWithTag("door").GetComponent<doorInteraction>();
        eleMvmt = GameObject.FindGameObjectWithTag("ElevatorManager").GetComponent<ElevatorMovement>();

        StartCoroutine(CreditsCycle());
	}
	
	// Update is called once per frame
	void Update () {
        if (eleMvmt.floorPos == nextFloor) {
            if (nextFloor < maxFloor) {
                nextFloor++;
                StartCoroutine(CreditsCycle());
            }
            else {
                //We've reached end of credits
                liftDoor.openDoor();
            }
        }
    }

    IEnumerator CreditsCycle() {
        yield return new WaitForSeconds(1f);
        //Open door
        liftDoor.openDoor();

        //Wait for timeOnFloor
        yield return new WaitForSeconds(timeOnFloor);

        //Close the door
        liftDoor.closeDoor();

        yield return new WaitForSeconds(3f);

        //Set new target floor
        eleMvmt.newDoorTarget(nextFloor);
    }
}