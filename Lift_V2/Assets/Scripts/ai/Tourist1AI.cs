﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tourist1AI : Agent {

	// Use this for initialization
	void Start () {
        filename = "1.2Tourist.json";
        Init();
        isEndNode = false;
        isExit = false;
        timer = currentNode.wait;
        Build();
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (list[listIndex]()) {
            listIndex += 1;
            isPlayed = false;
            setup = false;
            resetGesture();

            if (listIndex >= list.Count) listIndex = list.Count - 1;
        }
	}

    private void Build() {
        list.Add(() => waitStart());
        list.Add(() => playStart());
        list.Add(() => playNormal());
    }

    private delegate bool tEvent();
    private bool isPlayed = false;
    private List<tEvent> list = new List<tEvent>();
    private bool setup = false;
    private int listIndex = 0;
    private int state = 0; //0 = normal 1 = notfloor 2 = exit

    private bool waitStart() {
        if (enter()) return true;
        return false;
    }

    private bool playStart() {
        if (!isDoorOpen()) {
            currentNode = nodeDict[currentNode.noResponse];
            onNode = currentNode;
            return true;
        }

        say(true);

        if (timer <= 0) {
            timer = currentNode.wait;
            isPlayed = false;
            changeMood(currentNode.noResponseChange);
        }

        return false;
    }

    private bool playNormal() {
        if (!setup) {
            timer = onNode.wait;
            //stops repeat at an end node
            if (isEndNode && state == 0) {
                isPlayed = true;
            } else {
                say();
            }
            if (onNode.noResponse == onNode.name && onNode != notFloorNode && onNode.listen.Count < 1) isEndNode = true;
            setup = true;
        }

        //node transitions
        int nextState = getNextState();

        //if transition, move and execute
        if (state != nextState) {
            Debug.Log("FROM " + state + " TO " + nextState);
            state = nextState;
            return true;
        }

        return doState();
    }

    private int getNextState() {
        switch (state) {
            case 0:
                if (isDoorOpen() && getFloorNumber() != attributes.goal) {
                    onNode = notFloorNode;
                    return 1;
                }
                else if (isDoorOpen() && getFloorNumber() == attributes.goal) {
                    onNode = endNode;
                    return 2;
                }
                else return 0;
            case 1:
                if (!isDoorOpen()) {
                    onNode = currentNode;
                    return 0;
                }
                else return 1;
            case 2:
                return 2;
            default:
                throw new System.InvalidOperationException("ERROR STAE MUST BE 0 1 or 2");
        }
    }

    private bool doState() {
        switch (state) {
            case 0:
                if (onNode.listen.Contains(getGesture())) {
                    int index = onNode.listen.IndexOf(getGesture());
                    changeMood(onNode.change[index]);
                    currentNode = nodeDict[onNode.toNode[index]];
                    onNode = currentNode;
                    return true;
                } else if (timer <= 0) {
                    changeMood(onNode.noResponseChange);
                    currentNode = nodeDict[onNode.noResponse];
                    onNode = currentNode;
                    return true;
                }
                return false;
            case 1:
                if (timer <= 0) {
                    changeMood(onNode.noResponseChange);
                    return true;
                }
                return false;
            case 2:
                if (!isExit) {
                    exit();
                    isExit = true;
                }
                return false;
            default:
                throw new System.InvalidOperationException("ERROR STAE MUST BE 0 1 or 2");
        }
    }

    private void say(bool start = false) {
        if (isPlayed) return;

        node play = onNode;

        if (start) play = currentNode;

        Debug.Log("I SAID SOMETHING");

        int index = 1; //neu
        if (attributes.mood < -3) index = 2; //neg
        else if (attributes.mood > 3) index = 0; //pos
        bubble.text = play.dialogue[index];

        string dialogue = play.dialogue[index];

        GameObject myObject = GameObject.Find("_SFX_" + lastSound);
        if (myObject != null) myObject.GetComponent<SoundGroup>().pingSound();
        dialogue.PlaySound(transform.position);
        lastSound = dialogue;

        isPlayed = true;
    }
}