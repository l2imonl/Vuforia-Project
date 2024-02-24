using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CornerBehaviour : DefaultObserverEventHandler
{
    public bool tracked = false;

    protected override void OnTrackingFound()
    {
        Debug.Log("Target Found");

        this.tracked = true;

        base.OnTrackingFound();
    }

    protected override void OnTrackingLost()
    {
        Debug.Log("Target Lost");

        this.tracked = false;

        base.OnTrackingLost();
    }
}
