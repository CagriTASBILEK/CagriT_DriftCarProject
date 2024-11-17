using UnityEngine;

public class DriftState : BaseVehicleState
{
    private float driftDirection;

    public DriftState(PlayerVehicle vehicle, PlayerVehicleSettings settings, IWheelController wheelController) 
        : base(vehicle, settings, wheelController)
    {
        driftDirection = 0f;
    }

    public override void EnterState()
    {
        base.EnterState();
        GameEvents.TriggerVehicleDriftStarted(driftDirection);
    }

    public override void ExitState()
    {
        base.ExitState();
        GameEvents.TriggerVehicleDriftEnded();
    }

    public override void HandlePhysics(float deltaTime)
    {
        float tilt = -driftDirection * settings.tiltAngle;    // Z ekseni eğimi
        float rotation = driftDirection * -settings.rotationAngle;  // Y ekseni yönü
    
        // Mevcut rotasyonu al
        Vector3 currentRotation = vehicle.transform.rotation.eulerAngles;
    
        // Yeni rotasyonu oluştur (X'i zorla 0 yap)
        Quaternion targetRotation = Quaternion.Euler(0f, rotation, tilt);
        Quaternion newRotation = Quaternion.Lerp(vehicle.transform.rotation, targetRotation, deltaTime * settings.driftSpeed);
    
        // X rotasyonunu zorla 0 yap
        Vector3 newEuler = newRotation.eulerAngles;
        newEuler.x = 0f;
    
        UpdateVehicleRotation(Quaternion.Euler(newEuler));
    
        wheelController?.UpdateWheels(tilt, driftDirection * settings.wheelTurnAngle);
    }

    public override void HandleInput(float inputValue)
    {
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
}