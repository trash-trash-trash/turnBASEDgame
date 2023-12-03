using System;
using UnityEngine;

namespace Overworld.Camera
{
    public class CameraDetectPlayerBox : MonoBehaviour
    {
        public bool trackingPlayer = false;
    
        public event Action<bool> DeclarePlayerDetectedEvent;
    
        public void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerBrain>() != null)
            {
                trackingPlayer = true;
                DeclarePlayerDetectedEvent?.Invoke(trackingPlayer);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerBrain>() != null)
            {
                trackingPlayer = false;
                DeclarePlayerDetectedEvent?.Invoke(trackingPlayer);
            }
        }
    }
}
