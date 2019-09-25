using GoogleARCore;
using GoogleARCore.Examples.ObjectManipulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceManipulator : Manipulator
{

    /// <summary>
    /// The visualization game object that will become active when the object is selected.
    /// </summary>
    public GameObject SelectionVisualization;

    protected override void Update()
    {
        base.Update();
    }

    protected override bool CanStartManipulationForGesture(TapGesture gesture)
    {
        return true;
    }

    protected override void OnEndManipulation(TapGesture gesture)
    {
        if (gesture.WasCancelled)
        {
            return;
        }

        if (ManipulationSystem.Instance == null)
        {
            return;
        }

        GameObject target = gesture.TargetObject;
        if (target == gameObject)
        {
            Select();
        }

        // Raycast against the location the player touched to search for planes.
        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon;

        if (!Frame.Raycast(
            gesture.StartPosition.x, gesture.StartPosition.y, raycastFilter, out hit))
        {
            Deselect();
        }
    }

    protected override void OnSelected()
    {
        SelectionVisualization.SetActive(true);
    }

    protected override void OnDeselected()
    {
        SelectionVisualization.SetActive(false);
    }
}
