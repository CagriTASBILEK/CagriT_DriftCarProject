using Interfaces;
using Scriptables;
using UnityEngine;
using Vehicles;

namespace State
{
    /// <summary>
    /// Represents the normal driving state of the vehicle with basic steering
    /// </summary>
    public class NormalState : BaseVehicleState
    {
        private readonly Quaternion startRotation;

        public NormalState(PlayerVehicle vehicle, PlayerVehicleSettings settings, IWheelController wheelController) 
            : base(vehicle, settings, wheelController)
        {
            startRotation = Quaternion.Euler(0, 0, 0);
        }

        /// <summary>
        /// Handles vehicle physics in normal state
        /// Smoothly returns vehicle to upright position
        /// </summary>
        public override void HandlePhysics(float deltaTime)
        {
            if (vehicle.transform.rotation != startRotation)
            {
                UpdateVehicleRotation(
                    Quaternion.Lerp(vehicle.transform.rotation, startRotation, deltaTime * settings.returnSpeed)
                );
            }
        }

        /// <summary>
        /// Handles steering input in normal state
        /// Transitions to drift state if input exceeds threshold
        /// </summary>
        public override void HandleInput(float inputValue)
        {
            if (Mathf.Abs(inputValue) > settings.driftThreshold)
            {
                // Debug.Log($"<color=yellow>Attempting Drift State - Input Value: {inputValue}</color>");
                vehicle.ChangeState(new DriftState(vehicle, settings, wheelController));
                return;
            }

            Vector3 newPosition = vehicle.transform.position;
            newPosition.x = Mathf.Clamp(
                newPosition.x + (inputValue * settings.normalSteerSpeed * Time.fixedDeltaTime),
                -settings.maxSteerDistance,
                settings.maxSteerDistance
            );
        
            UpdateVehiclePosition(newPosition);
            wheelController?.UpdateWheels(0f, inputValue * settings.wheelTurnAngle);
        }
    }
}