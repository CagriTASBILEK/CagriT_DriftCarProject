using UnityEngine;

public class TrackSegment : MonoBehaviour, IPoolable
{
    public bool IsActive => gameObject.activeSelf;

    public void OnSpawn()
    {
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        gameObject.SetActive(false);
    }
}