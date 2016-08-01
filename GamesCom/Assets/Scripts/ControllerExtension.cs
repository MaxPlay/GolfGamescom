using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ControllerExtension : MonoBehaviour
{
    private GameObject model;

    private List<GameObject> colliding;

    public GameObject[] Colliding
    {
        get { return colliding.ToArray(); }
    }

    private bool collidesWithSteeringWheel;
    private Vector3 relativeGrabbingPoint;

    public Vector3 GrabbingPoint
    {
        get
        {
            return relativeGrabbingPoint.x * transform.right + relativeGrabbingPoint.y * transform.up + relativeGrabbingPoint.z * transform.forward + transform.position;
        }
    }

    public bool CollidesWithSteeringWheel
    {
        get { return collidesWithSteeringWheel; }
    }

    private bool collidesWithClub;

    public bool CollidesWithClub
    {
        get { return collidesWithClub; }
        set { collidesWithClub = value; }
    }


    void Awake()
    {
        colliding = new List<GameObject>();
    }

    void Start()
    {
        relativeGrabbingPoint = GetComponent<BoxCollider>().center;
        model = transform.GetChild(0).gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        colliding.Add(other.gameObject);
        if (other.CompareTag("Steering Wheel"))
            collidesWithSteeringWheel = true;
        if (other.CompareTag("Club"))
            collidesWithClub = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (colliding.Contains(other.gameObject))
            colliding.Remove(other.gameObject);

        if (other.CompareTag("Steering Wheel"))
            collidesWithSteeringWheel = false;
        if (other.CompareTag("Club"))
            collidesWithClub = false;
    }

    public void SetRenderer(bool v)
    {
        if (model.activeSelf != v)
            model.SetActive(v);
    }
}
