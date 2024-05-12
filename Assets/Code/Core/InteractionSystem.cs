using System;
using Cinemachine;
using UnityEngine;
using Whilefun.FPEKit;

namespace Code.Core
{
    public class InteractionSystem : MonoBehaviour
    {
        [SerializeField] private float _interactionDistance;
        [SerializeField] private float _examinationDistance;
        private CinemachineVirtualCamera _camera;
        private InteractableObject _currentInspectedObject;

        private void Start()
        {
            _camera = GameCore.PlayerObject.GetComponentInChildren<CinemachineVirtualCamera>();
            //_examinationCamera.enabled = false;
        }

        RaycastHit hit;
        private void Update()
        {
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, _interactionDistance))
            {
                if (hit.collider.TryGetComponent<InteractableObject>(out var interactableObject))
                {
                    if (Input.GetKeyDown(KeyCode.E) && GameCore.State == GameState.Normal)
                    {
                        Debug.Log("Interacting with object " + interactableObject.name);
                        interactableObject.Interact();
                    }
                }
            }
            
            if (GameCore.State == GameState.Inspecting)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    StopExamination();
                }
                
                // mouse drags rotate the object around like a 3d viewer
                // mouse scroll zooms in and out
                if (Input.GetMouseButton(0))
                {
                    _currentInspectedObject.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * 2);
                    _currentInspectedObject.transform.Rotate(Vector3.right, Input.GetAxis("Mouse Y") * 2);
                }
                else if (Input.mouseScrollDelta.y != 0)
                {
                    _currentInspectedObject.transform.position += _camera.transform.forward * Input.mouseScrollDelta.y;
                }
            }
            
        }
        
        private Vector3 _originalPosition;
        public void InspectObject(InteractableObject obj)
        {
            GameCore.State = GameState.Inspecting;
            _currentInspectedObject = obj;
            obj.onMadeInactive.Invoke();

            if (obj.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.isKinematic = true;
            }
            
            _originalPosition = obj.transform.position;
            _currentInspectedObject.transform.position = _camera.transform.position + _camera.transform.forward * _examinationDistance;
        }
        
        public void StopExamination()
        {
            if (GameCore.State != GameState.Inspecting) return;
            _currentInspectedObject.onMadeActive.Invoke();
            
            if (_currentInspectedObject.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.isKinematic = false;
            }
            
            _currentInspectedObject.transform.position = _originalPosition;
            _currentInspectedObject = null;
            GameCore.State = GameState.Normal;
        }
    }
}