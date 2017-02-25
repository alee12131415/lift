﻿using UnityEngine;
using System.Collections;
using Edwon.VR.Input;
using Edwon.VR.Gesture;

namespace Edwon.VR.Gesture
{
    public class GestureList : MonoBehaviour
    {

        VRGestureRig rig;
        IInput input;

        Transform playerHead;
        Transform playerHandL;
        Transform playerHandR;

        private string lastGesture;

        void Start()
        {
            rig = FindObjectOfType<VRGestureRig>();
            if (rig == null)
            {
                Debug.Log("there is no VRGestureRig in the scene, please add one");
            }

            playerHead = rig.head;
            playerHandR = rig.handRight;
            playerHandL = rig.handLeft;

            input = rig.GetInput(rig.mainHand);
        }

        void OnEnable()
        {
            Debug.Log("now working");
            GestureRecognizer.GestureDetectedEvent += OnGestureDetected;
        }

        void OnDisable()
        {
            Debug.Log("now not working");
            GestureRecognizer.GestureDetectedEvent -= OnGestureDetected;
        }

        public void resetGesture()
        {
            lastGesture = null;
        }

        public string getGesture()
        {
            return lastGesture;
        }

        void OnGestureDetected(string gestureName, double confidence, Handedness hand, bool isDouble)
        {
            string confidenceString = confidence.ToString().Substring(0, 4);
            //Debug.Log("detected gesture: " + gestureName + " with confidence: " + confidenceString);
         
            lastGesture = gestureName;
           

            //Debug.Log(lastGesture);

            switch (lastGesture)
            {
                case "yes":
                    Debug.Log("Yes Works!");
                    break;
                case "no":
                    Debug.Log("No Works!");
                    break;
                case "pat":
                    Debug.Log("Pat Works!");
                    break;
                case "rude":
                    Debug.Log("Rude Works!");
                    break;
                case "agree":
                    Debug.Log("agree Works!");
                    break;
                case "disagree":
                    Debug.Log("disagree Works!");
                    break;
                case "salute":
                    Debug.Log("salute Works!");
                    break;
                case "maybe":
                    Debug.Log("maybe Works!");
                    break;
                case "confused":
                    Debug.Log("confused Works!");
                    break;
                case "continue":
                    Debug.Log("continue Works!");
                    break;
            }
        }
    }
}