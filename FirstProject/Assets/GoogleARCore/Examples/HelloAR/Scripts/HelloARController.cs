//-----------------------------------------------------------------------
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


        [SerializeField]
        private bool menuOpened;

        public GameObject popupCanvas;

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

        private void Start()
        {
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



            if (gesture.TargetObject == null)
            {
                Debug.Log("CanStartManipulationForGesture");
                print("close menu");


                return false;
            }
            else if(gesture.TargetObject.transform.tag == "AnotherGame" && ObjectPlaceUIManager.instance.spotSquare.activeSelf == false)
            {
                if (popupCanvas.activeSelf == false)
                {
                    popupCanvas.SetActive(true);
                }
            }
            else if(gesture.TargetObject.transform.tag == "Building")
            {
                print("gesture.TargetObject.transform.tag : " + gesture.TargetObject.transform.tag);
                gesture.TargetObject.GetComponent<Manipulator>().Select();

                return true;
            }
            else
            {
                ObjedtPlaceObjectController(gesture);

                return true;
            }

            //PiUiController(gesture);

            //PlaceObject(gesture);
            //// Raycast against the location the player touched to search for planes.
            //TrackableHit hit;
            //TrackableHitFlags raycastFilter = TrackableHitFlags.Default;

            //if (Frame.Raycast(
            //    gesture.StartPosition.x, gesture.StartPosition.y, raycastFilter, out hit))
            //{
            //    print("VVVVV : " + gesture.TargetObject);

            //    // Use hit pose and camera pose to check if hittest is from the
            //    // back of the plane, if it is, no need to create the anchor.
            //    if ((hit.Trackable is DetectedPlane) &&
            //        Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
            //            hit.Pose.rotation * Vector3.up) < 0)
            //    {
            //        Debug.Log("Hit at back of the current DetectedPlane");
            //    }
            //    else
            //    {
            //        print("AAAAA : " + gesture.TargetObject);


            //    }
            //}
            return false;
        }

        public void ObjedtPlaceObjectController(TapGesture gesture)
        {
            
            print("ObjedtPlaceObjectController run");

            ObjectPlaceUIManager.instance.StartPlace(gesture);

        }

        public bool PlaceObject(GameObject target, GameObject HorizontalPlanePrefab, bool _EnviromentCheck = false)
        {
            

            Debug.Log("unity test  gesture.TargetObject.transform.position : ");

            if (HorizontalPlanePrefab == null)
            {
                return false;
            }
            //Debug.Log("unity test  gesture.TargetObject.transform : " + gesture.TargetObject.transform);

            else
            {
                // Choose the Andy model for the Trackable that got hit.
                GameObject prefab = HorizontalPlanePrefab;

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

                GameObject selectionVisualization = manipulator.transform.Find("Selection Visualization").gameObject;

                selectionVisualization.transform.position = new Vector3(0, 0.01f, 0);

                selectionVisualization.transform.localScale = new Vector3(50, 0, 50);

                selectionVisualization.transform.SetParent(placeObject.transform);

                placeObject.transform.SetParent(manipulator.transform);

                manipulator.transform.SetParent(target.transform);

                manipulator.transform.transform.localPosition = Vector3.zero;

                manipulator.tag = placeObject.tag;

                placeObject.transform.localPosition = Vector3.zero;

                placeObject.transform.rotation = target.transform.rotation;

                manipulator.transform.SetParent(target.transform.parent.transform);

                manipulator.transform.localScale = Vector3.one * 0.5f;

                // Make manipulator a child of the anchor.
                //gesture.TargetObject.transform.parent = manipulator.transform;

                if (_EnviromentCheck == false)
                {
                    manipulator.GetComponent<Manipulator>().Select();
                }
                // Select the placed object.

                //StartCoroutine(CustomAnimationCurve.Instance.TempAnimation(placeObject));
                //TapGesturePositionCorrection(target, manipulator.transform, 5);

                HorizontalPlanePrefab = null;

                //추후 주석 풀기
                //BuildingMoneyCtrl bMC = placeObject.AddComponent<BuildingMoneyCtrl>();
                //bMC.placeObject = placeObject;
                //bMC.time = 5;
                //bMC.increseMoney = 5;

                placeObject.SetActive(true);

            }

            return true;
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
        
        public void TapGesturePositionCorrection(TapGesture gesture, Transform obejct, float height)
        {
            if (gesture.TargetObject.transform.name == "bottom_face")
            {
                obejct.transform.localPosition = Vector3.forward * -2.5f + Vector3.left * 2.5f + Vector3.up * height;
            }
            if (gesture.TargetObject.transform.name == "forward_face")
            {
                obejct.transform.localPosition = Vector3.forward * -2.5f + Vector3.left * 2.5f + Vector3.up * height;
            }
            if (gesture.TargetObject.transform.name == "left_face")
            {
                obejct.transform.localPosition = Vector3.forward * -2.5f + Vector3.left * 2.5f + Vector3.up * height;
            }

            if (gesture.TargetObject.transform.name == "top_face")
            {
                obejct.transform.localPosition = Vector3.forward * -2.5f + Vector3.left * 2.5f + Vector3.up * height;
            }

            if (gesture.TargetObject.transform.name == "back_face")
            {
                obejct.transform.localPosition = Vector3.forward * -2.5f + Vector3.left * 2.5f + Vector3.up * height;
            }
            if (gesture.TargetObject.transform.name == "right_face")
            {
                obejct.transform.localPosition = Vector3.forward * -2.5f + Vector3.left * 2.5f + Vector3.up * height;
            }
        }
    }
}
