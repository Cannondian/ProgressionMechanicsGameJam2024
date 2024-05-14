using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpCrack : MonoBehaviour
{
    [SerializeField] private Image popCrack;
    [SerializeField] private Image originalHelmet;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            popCrack.enabled = true;
            originalHelmet.enabled = false;

        }
    }

}
