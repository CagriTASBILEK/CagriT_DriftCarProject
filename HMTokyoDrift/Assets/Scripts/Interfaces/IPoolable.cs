namespace Interfaces
{
    public interface IPoolable
    {
        bool IsActive { get; }
        void OnSpawn();
        void OnDespawn();
    }
}