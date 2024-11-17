
namespace Interfaces
{
    public interface IVehicleState
    {
        void EnterState();
        void ExitState();
        void HandlePhysics(float deltaTime);
        void HandleInput(float inputValue);
    }
}
