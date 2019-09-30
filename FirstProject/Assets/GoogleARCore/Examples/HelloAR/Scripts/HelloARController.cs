﻿//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.HelloAR
{
    using System.Collections.Generic;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using GoogleARCore.Examples.ObjectManipulation;
    using UnityEngine;
    using UnityEngine.EventSystems;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class HelloARController : Manipulator
    {
        public static HelloARController instance;

        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR
        /// background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// The rotation in degrees need to apply to model when the Andy model is placed.
        /// </summary>
        private const float k_ModelRotation = 180.0f;

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error,
        /// otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

        public GameObject placeObjectManipulatorPrefab;

        public GameObject HorizontalPlanePrefab { get; set; }

        /// <summary>
        /// The Unity Awake() method.
        /// </summary>
        public void Awake()
        {
            //Check if instance already exists
            if (instance == null)
            {
                //if not, set instance to this
                instance = this;

            }


            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);

            // Enable ARCore to target 60fps camera capture frame rate on supported devices.
            // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
            Application.targetFrameRate = 60;
        }

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public new void Update()
        {
            _UpdateApplicationLifecycle();
        }

        protected override bool CanStartManipulationForGesture(TapGesture gesture)
        {
            Debug.Log("CanStartManipulationForGesture true");

            if (gesture.TargetObject == null)
            {
                Debug.Log("CanStartManipulationForGesture");

                return false;
            }


            Debug.Log("OnStartManipulation : " + gesture.TargetObject.transform.position);

            // Raycast against the location the player touched to search for planes.
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon;

            if (Frame.Raycast(
                gesture.StartPosition.x, gesture.StartPosition.y, raycastFilter, out hit))
            {
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
                    // Choose the Andy model for the Trackable that got hit.
                    GameObject prefab = HorizontalPlanePrefab;

                    //Debug.Log("unity test  gesture.TargetObject.transform.position : " + gesture.TargetObject.transform.position);
                    //Debug.Log("unity test  gesture.TargetObject.transform : " + gesture.TargetObject.transform);

                    

                    var manipulator =
                        Instantiate(placeObjectManipulatorPrefab);

                    var placeObject =
                        Instantiate(prefab);

                    // Create an anchor to allow ARCore to track the hitpoint as understanding of
                    // the physical world evolves.
                    //var anchor = hit.Trackable.CreateAnchor(new Pose(gesture.TargetObject.transform.position, Quaternion.identity));

                    //var anchor = hit.Trackable.CreateAnchor(new Pose(GetNearestPointOnGrid(hit.Pose.position, DetectedPlaneVisualizer.Gab), Quaternion.identity));

                    // Make Andy model a child of the manipulator.
                    //placeObject.transform.parent = manipulator.transform;

                    placeObject.transform.SetParent(manipulator.transform);

                    manipulator.transform.SetParent(gesture.TargetObject.transform);

                    manipulator.transform.transform.localPosition = new Vector3(0, 0, 0);

                    placeObject.transform.localPosition = new Vector3(0,0,0);

                    // Make manipulator a child of the anchor.
                    //gesture.TargetObject.transform.parent = manipulator.transform;

                    placeObject.transform.rotation = gesture.TargetObject.transform.rotation;

                    print("gggeture 타게사삿 : " + gesture.TargetObject.transform);

                    // Select the placed object.
                    manipulator.GetComponent<Manipulator>().Select();

                    //StartCoroutine(CustomAnimationCurve.Instance.TempAnimation(placeObject));

                    if (gesture.TargetObject.transform.name == "bottom_face")
                    {
                        placeObject.transform.localPosition = Vector3.forward * 5 + Vector3.left * -5;
                    }
                    if (gesture.TargetObject.transform.name == "forward_face")
                    {
                        placeObject.transform.localPosition = Vector3.up * -5 + Vector3.left * -5;
                    }
                    if (gesture.TargetObject.transform.name == "left_face")
                    {
                        placeObject.transform.localPosition = Vector3.forward * -5 + Vector3.up * 5;
                    }

                    if (gesture.TargetObject.transform.name == "top_face")
                    {
                        placeObject.transform.localPosition = Vector3.forward * -5 + Vector3.left * 5;
                    }

                    if (gesture.TargetObject.transform.name == "back_face")
                    {
                        placeObject.transform.localPosition = Vector3.up * -5 + Vector3.left * 5;
                    }
                    if (gesture.TargetObject.transform.name == "right_face")
                    {
                        placeObject.transform.localPosition = Vector3.forward * 5 + Vector3.up * 5;
                    }
                }
            }
            return false;
        }

        protected override void OnStartManipulation(TapGesture gesture)
        {


            if (gesture.WasCancelled)
            {
                Debug.Log("WasCancelled");

                return;
            }

            // If gesture is targeting an existing object we are done.
            if (gesture.TargetObject != null)
            {
                Debug.Log("TargetObject : " + gesture.TargetObject.transform);

                return;
            }


        }



        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                Screen.sleepTimeout = SleepTimeout.SystemSetting;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to
            // appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage(
                    "ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity =
                unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject =
                        toastClass.CallStatic<AndroidJavaObject>(
                            "makeText", unityActivity, message, 0);
                    toastObject.Call("show");
                }));
            }
        }
        public Vector3 GetNearestPointOnGrid(Vector3 position, Vector3 gab)
        {
            position -= transform.position;

            float xPosition = Mathf.Round(position.x * 1000) * 0.001f;
            //float yPosition = Mathf.Round(position.y / size);
            float zPosition = Mathf.Round(position.z * 1000) * 0.001f;

            Vector3 result = new Vector3(
                (float)xPosition + gab.x,
                position.y,
                (float)zPosition + gab.z);

            return result;
        }
    }
}
