using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Vehicles.Car;

namespace GamesCom
{
    [RequireComponent(typeof (CarController))]
    public class CartController : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use

        [SerializeField]
        private SteeringWheel wheel;
        [SerializeField]
        private CarInteractor interactor;

        [SerializeField]
        private Pedal brake;
        [SerializeField]
        private Pedal throttle;

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            float h = wheel.GetRotation();//CrossPlatformInputManager.GetAxis("Horizontal");
            float v = interactor.TouchInput;//CrossPlatformInputManager.GetAxis("Vertical");
            Debug.Log(interactor.TouchInput);

            if (v > 0)
                throttle.PressedValue = v;
            else
                throttle.PressedValue = 0;

            if (v < 0)
                brake.PressedValue = -v;
            else
                brake.PressedValue = 0;

#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
