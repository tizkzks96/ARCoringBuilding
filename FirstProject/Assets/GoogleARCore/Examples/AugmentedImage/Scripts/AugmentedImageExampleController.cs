//-----------------------------------------------------------------------
// <copyright file="AugmentedImageExampleController.cs" company="Google">
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

namespace GoogleARCore.Examples.AugmentedImage
{
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using GoogleARCore;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Controller for AugmentedImage example.
    /// </summary>
    /// <remarks>
    /// In this sample, we assume all images are static or moving slowly with
    /// a large occupation of the screen. If the target is actively moving,
    /// we recommend to check <see cref="AugmentedImage.TrackingMethod"/> and
    /// render only when the tracking method equals to
    /// <see cref="AugmentedImageTrackingMethod"/>.<c>FullTracking</c>.
    /// See details in <a href="https://developers.google.com/ar/develop/c/augmented-images/">
    /// Recognize and Augment Images</a>
    /// </remarks>
    public class AugmentedImageExampleController : MonoBehaviour
    {
        public static AugmentedImageExampleController instance;

        //test
        public GameObject imageCropperController;
        public Texture croppedImageHolder;
        //////////////////

        /// <summary>
        /// A prefab for visualizing an AugmentedImage.
        /// </summary>
        public AugmentedImageVisualizer AugmentedImageVisualizerPrefab;

        public GameObject CropRenderCanvasPrefab;

        /// <summary>
        /// The overlay containing the fit to scan user guide.
        /// </summary>
        public GameObject FitToScanOverlay;

        private Dictionary<int, bool> m_Visualizers
            = new Dictionary<int, bool>();

        private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();

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

        public void OnEnable()
        {
            FitToScanOverlay.SetActive(true);
        }
        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
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

            //if (m_TempAugmentedImages == null)
            //{
            //    m_TempAugmentedImages = new List<AugmentedImage>();
            //    Debug.Log("Unity Image - " + "m_TempAugmentedImages null check: " + m_TempAugmentedImages);
            //}

            // Get updated augmented images for this frame.
            Session.GetTrackables<AugmentedImage>(
                m_TempAugmentedImages, TrackableQueryFilter.Updated);

            // Create visualizers and anchors for updated augmented images that are tracking and do
            // not previously have a visualizer. Remove visualizers for stopped images.
            foreach (var image in m_TempAugmentedImages)
            {
                bool visualizer = false;

                m_Visualizers.TryGetValue(image.DatabaseIndex, out visualizer);
                if (image.TrackingState == TrackingState.Tracking && visualizer == false)
                {
                    Debug.Log("unity test 11111");
                    FitToScanOverlay.SetActive(false);
                    //StartCoroutine(ImageCropperNamespace.ImageCropperController.instance.SnapShot(image));

                    //수정했으니 변경해야됨 주석처리 지우자!!!!!
                    //ImageCropperNamespace.ImageCropperController.instance.Crop(image);



                    // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                    Anchor anchor = image.CreateAnchor(image.CenterPose);

                    //test
                    Instantiate(imageCropperController, anchor.transform);

                    Texture2D screenshot = new Texture2D((int)size.x, (int)size.y, TextureFormat.RGB24, false);

                    screenshot.ReadPixels(new Rect(Screen.width / 2 - size.x / 2, Screen.height / 2 - size.y / 2, size.x, size.y), 0, 0);
                    //Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

                    //screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                    screenshot.Apply();

                    croppedImageHolder.texture = screenshot;

                    //////////////////////////////////////////////////


                    m_TempAugmentedImages.Clear();
                    Debug.Log("Unity Image - " + "★m_TempAugmentedImages null check: " + m_TempAugmentedImages);

                    m_Visualizers.Add(image.DatabaseIndex, true);




                }
                else if (image.TrackingState == TrackingState.Stopped && visualizer != null)
                {
                    m_Visualizers.Remove(image.DatabaseIndex);
                }
            }

            //// Show the fit-to-scan overlay if there are no images that are Tracking.
            //foreach (var visualizer in m_Visualizers.Values)
            //{
            //    if (visualizer.Image.TrackingState == TrackingState.Tracking)
            //    {
            //        FitToScanOverlay.SetActive(false);
            //        return;
            //    }
            //}

            FitToScanOverlay.SetActive(true);
        }
    }
}
