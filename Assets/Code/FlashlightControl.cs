using System;
using UnityEngine;

namespace Code
{
    [RequireComponent(typeof(Light))]
    public class FlashlightControl : MonoBehaviour
    {
        [SerializeField] private Light light;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                light.enabled = !light.enabled;
            }
        }
    }
}