using UnityEngine;
using System.Collections;
using System;

public class SteeringWheel : MonoBehaviour
{
    [SerializeField]
    private float limitRotation;

    private float currentRotation;

    private Transform model;

    private Vector3 lastGrab;
    private bool grabbed;

    [SerializeField]
    private Vector3 rotationPoint;

    [SerializeField]
    private LayerMask layermask;
    private float baseRotation;

    void Start()
    {
        model = transform.GetChild(0);
    }

    void Update()
    {
        //Controller/Keyboardinputs
        //float input = Input.GetAxis("Horizontal");
        //currentRotation = input * limitRotation;

        if (Math.Abs(currentRotation) > limitRotation)
        {
                currentRotation = limitRotation * Mathf.Sign(currentRotation);
        }

        //Apply the rotation to the model
        Quaternion q = Quaternion.identity;
        q.eulerAngles = Vector3.forward * currentRotation;
        model.localRotation = q;
    }

    void FixedUpdate()
    {
        grabbed = Input.GetMouseButton(0);

        if (grabbed)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 10, layermask.value))
            {
                Vector3 collision = hit.point;
                Vector3 center = transform.position + rotationPoint;

                //Get Initial angle to get the relative angle between the original grab-point and the current grab point.
                if (Input.GetMouseButtonDown(0))
                {
                    baseRotation = Vector3.Angle(center - collision, transform.up);
                    Vector3 baseCross = Vector3.Cross(center - collision, transform.up);
                    if (baseCross.y > 0)
                        baseRotation = -baseRotation;
                }

                //Get angle of the grab-location.
                currentRotation = Vector3.Angle(center - collision, transform.up);
                Vector3 cross = Vector3.Cross(center - collision, transform.up);
                if (cross.y > 0)
                {
                    currentRotation = -currentRotation;
                }

                currentRotation -= baseRotation;
            }
        }
        else
        {
            //if not grabbed, let the rotation smoothly rotate until it reaches the resting point
            if (Math.Abs(currentRotation) > 0.0001f)
            {
                currentRotation *= 0.9f;
            }
            else
                currentRotation = 0;
        }
    }

    /// <summary>
    /// Returns the rotation of the steering wheel to be used by the car controller (or anything that needs it).
    /// </summary>
    /// <returns></returns>
    public float GetRotation()
    {
        if (limitRotation == 0)
            return 0;

        return currentRotation / limitRotation;
    }

    [ExecuteInEditMode]
    void OnDrawGizmos()
    {
        //Draws the helper for the center
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + rotationPoint, Vector3.one * 0.02f);
    }
}