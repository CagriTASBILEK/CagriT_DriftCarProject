using UnityEngine;

public class WheelController : MonoBehaviour, IWheelController
{
    [Header("Wheel References")]
    [SerializeField] private Transform frontLeftWheel;
    [SerializeField] private Transform frontRightWheel;
    [SerializeField] private Transform rearLeftWheel;
    [SerializeField] private Transform rearRightWheel;

    [Header("Suspension Settings")]
    [SerializeField] private float wheelSuspensionAmount = 0.3f;
    [SerializeField] private float wheelRotationSpeed = 360f;

    private Vector3 frontLeftDefaultPos;
    private Vector3 frontRightDefaultPos;
    private Vector3 rearLeftDefaultPos;
    private Vector3 rearRightDefaultPos;

    private float currentWheelRotation;
    private void Awake()
    {
        if (frontLeftWheel) frontLeftDefaultPos = frontLeftWheel.localPosition;
        if (frontRightWheel) frontRightDefaultPos = frontRightWheel.localPosition;
        if (rearLeftWheel) rearLeftDefaultPos = rearLeftWheel.localPosition;
        if (rearRightWheel) rearRightDefaultPos = rearRightWheel.localPosition;
    }

    public void UpdateWheels(float tiltAngle, float turnAngle)
    {
        if (!ValidateWheelReferences()) return;

        // Süspansiyon hesaplaması
        float leftSuspension = Mathf.Clamp(-tiltAngle, 0, wheelSuspensionAmount);
        float rightSuspension = Mathf.Clamp(tiltAngle, 0, wheelSuspensionAmount);

        // Tekerlek dönüş animasyonu
        currentWheelRotation += wheelRotationSpeed * Time.deltaTime;
        if (currentWheelRotation >= 360f) currentWheelRotation -= 360f;

        // Sol tekerleklerin pozisyonlarını güncelle
        frontLeftWheel.localPosition = new Vector3(
            frontLeftDefaultPos.x,
            frontLeftDefaultPos.y - leftSuspension,
            frontLeftDefaultPos.z
        );
        rearLeftWheel.localPosition = new Vector3(
            rearLeftDefaultPos.x,
            rearLeftDefaultPos.y - leftSuspension,
            rearLeftDefaultPos.z
        );

        // Sağ tekerleklerin pozisyonlarını güncelle
        frontRightWheel.localPosition = new Vector3(
            frontRightDefaultPos.x,
            frontRightDefaultPos.y - rightSuspension,
            frontRightDefaultPos.z
        );
        rearRightWheel.localPosition = new Vector3(
            rearRightDefaultPos.x,
            rearRightDefaultPos.y - rightSuspension,
            rearRightDefaultPos.z
        );

        // Ön tekerleklerin yön dönüşü
        Quaternion frontWheelRotation = Quaternion.Euler(currentWheelRotation, turnAngle, 0);
        Quaternion rearWheelRotation = Quaternion.Euler(currentWheelRotation, 0, 0);

        // Tüm tekerleklere dönüş uygula
        frontLeftWheel.localRotation = frontWheelRotation;
        frontRightWheel.localRotation = frontWheelRotation;
        rearLeftWheel.localRotation = rearWheelRotation;
        rearRightWheel.localRotation = rearWheelRotation;
    }

    public void ResetWheels()
    {
        if (!ValidateWheelReferences()) return;
        
       
        frontLeftWheel.localPosition = frontLeftDefaultPos;
        frontRightWheel.localPosition = frontRightDefaultPos;
        rearLeftWheel.localPosition = rearLeftDefaultPos;
        rearRightWheel.localPosition = rearRightDefaultPos;

        Quaternion defaultRotation = Quaternion.identity;
        frontLeftWheel.localRotation = defaultRotation;
        frontRightWheel.localRotation = defaultRotation;
        rearLeftWheel.localRotation = defaultRotation;
        rearRightWheel.localRotation = defaultRotation;

        currentWheelRotation = 0f;
    }

    private bool ValidateWheelReferences()
    {
        return frontLeftWheel != null && frontRightWheel != null && 
               rearLeftWheel != null && rearRightWheel != null;
    }
}
