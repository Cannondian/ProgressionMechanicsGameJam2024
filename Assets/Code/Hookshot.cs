using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Code.Core;
using UnityEngine;
using Whilefun.FPEKit;

public class Hookshot : MonoBehaviour
{
    [Header("References")]
    public LineRenderer lr;
    public Transform gunTip, cam, player;
    public LayerMask whatIsGrappleable;
    public FirstPersonControls fpeController;
    [SerializeField] private Transform hook;
    [SerializeField] private Transform hookStartPos;

    [Header("Swinging")]
    [SerializeField] private float maxSwingDistance = 25f;
    private Vector3 swingPoint;
    private SpringJoint joint;

    [Header("OdmGear")]
    public Transform orientation;
    public Rigidbody rb;
    public float horizontalThrustForce;
    public float forwardThrustForce;
    public float extendCableSpeed;

    [Header("Prediction")]
    public RaycastHit predictionHit;
    public float predictionSphereCastRadius;
    public Transform predictionPoint;

    [Header("Audio")]
    public AudioSource audSrc;
    public AudioClip hookShotShoot;
    public AudioClip hookShotHooked;


    [Header("Input")]
    public KeyCode swingKey = KeyCode.P;
    
    private bool isActive = false;
    private Vector3 remainingForce;

    public void OnPickup()
    {
        player = FindObjectOfType<FirstPersonControls>().transform;
        cam = GameCore.PlayerObject.GetComponentInChildren<CinemachineVirtualCamera>().transform;
        rb = player.GetComponent<Rigidbody>();
        orientation = player.transform;
        fpeController = player.GetComponent<FirstPersonControls>();
        predictionPoint.gameObject.SetActive(true);
        isActive = true;
    }
    
    public void OnDrop()
    {
        predictionPoint.gameObject.SetActive(false);
        isActive = false;
    }

    private void Start()
    {
        audSrc = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isActive) return;

        if (Input.GetKeyDown(swingKey))
        {
            audSrc.PlayOneShot(hookShotShoot,0.25f);
            StartSwing();
        }
        if (Input.GetKeyUp(swingKey)) StopSwing();
        
        CheckForSwingPoints();

        if (joint != null) OdmGearMovement();
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void CheckForSwingPoints()
    {
        if (joint != null) return;

        RaycastHit sphereCastHit;
        Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward, 
                            out sphereCastHit, maxSwingDistance, whatIsGrappleable);

        RaycastHit raycastHit;
        Physics.Raycast(cam.position, cam.forward, 
                            out raycastHit, maxSwingDistance, whatIsGrappleable);

        Vector3 realHitPoint;

        // Option 1 - Direct Hit
        if (raycastHit.point != Vector3.zero)
        {
            realHitPoint = raycastHit.point;
        }

        // Option 2 - Indirect (predicted) Hit
        else if (sphereCastHit.point != Vector3.zero)
        {
            realHitPoint = sphereCastHit.point;
        }

        // Option 3 - Miss
        else
            realHitPoint = Vector3.zero;

        // realHitPoint found
        if (realHitPoint != Vector3.zero)
        {
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = realHitPoint;
        }
        // realHitPoint not found
        else
        {
            predictionPoint.gameObject.SetActive(false);
        }

        predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
    }


    private void StartSwing()
    {
        // return if predictionHit not found
        if (predictionHit.point == Vector3.zero) return;
        
        Debug.Log("we are swinging");
        audSrc.PlayOneShot(hookShotHooked,0.5f);

        fpeController.ResetRestrictions();
        fpeController.swinging = true;
        rb.useGravity = true;

        swingPoint = predictionHit.point;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        if (predictionHit.collider.TryGetComponent<Rigidbody>(out var hitRB))
        {
            Debug.Log("Connected to rigidbudy" + rb.name);
            joint.connectedBody = hitRB;
        }
        else
        {
            joint.connectedAnchor = swingPoint;
        }

        float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

        // the distance grapple will try to keep from grapple point. 
        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;

        // customize values as you like
        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        lr.positionCount = 2;
        currentGrapplePosition = gunTip.position;
    }

    public void StopSwing()
    {
        fpeController.swinging = false;   
        lr.positionCount = 0;
        hook.localPosition = hookStartPos.position;

        Destroy(joint);
    }

    private void OdmGearMovement()
    {
        // right
        if (Input.GetKey(KeyCode.D)) rb.AddForce(orientation.right * (horizontalThrustForce * Time.deltaTime));
        // left
        if (Input.GetKey(KeyCode.A)) rb.AddForce(-orientation.right * (horizontalThrustForce * Time.deltaTime));

        // forward
        if (Input.GetKey(KeyCode.W)) rb.AddForce(orientation.forward * (horizontalThrustForce * Time.deltaTime));
        
        if (Input.GetKey(KeyCode.S)) rb.AddForce(-orientation.forward * (horizontalThrustForce * Time.deltaTime));

        // shorten cable
        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 directionToPoint = swingPoint - transform.position;
            rb.AddForce(directionToPoint.normalized * (forwardThrustForce * Time.deltaTime));

            float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
        }
        // extend cable
        if (Input.GetKey(KeyCode.LeftShift))
        {
            float extendedDistanceFromPoint = Vector3.Distance(transform.position, swingPoint);

            joint.maxDistance = extendedDistanceFromPoint * 0.8f;
            joint.minDistance = extendedDistanceFromPoint * 0.25f;
        }
    }

    private Vector3 currentGrapplePosition;

    private void DrawRope()
    {
        // if not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = 
            Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * extendCableSpeed);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
        hook.position = currentGrapplePosition;
    }
}
