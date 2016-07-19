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
    private Vector3 lastCollision;

    [SerializeField]
    private LayerMask layermask;

    void Start()
    {
        model = transform.GetChild(0);
    }

    void Update()
    {
        float input = Input.GetAxis("Horizontal");
        currentRotation = input * limitRotation;

        Quaternion q = Quaternion.identity;
        q.eulerAngles = Vector3.forward * currentRotation;
        model.localRotation = q;
        Debug.Log(Vector3.forward * currentRotation);
    }

    void FixedUpdate()
    {
        grabbed = Input.GetMouseButton(0);
        Debug.Log(grabbed);
        if (grabbed)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 10, layermask.value))
            {
                Vector3 collision = hit.point;
                Vector3 center = transform.position + rotationPoint;


                lastCollision = collision;
            }

            Debug.DrawRay(ray.origin, ray.direction*5);
        }
    }

    public float GetRotation()
    {
        if (limitRotation == 0)
            return 0;

        return currentRotation / limitRotation;
    }

    [ExecuteInEditMode]
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + rotationPoint, Vector3.one * 0.02f);
    }
}