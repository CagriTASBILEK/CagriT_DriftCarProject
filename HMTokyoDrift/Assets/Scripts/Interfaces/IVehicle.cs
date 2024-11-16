public interface IVehicle
{
    int CurrentLane { get; }
    bool IsActive { get; }
    void Initialize(int lane);
    void HandleCollision(IVehicle other);
}