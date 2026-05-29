using UnityEngine;

public interface IShooter 
{
    void Shoot();
    void Shoot(GameObject prefab);
    void SetMaxRayDistance(float max);
    float MaxRayDistance { get; }
}
