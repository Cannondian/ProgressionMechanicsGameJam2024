using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Whilefun.FPEKit;
using Object = UnityEngine.Object;

namespace Code
{
    public class MiningChargePickupItem : MonoBehaviour
    {
        public Vector3 projectedThrowForce;
        [SerializeField] private Object _realChargePrefab;
        private float _holdTime;
        private bool _wasHoldingLastFrame;
        private bool _isActive;
        private int _chargeCount;
        private CinemachineVirtualCamera camera;
        [SerializeField] private float _holdTimeBeforeThrow;
        [SerializeField] private float _baseThrowForce;
        [SerializeField] private float _chargeMass;
        [SerializeField] private float _maxThrowForce = 10;

        [Header("Throw Trajectory")]
        [SerializeField] private LineRenderer lr;
        [SerializeField] private Transform releasePosition;
        [SerializeField] [Range(10, 100)] private int LinePoints = 25;
        [SerializeField] [Range(0.01f, 0.25f)] private float timeBetweenPoints = 0.1f;
        [SerializeField] private LayerMask trajectoryCollision;

        private void Start()
        {
            camera = GameCore.PlayerObject.GetComponentInChildren<CinemachineVirtualCamera>();
        }

        public void OnPickup()
        {
            _isActive = true;
            releasePosition = GameCore.PlayerInventory.HandTransform;
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
                projectedThrowForce = Mathf.Clamp(_baseThrowForce * (_holdTime) % 10, 0f, _maxThrowForce) * camera.transform.forward;
                _wasHoldingLastFrame = true;
                DrawTrajectory();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (_wasHoldingLastFrame && _holdTime >= _holdTimeBeforeThrow)
                {
                    Throw();
                }
                _holdTime = 0;
                projectedThrowForce = Vector3.zero;
                _wasHoldingLastFrame = false;
                lr.enabled = false;
            }
        }

        private void DrawTrajectory()
        {
            lr.enabled = true;
            lr.positionCount = Mathf.CeilToInt(LinePoints / timeBetweenPoints) + 1;
            Vector3 startPos = releasePosition.position;
            Vector3 projVelocity = projectedThrowForce / _chargeMass;
            
            int i = 0;
            lr.SetPosition(0, startPos);
            for (float time = 0; time < LinePoints; time += timeBetweenPoints)
            {
                i++;
                Vector3 point = startPos + time * projVelocity;
                point.y = startPos.y + projVelocity.y * time + (Physics.gravity.y / 2f * time * time);
                
                lr.SetPosition(i, point);

                Vector3 lastPos = lr.GetPosition(i - 1);
                if (Physics.Raycast(lastPos, (point - lastPos).normalized, out RaycastHit hit,
                        (point - lastPos).magnitude, trajectoryCollision))
                {
                    lr.SetPosition(i, hit.point);
                    lr.positionCount = i + 1;
                    return;
                }
            }

        }

        private void Throw()
        {
            GameObject charge = Instantiate(_realChargePrefab, transform.position, transform.rotation) as GameObject;
            charge.GetComponent<MiningCharge>().Throw(projectedThrowForce);
            charge.GetComponent<Rigidbody>().mass = _chargeMass;
            
            _chargeCount--;
            if (_chargeCount <= 0)
            {
                GameCore.PlayerInventory.DropItem(gameObject);
                Destroy(gameObject);
            }
        }
    }
}