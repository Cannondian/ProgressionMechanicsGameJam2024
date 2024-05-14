using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popupTrigger : MonoBehaviour
{

    public GameObject popup;
    void onTriggerEnter(Collider other)
    {
        if (other.CompareTag ("popmessage")) {
            this.enabled = true;
        }
    
    }
}