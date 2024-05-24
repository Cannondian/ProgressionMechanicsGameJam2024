using System;
using UnityEngine;

namespace Code
{
    [RequireComponent(typeof(Collider))]
    public class KillPlane : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                ResetGame.ReloadScene();
            }
        }
    }
}