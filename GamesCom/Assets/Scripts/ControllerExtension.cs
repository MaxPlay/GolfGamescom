using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllerExtension : MonoBehaviour
{
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

    void Awake()
    {
        colliding = new List<GameObject>();
    }

    void Start()
    {
        relativeGrabbingPoint = GetComponent<BoxCollider>().center;
    }

    void OnTriggerEnter(Collider other)
    {
        colliding.Add(other.gameObject);
        if (other.CompareTag("Steering Wheel"))
            collidesWithSteeringWheel = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (colliding.Contains(other.gameObject))
            colliding.Remove(other.gameObject);

        if (other.CompareTag("Steering Wheel"))
            collidesWithSteeringWheel = false;
    }
}
