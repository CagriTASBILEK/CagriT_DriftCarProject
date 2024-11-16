public interface IDamageable
{
    float Health { get; set; }
    float MaxHealth { get; }
    bool IsAlive { get; }
    void TakeDamage(float damage);
    void Die();
}