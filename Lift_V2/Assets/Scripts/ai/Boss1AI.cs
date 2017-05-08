﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1AI : Agent {

	void Start () {
        filename = "1.1Boss.json";
        Init();
        timer = nodeDict["Start"].wait;
        Build();
	}

    // Update is called once per frame
    void Update () {
        timer -= Time.deltaTime;

        if (list[listIndex]()) {
            listIndex += 1;
            //timer = currentNode.wait;
            isPlayed = false;
            playedFirst = false;
            setup = false;
        }

        if (timer <= 0) {
            timer = currentNode.wait;
            isPlayed = false;
        }
    }

    private void Build() {
        list.Add(() => playStart());

        list.Add(() => playFirstOrder());

        list.Add(() => playNormal(nodeDict["Day to day operations"]));

        list.Add(() => playNormal(nodeDict["The Guests"]));

        list.Add(() => playNormal(nodeDict["The first day"]));

        list.Add(() => playGoodStart());

        list.Add(() => playEnd());
    }

    //
    private delegate bool bossEvent();
    private bool isPlayed = false;
    private List<bossEvent> list = new List<bossEvent>();
    private bool playedFirst = false;
    private bool setup = false;
    private int listIndex = 0;

    private bool playStart() {
        if (enter()) return true;

        if (!setup) {
            currentNode = nodeDict["Start"];
            timer = currentNode.wait;
            setup = true;
        }

        if (playedFirst) currentNode = nodeDict["Still waiting"];

        say();
        playedFirst = true;
        return false;
    }

    private bool playFirstOrder() {
        if (!isDoorOpen()) return true;

        if (!setup) {
            currentNode = nodeDict["First order"];
            timer = currentNode.wait;
            setup = true;
        }

        if (isNearLever() && isDoorOpen() && !playedFirst) {
            currentNode = nodeDict["Ancient mechanism"];
            isPlayed = false;
            playedFirst = true;
            timer = currentNode.wait;
        }

        say();
        return false;
    }

    private bool playNormal(node n) {
        if (timer <= 0 && currentNode != notFloorNode) return true;

        if (!setup) {
            currentNode = n;
            timer = currentNode.wait;
            setup = true;
        }

        if (isDoorOpen() && attributes.goal != getFloorNumber() && !playedFirst) {
            isPlayed = false;
            currentNode = notFloorNode;
            timer = currentNode.wait;
            playedFirst = true;
        } else if (!isDoorOpen() && currentNode == notFloorNode) {
            currentNode = n;
            timer = currentNode.wait;
            playedFirst = false;
            isPlayed = false;
        }

        say();
        return false;
    }

    private bool playGoodStart() {
        if (isDoorOpen() && getFloorNumber() == attributes.goal) return true;

        if (!setup) {
            currentNode = nodeDict["Good start"];
            timer = currentNode.wait;
            setup = true;
        }

        if (isDoorOpen() && attributes.goal != getFloorNumber() && !playedFirst) {
            isPlayed = false;
            currentNode = notFloorNode;
            timer = currentNode.wait;
            playedFirst = true;
        }
        else if (!isDoorOpen() && currentNode == notFloorNode) {
            currentNode = nodeDict["Good start"];
            timer = currentNode.wait;
            playedFirst = false;
            isPlayed = false;
        }

        say();
        return false;
    }

    private bool playEnd() {

        currentNode = endNode;

        if (!playedFirst) {
            say();
            playedFirst = true;
            stopTalking();
            exit();
        }

        return false;
    }

    private void say() {
        if (isPlayed) return;

        int index = 1; //neu
        if (attributes.mood < -3) index = 2; //neg
        else if (attributes.mood > 3) index = 0; //pos

        //text
        //bubble.text = currentNode.dialogue[index];

        string dialogue = currentNode.dialogue[index];

        //animation
        if (currentNode.name != "End") animate(currentNode.animation[index]);

        GameObject myObject = GameObject.Find("_SFX_" + lastSound);
        if (myObject != null) myObject.GetComponent<SoundGroup>().pingSound();
        //dialogue.PlaySound(transform.position);
        pa.playDialogue(dialogue);

        //float test = GameObject.Find("_SFX_" + dialogue).GetComponent<AudioClip>().length;
        lastSound = dialogue;

        isPlayed = true;
    }
}
