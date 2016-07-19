using UnityEngine;
using System.Collections;

public class Pedal : MonoBehaviour
{

    private Quaternion restingRotation;
    private Quaternion pressedRotation;

    [SerializeField]
    private float pressedAngle;

    private float pressed;

    public float PressedValue
    {
        get { return pressed; }
        set { pressed = value; }
    }


    void Start()
    {
        restingRotation = transform.localRotation;
        pressedRotation = Quaternion.Euler(pressedAngle, restingRotation.eulerAngles.y, restingRotation.eulerAngles.z);
    }

    void Update()
    {
        transform.localRotation = Quaternion.Slerp(restingRotation, pressedRotation, pressed);
    }
}
