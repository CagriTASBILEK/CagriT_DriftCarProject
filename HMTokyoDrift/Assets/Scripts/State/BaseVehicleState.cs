using Core.Events;
using Interfaces;
using Scriptables;
using UnityEngine;
using Vehicles;

namespace State
{
    /// <summary>
    /// Base class for all vehicle states implementing common functionality
    /// </summary>
    public abstract class BaseVehicleState : IVehicleState
    {
        protected readonly PlayerVehicle vehicle;
        protected readonly PlayerVehicleSettings settings;
        protected readonly IWheelController wheelController;

        protected BaseVehicleState(PlayerVehicle vehicle, PlayerVehicleSettings settings, IWheelController wheelController)
        {
            this.vehicle = vehicle;
            this.settings = settings;
            this.wheelController = wheelController;
        }

        public virtual void EnterState()
        {
            // Debug.Log($"<color=green>Entering State: {this.GetType().Name}</color>");
            GameEvents.TriggerVehicleStateChanged(this);
        }

        public virtual void ExitState() 
        {
            // Debug.Log($"<color=red>Exiting State: {this.GetType().Name}</color>");
            wheelController.ResetWheels();
        }

        public abstract void HandlePhysics(float deltaTime);
        public abstract void HandleInput(float inputValue);

        protected void UpdateVehiclePosition(Vector3 newPosition)
        {
            if (vehicle != null)
            {
                vehicle.transform.position = newPosition;
            }
        }

        protected void UpdateVehicleRotation(Quaternion newRotation)
        {
            if (vehicle != null)
            {
                vehicle.transform.rotation = newRotation;
            }
        }
    }
}