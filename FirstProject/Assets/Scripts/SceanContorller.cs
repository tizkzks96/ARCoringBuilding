﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleARCore;
using System;
using GoogleARCore.Examples.Common;

public enum SceanState
{
    MAIN,
    AUGMENTEDIMAGE
}

public class SceanContorller : MonoBehaviour
{

    public ARCoreSession arCoreSession;

    public ARCoreSessionConfig augmentedImagesSessionConfig;
    public ARCoreSessionConfig defaultSessionConfig;

    public static SceanContorller instance;

    public GameObject Main;

    public GameObject AugmentedImage;

    private GameObject edtingSlot;
    public GameObject EdtingSlot { get => edtingSlot; set => edtingSlot = value; }

    public SceanState SceanState { get; set; }

    public void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;

            DontDestroyOnLoad(gameObject);
        }


        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    public void ChangeScean(SceanState sceanState)
    {
        switch (sceanState)
        {
            case SceanState.MAIN:
                Main.SetActive(true);

                AugmentedImage.SetActive(false);

                DetectedPlaneVisualizer.cubeWorld.SetActive(true);

                arCoreSession.SessionConfig = defaultSessionConfig;
                break;
            case SceanState.AUGMENTEDIMAGE:
                Main.SetActive(false);

                AugmentedImage.SetActive(true);

                DetectedPlaneVisualizer.cubeWorld.SetActive(false);

                arCoreSession.SessionConfig = augmentedImagesSessionConfig;

                break;
            default:
                break;
        }
    }
    
}
