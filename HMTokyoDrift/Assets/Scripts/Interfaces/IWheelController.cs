namespace Interfaces
{
    public interface IWheelController
    {
        void UpdateWheels(float tiltAngle, float turnAngle);
        void ResetWheels();
    }
}