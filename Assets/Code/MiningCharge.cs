using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class MiningCharge : MonoBehaviour
    {
        [SerializeField] private float _explosionRadius;
        [SerializeField] private float _explosionForceThreshold;
        private Rigidbody _rigidbody;
        private List<Explodable> _nearbyExplodables = new ();
        private bool _isActive;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        public void Throw(Vector3 direction, float force)
        {
            Debug.Log("Tossing explosive with force " + force);
            _rigidbody.AddForce(direction * force, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.relativeVelocity.magnitude > _explosionForceThreshold)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, _explosionRadius);
                foreach (var col in hitColliders)
                {
                    if (col.TryGetComponent(out Explodable explodable))
                    {
                        _nearbyExplodables.Add(explodable);
                    }
                }
                Explode();
            }
        }

        private void Explode()
        {
            Debug.Log("KA-BOOOM!");
            foreach (var explodable in _nearbyExplodables)
            {
                Debug.Log("Affected explodable: " + explodable.name);
                explodable.Explode();
            }
            Destroy(this.gameObject);
        }
    }
}