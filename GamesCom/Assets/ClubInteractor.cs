using UnityEngine;
using System.Collections;
using Valve.VR;
using System;

public class ClubInteractor : MonoBehaviour
{
    enum ClubGrabber
    {
        None,
        Right,
        Left
    }

    ClubGrabber currentGrabber;

    Renderer model;
    new Rigidbody rigidbody;
    GameObject globalPhysics;

    [SerializeField]
    private Material regular;
    [SerializeField]
    private Material outlined;

    [SerializeField]
    SteamVR_TrackedObject rightTrackedObject;

    [SerializeField]
    ControllerExtension rightControllerExtension;

    [SerializeField]
    SteamVR_TrackedObject leftTrackedObject;

    [SerializeField]
    ControllerExtension leftControllerExtension;
    private Vector3 relativeGrabbingPoint;

    SteamVR_Controller.Device rightController { get { return SteamVR_Controller.Input((int)rightTrackedObject.index); } }
    SteamVR_Controller.Device leftController { get { return SteamVR_Controller.Input((int)leftTrackedObject.index); } }



    void Start()
    {
        model = GetComponent<Renderer>();
        rigidbody = GetComponent<Rigidbody>();
        globalPhysics = transform.GetChild(0).gameObject;
    }

    void FixedUpdate()
    {
        if (rightController == null)
        {
            Debug.LogError("Right Controller not initialized.");
            return;
        }

        if (leftController == null)
        {
            Debug.LogError("Right Controller not initialized.");
            return;
        }

        Highlight(CollidesWithClubRight || CollidesWithClubLeft);

        if (CollidesWithClubRight && GrabbedRight)
        {
            currentGrabber = ClubGrabber.Right;
        }
        else
        if (CollidesWithClubLeft && GrabbedLeft)
            currentGrabber = ClubGrabber.Left;
        else
            currentGrabber = ClubGrabber.None;
        switch (currentGrabber)
        {
            case ClubGrabber.None:
                this.transform.parent = null;
                rightControllerExtension.SetRenderer(true);
                leftControllerExtension.SetRenderer(true);
                this.rigidbody.useGravity = true;
                globalPhysics.SetActive(true);
                break;
            case ClubGrabber.Right:
                if (SuddenlyGrabbedRight)
                {
                    relativeGrabbingPoint = this.transform.position - GrabbingPointRight;
                    this.transform.parent = rightControllerExtension.transform;
                    rightControllerExtension.SetRenderer(false);
                    leftControllerExtension.SetRenderer(true);
                    this.rigidbody.useGravity = false;
                    transform.rotation = rightControllerExtension.transform.rotation;
                    globalPhysics.SetActive(false);
                }
                break;
            case ClubGrabber.Left:
                if (SuddenlyGrabbedLeft)
                {
                    relativeGrabbingPoint = this.transform.position - GrabbingPointRight;
                    leftControllerExtension.SetRenderer(false);
                    rightControllerExtension.SetRenderer(true);
                    this.rigidbody.useGravity = false;
                    this.transform.parent = leftControllerExtension.transform;
                    transform.rotation = leftControllerExtension.transform.rotation;
                    globalPhysics.SetActive(false);
                }
                break;
        }

    }

    private void Highlight(bool highlight)
    {
        if (highlight)
            model.GetComponent<Renderer>().materials = new Material[] { outlined };
        else
            model.GetComponent<Renderer>().materials = new Material[] { regular };
    }

    public bool GrabbedRight { get { return rightController.GetPress(EVRButtonId.k_EButton_SteamVR_Trigger); } }
    public bool GrabbedLeft { get { return leftController.GetPress(EVRButtonId.k_EButton_SteamVR_Trigger); } }

    public Vector3 ControllerPositionRight { get { return rightController.transform.pos; } }
    public Vector3 ControllerPositionLeft { get { return leftController.transform.pos; } }

    public bool CollidingRight { get { return rightControllerExtension.Colliding.Length > 0; } }
    public bool CollidingLeft { get { return leftControllerExtension.Colliding.Length > 0; } }

    public bool SuddenlyGrabbedRight { get { return rightController.GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger); } }
    public bool SuddenlyGrabbedLeft { get { return leftController.GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger); } }

    public Vector3 GrabbingPointRight { get { return rightControllerExtension.GrabbingPoint; } }
    public Vector3 GrabbingPointLeft { get { return leftControllerExtension.GrabbingPoint; } }

    public float TouchInputRight { get { return rightController.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad) ? rightController.GetAxis(EVRButtonId.k_EButton_Axis0).y : 0; } }
    public float TouchInputLeft { get { return leftController.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad) ? rightController.GetAxis(EVRButtonId.k_EButton_Axis0).y : 0; } }

    public bool CollidesWithClubRight { get { return rightControllerExtension.CollidesWithClub; } }
    public bool CollidesWithClubLeft { get { return leftControllerExtension.CollidesWithClub; } }
}
