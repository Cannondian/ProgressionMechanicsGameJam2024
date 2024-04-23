using System;
using System.Collections.Generic;
using UnityEngine;
using Whilefun.FPEKit;

namespace Code
{
    [RequireComponent(typeof(FPEInteractablePickupScript))]
    public class MiningCharge : MonoBehaviour
    {
        [SerializeField] private Collider _explostionCollider;
        [SerializeField] private float _explosionForceThreshold;
        [SerializeField] private float _holdTimeBeforeThrow;
        [SerializeField] private float _baseThrowForce;
        public float _projectedThrowForce;
        private Rigidbody _rigidbody;
        private List<Explodable> _nearbyExplodables = new ();
        private float _holdTime;
        private bool _wasHoldingLastFrame;
        private bool _isActive;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void OnPickup()
        {
            _isActive = true;
        }
        
        public void OnDrop()
        {
            _isActive = false;
        }

        private void Update()
        {
            if (!_isActive) return;
            
            if (Input.GetMouseButton(0))
            {
                _holdTime += Time.deltaTime;
                _projectedThrowForce = _baseThrowForce * (_holdTime) % 10;
                _wasHoldingLastFrame = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (_wasHoldingLastFrame && _holdTime >= _holdTimeBeforeThrow)
                {
                    Throw(Camera.main.transform.forward, _projectedThrowForce);
                }
                _holdTime = 0;
                _projectedThrowForce = 0;
                _wasHoldingLastFrame = false;
            }
        }

        public void Throw(Vector3 direction, float force)
        {
            Debug.Log("Tossing explosive with force " + force);
            this.GetComponent<FPEInteractablePickupScript>().drop();
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(direction * force, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.relativeVelocity.magnitude > _explosionForceThreshold)
            {
                Explode();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Explodable explodable))
            {
                _nearbyExplodables.Add(explodable);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Explodable explodable))
            {
                _nearbyExplodables.Remove(explodable);
            }
        }

        private void Explode()
        {
            Debug.Log("KA-BOOOM!");
            foreach (var explodable in _nearbyExplodables)
            {
                explodable.Explode();
            }
            Destroy(this.gameObject);
        }
    }
}