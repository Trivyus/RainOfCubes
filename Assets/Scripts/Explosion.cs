using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float explosionForce = 1000f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float upwardsModifier = 0.5f;
    [SerializeField] private ForceMode forceMode = ForceMode.Impulse;

    public void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Rigidbody rigidbody))
                rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier, forceMode);
        }
    }
}