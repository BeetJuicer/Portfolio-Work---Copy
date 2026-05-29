using System;
using UnityEngine;

namespace CommandPattern
{
    class FPSShoot : MonoBehaviour, IShooter
    {
        public float MaxRayDistance => maxRayDistance;
        private float maxRayDistance = 15f;
        [SerializeField] private LayerMask whatIsShootable;

        //temporary attack details
        [SerializeField] private float radius;
        [SerializeField] private float explosionForce;

        Vector3? hitPos;

        public void SetMaxRayDistance(float max)
        {
            maxRayDistance = max;
        }

        public void Shoot()
        {
            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit info, maxRayDistance, whatIsShootable))
            {
                hitPos = info.point;
                Collider[] hits = Physics.OverlapSphere(info.point, radius);
                foreach (Collider hit in hits)
                {
                    print("hit: " + hit.name);
                    if (hit.TryGetComponent(out Rigidbody rb))
                    {
                        print("rb: " + rb.name);
                        rb.AddExplosionForce(explosionForce, info.point, radius, 0.5f, ForceMode.Impulse);
                    }
                }
            }
            else
            {
                hitPos = null;
            }
        }

        public void Shoot(GameObject prefab)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit info, maxRayDistance, whatIsShootable))
            {
                hitPos = info.point;
                Collider[] hits = Physics.OverlapSphere(info.point, radius);
                Instantiate(prefab, info.point, Quaternion.identity);
                foreach (Collider hit in hits)
                {
                    if (hit.TryGetComponent(out Rigidbody rb))
                    {
                        rb.AddExplosionForce(explosionForce, info.point, radius, 0.5f, ForceMode.Impulse);
                    }
                }
            }
            else
            {
                hitPos = null;
            }
        }
    }
}
