using UnityEngine;
using Whilefun.FPEKit;

namespace Code
{
    public class MiningChargePickupItem : MonoBehaviour
    {
        public float _projectedThrowForce;
        [SerializeField] private Object _realChargePrefab;
        private float _holdTime;
        private bool _wasHoldingLastFrame;
        private bool _isActive;
        private int _chargeCount;
        [SerializeField] private float _holdTimeBeforeThrow;
        [SerializeField] private float _baseThrowForce;
        
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
                    Throw();
                }
                _holdTime = 0;
                _projectedThrowForce = 0;
                _wasHoldingLastFrame = false;
            }
        }

        private void Throw()
        {
            GameObject charge = Instantiate(_realChargePrefab, transform.position, transform.rotation) as GameObject;
            charge.GetComponent<MiningCharge>().Throw(Camera.main.transform.forward, _projectedThrowForce);
            
            _chargeCount--;
            if (_chargeCount <= 0)
            {
                GameCore.PlayerInventory.DropItem(gameObject);
                Destroy(gameObject);
            }
        }
    }
}