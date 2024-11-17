using Core.Events;
using Interfaces;
using Scriptables;
using UnityEngine;
using Vehicles;

namespace State
{
    /// <summary>
    /// Represents the drifting state of the vehicle with scoring mechanics
    /// </summary>
    public class DriftState : BaseVehicleState
    {
        private float driftDirection;
        private float driftStartTime;
        private float currentDriftAngle;

        public DriftState(PlayerVehicle vehicle, PlayerVehicleSettings settings, IWheelController wheelController) 
            : base(vehicle, settings, wheelController)
        {
            driftDirection = 0f;
            driftStartTime = Time.time;
        }

        public override void EnterState()
        {
            base.EnterState();
            driftStartTime = Time.time;
        }

        public override void ExitState()
        {
            base.ExitState();
            float driftDuration = Time.time - driftStartTime;
            float driftScore = CalculateDriftScore(driftDuration, currentDriftAngle);
            GameEvents.TriggerScoreChange((int)driftScore);
        }

        public override void HandlePhysics(float deltaTime)
        {
            // Calculate tilt and rotation based on drift direction
            float tilt = -driftDirection * settings.tiltAngle;   
            float rotation = driftDirection * -settings.rotationAngle; 
        
            Vector3 currentRotation = vehicle.transform.rotation.eulerAngles;
        
            Quaternion targetRotation = Quaternion.Euler(0f, rotation, tilt);
            Quaternion newRotation = Quaternion.Lerp(vehicle.transform.rotation, targetRotation, deltaTime * settings.driftSpeed);
   
            Vector3 newEuler = newRotation.eulerAngles;
            newEuler.x = 0f;
        
            currentDriftAngle = Mathf.Abs(tilt);
    
            UpdateVehicleRotation(Quaternion.Euler(newEuler));
    
            wheelController?.UpdateWheels(tilt, driftDirection * settings.wheelTurnAngle);
        }

        public override void HandleInput(float inputValue)
        {
            // Return to normal state if input is below threshold
            if (Mathf.Abs(inputValue) < settings.driftThreshold)
            {
                // Debug.Log($"<color=yellow>Returning to Normal State - Input Value: {inputValue}</color>");
                vehicle.ChangeState(new NormalState(vehicle, settings, wheelController));
                return;
            }

            driftDirection = Mathf.Sign(inputValue);
        
            Vector3 newPosition = vehicle.transform.position;
            newPosition.x = Mathf.Clamp(
                newPosition.x + (inputValue * settings.driftSpeed * Time.fixedDeltaTime),
                -settings.maxDriftDistance,
                settings.maxDriftDistance
            );
        
            UpdateVehiclePosition(newPosition);
        }
    
        private float CalculateDriftScore(float duration, float angle)
        {
            float baseScore = duration * 100f;
            float angleMultiplier = angle / settings.tiltAngle;
            return baseScore * angleMultiplier;
        }
    }
}