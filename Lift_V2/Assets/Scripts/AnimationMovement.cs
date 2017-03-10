using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMovement : MonoBehaviour {

	private static GameObject targetWaypoint;
	static Animator anim;

	[Range(0.5f, 5)]
	public float walkSpeed = 10f;

	public bool moving = false;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		if (moving)
		{
			//float step = walkSpeed * Time.deltaTime;
			//transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.transform.position, step);
			//Start ANIMATION
			anim.SetBool("isWaiting", false);

			if (transform.position == targetWaypoint.transform.position)
			{
				// go into animator and trigger stop walking
				this.GetComponent<AnimationState>().enabled = false;

				//We've reached our destination
				GetComponent<PatronManager>().destinationReached();
				moving = false;
			}
		}
	}

	public void enterElevator(GameObject elevatorWaypoint)
	{
		targetWaypoint = elevatorWaypoint;
		moving = true;
	}

	public void leaveElevator(GameObject hotelWaypoint)
	{
		targetWaypoint = hotelWaypoint;
		moving = true;
	}

	public void wait()
	{

	}
}
