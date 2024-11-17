using UnityEngine;

public class NormalState : BaseVehicleState
{
    private readonly Quaternion startRotation;

    public NormalState(PlayerVehicle vehicle, PlayerVehicleSettings settings, IWheelController wheelController) 
        : base(vehicle, settings, wheelController)
    {
        startRotation = Quaternion.Euler(0, 0, 0);
    }

    public override void HandlePhysics(float deltaTime)
    {
        if (vehicle.transform.rotation != startRotation)
        {
            UpdateVehicleRotation(
                Quaternion.Lerp(vehicle.transform.rotation, startRotation, deltaTime * settings.returnSpeed)
            );
        }
    }

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