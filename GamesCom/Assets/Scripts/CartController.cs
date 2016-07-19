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

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
            //wheel = GetComponent<SteeringWheel>();
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            float h = wheel.GetRotation();//CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
