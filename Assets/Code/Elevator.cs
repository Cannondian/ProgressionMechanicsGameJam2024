using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Rigidbody elevator_rb;
    // Start is called before the first frame update
    void Start()
    {
        elevator_rb = GetComponent<Rigidbody>();
        elevator_rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            elevator_rb.useGravity = true;
        }
    }
}
