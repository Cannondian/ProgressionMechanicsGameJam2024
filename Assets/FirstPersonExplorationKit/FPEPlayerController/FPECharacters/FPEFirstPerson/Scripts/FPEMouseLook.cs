using System;
using UnityEngine;

namespace Whilefun.FPEKit {

    public class FPEMouseLook : MonoBehaviour
    {

        public float sensX;
        public float sensY;
        public float multiplier;

        public Transform orientation;
        public Transform camHolder;
        public PlayerController pm;

        float xRotation;
        float yRotation;

        [Header("Fov")] public bool useFluentFov;
        public Rigidbody rb;
        public Camera cam;
        public float minMovementSpeed;
        public float maxMovementSpeed;
        public float minFov;
        public float maxFov;
        private FPEFOVKick fovKick;
        [SerializeField] private AnimationCurve fovCurvenew = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f), new Keyframe(1.5f, -1f), new Keyframe(2f, 0f));
        
        [Header("View Bob")]
        [SerializeField]
        private bool cameraBobEnabled = true;
        [SerializeField]
        private FPELerpControlledBob m_JumpBob = new FPELerpControlledBob();
        [SerializeField]
        private AnimationCurve CameraBobCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f), new Keyframe(1.5f, -1f), new Keyframe(2f, 0f));
        [SerializeField]
        private Vector2 bobRangeStanding = new Vector2(0.05f, 0.1f);
        [SerializeField]
        private Vector2 bobRangeCrouching = new Vector2(0.2f, 0.2f);
        
        private float bobCurveTime = 0.0f;
        private float bobCycleX = 0.0f;
        private float bobCycleY = 0.0f;
        private float cumulativeStepCycleCount = 0.0f;
        private float nextStepInCycle = 0.0f;
        [SerializeField, Tooltip("Approximate unit length of complete stride (left and right foot) of player walk. Influenced by current speed and movement type (e.g. walking vs. running)")]
        private float stepInterval = 5.0f;
        
        [SerializeField]
        private Vector3 cameraOffsetStanding = Vector3.zero;
        [SerializeField]
        private Vector3 cameraOffsetCrouching = Vector3.zero;

        private PlayerController.MovementState lastMoveState;

        private void Start()
        {
           // Cursor.lockState = CursorLockMode.Locked;
           // Cursor.visible = false;
           fovKick = new FPEFOVKick();
           fovKick.IncreaseCurve = fovCurvenew;
           fovKick.Setup(cam);
        }

        private void Update()
        {
            // get mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * sensY;

            yRotation += mouseX * multiplier;

            xRotation -= mouseY * multiplier;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // rotate cam and orientation
            camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
            
            UpdateCameraPosition(rb.velocity.magnitude);

            if (useFluentFov) HandleFov();
            lastMoveState = pm.state;
        }

        private void HandleFov()
        {
            if (pm.state == lastMoveState) return;
            
            if (pm.state is PlayerController.MovementState.sprinting or PlayerController.MovementState.swinging)
            {
                StartCoroutine(fovKick.FOVKickUp());
            }
            else if (lastMoveState is PlayerController.MovementState.sprinting or PlayerController.MovementState.swinging)
            {
                StartCoroutine(fovKick.FOVKickDown());
            }
        }
        
        private void UpdateCameraPosition(float speed)
        {

            if (cameraBobEnabled)
            {

                if (rb.velocity.magnitude > 0 && pm.grounded)
                {

                    float xOffset = CameraBobCurve.Evaluate(bobCycleX);
                    float yOffset = CameraBobCurve.Evaluate(bobCycleY);

                    Vector3 newCameraPosition = cameraOffsetStanding;
                    Vector2 bobRange = bobRangeStanding;

                    if (pm.state == PlayerController.MovementState.crouching)
                    {
                        newCameraPosition = cameraOffsetCrouching;
                        bobRange = bobRangeCrouching;
                    }

                    newCameraPosition.y += (yOffset * bobRange.y) - m_JumpBob.Offset();
                    newCameraPosition.x += xOffset * bobRange.x;

                    // Update bob cycle
                    float VerticalToHorizontalRatioStanding = 2.0f;
                    bobCycleX += (speed * Time.deltaTime) / stepInterval;
                    bobCycleY += ((speed * Time.deltaTime) / stepInterval) * VerticalToHorizontalRatioStanding;

                    if (bobCycleX > bobCurveTime)
                    {
                        bobCycleX = bobCycleX - bobCurveTime;
                    }
                    if (bobCycleY > bobCurveTime)
                    {
                        bobCycleY = bobCycleY - bobCurveTime;
                    }

                    // Lastly, lerp toward our new target camera position
                    camHolder.transform.localPosition = Vector3.Lerp(camHolder.transform.localPosition, newCameraPosition, 0.1f);


                }
                else
                {

                    // If we aren't actively moving or bobbing, just lerp toward our appropriate camera position
                    if (pm.state == PlayerController.MovementState.crouching)
                    {
                        Vector3 newCameraPosition = cameraOffsetCrouching;
                        newCameraPosition.y -= m_JumpBob.Offset();
                        camHolder.transform.localPosition = Vector3.Lerp(camHolder.transform.localPosition, newCameraPosition, 0.1f);
                    }
                    else
                    {
                        Vector3 newCameraPosition = cameraOffsetStanding;
                        newCameraPosition.y -= m_JumpBob.Offset();
                        camHolder.transform.localPosition = Vector3.Lerp(camHolder.transform.localPosition, newCameraPosition, 0.1f);
                    }

                }

            }
            else
            {

                if (pm.state == PlayerController.MovementState.crouching)
                {
                    camHolder.transform.localPosition = Vector3.Lerp(camHolder.transform.localPosition, cameraOffsetCrouching, 0.1f);
                }
                else
                {
                    camHolder.transform.localPosition = Vector3.Lerp(camHolder.transform.localPosition, cameraOffsetStanding, 0.1f);
                }

            }


        }
    }
}
