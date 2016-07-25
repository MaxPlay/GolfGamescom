using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField]
    SteamVR_TrackedObject trackedObject;
    private LineRenderer debugLineRenderer;

    Vector3 lastPosition;

    SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObject.index); } }

    void Start()
    {
        debugLineRenderer = GetComponent<LineRenderer>();
        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (controller == null)
        {
            Debug.LogError("Controller not initialized.");
            return;
        }

        if (controller.GetPress(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
        {
            debugLineRenderer.enabled = true;
            Vector3 velocity = trackedObject.transform.forward / 3f;
            Vector3 position = trackedObject.transform.position;
            List<Vector3> positions = new List<Vector3>();
            positions.Add(position);

            for (int i = 0; i < 20; i++)
            {
                RaycastHit hit;
                if (Physics.Raycast(new Ray(position, velocity), out hit, velocity.magnitude))
                {
                    positions.Add(hit.point);
                    break;
                }

                position += velocity;
                velocity += Physics.gravity / 2 * Time.deltaTime * 2;

                positions.Add(position);
            }
            debugLineRenderer.SetVertexCount(positions.Count);
            debugLineRenderer.SetPositions(positions.ToArray());
            lastPosition = positions[positions.Count - 1];
        }
        else
        {
            debugLineRenderer.enabled = false;
            transform.position = lastPosition;

        }
    }
}
