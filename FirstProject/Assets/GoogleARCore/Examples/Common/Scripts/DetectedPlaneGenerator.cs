//-----------------------------------------------------------------------
// <copyright file="DetectedPlaneGenerator.cs" company="Google">
//
// Copyright 2018 Google Inc. All Rights Reserved.
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

namespace GoogleARCore.Examples.Common
{
    using System.Collections.Generic;
    using GoogleARCore;
    using UnityEngine;

    /// <summary>
    /// Manages the visualization of detected planes in the scene.
    /// </summary>
    public class DetectedPlaneGenerator : Singleton<DetectedPlaneGenerator>
    {
        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject DetectedPlanePrefab;

        /// <summary>
        /// A list to hold new planes ARCore began tracking in the current frame. This object is
        /// used across the application to avoid per-frame allocations.
        /// </summary>
        private List<DetectedPlane> m_NewPlanes = new List<DetectedPlane>();

        public List<DetectedPlane> NewPlanes { get => m_NewPlanes; set => m_NewPlanes = value; }

        public static bool tempBtnbool = true;
        public static void tempBtn()
        {
            if (tempBtnbool == false)
            {
                tempBtnbool = true;
            }
            else
            {
                tempBtnbool = false;
            }
        }

        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            // Check that motion tracking is tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                return;
            }

            // Iterate over planes found in this frame and instantiate corresponding GameObjects to
            // visualize them.
            Session.GetTrackables<DetectedPlane>(NewPlanes, TrackableQueryFilter.New);
            for (int i = 0; i < NewPlanes.Count; i++)
            {
                // Instantiate a plane visualization prefab and set it to track the new plane. The
                // transform is set to the origin with an identity rotation since the mesh for our
                    // prefab is updated in Unity World coordinates.
                GameObject planeObject =
                    Instantiate(DetectedPlanePrefab, Vector3.zero, Quaternion.identity, transform);
                Debug.Log("몇번 실행이 될까요? : " + NewPlanes.Count);

                planeObject.GetComponent<DetectedPlaneVisualizer>().Initialize(NewPlanes[i]);

                Debug.Log("몇번 실행이 될까요? : " + NewPlanes.Count);
                transform.GetComponent<DetectedPlaneGenerator>().enabled = false;
            }
        }
    }
}
