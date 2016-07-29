using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CarInteractor : MonoBehaviour
{
    [SerializeField]
    SteamVR_TrackedObject rightTrackedObject;

    [SerializeField]
    ControllerExtension rightControllerExtension;

    SteamVR_Controller.Device rightController { get { return SteamVR_Controller.Input((int)rightTrackedObject.index); } }

    void FixedUpdate()
    {
        if (rightController == null)
        {
            Debug.LogError("Controller not initialized.");
            return;
        }
    }

    public bool Grabbed { get { return rightController.GetPress(EVRButtonId.k_EButton_SteamVR_Trigger); } }
    public Vector3 ControllerPosition { get { return rightController.transform.pos; } }
    public bool Colliding { get { return rightControllerExtension.Colliding.Length > 0; } }
    public bool CollidesWithSteeringWheel { get { return rightControllerExtension.CollidesWithSteeringWheel; } }
    public bool SuddenlyGrabbed { get { return rightController.GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger); } }
    public Vector3 GrabbingPoint { get { return rightControllerExtension.GrabbingPoint; } }
    public float TouchInput { get { return rightController.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad) ? rightController.GetAxis(EVRButtonId.k_EButton_Axis0).y : 0; } }
}
