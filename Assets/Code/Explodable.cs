using System;
using UnityEngine;

namespace Code
{
    [RequireComponent(typeof(Rigidbody))]
    public class Explodable : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            //_rigidbody.isKinematic = true;
            _rigidbody.Sleep();
        }


        public void Explode()
        {
            _rigidbody.WakeUp();
        }
        
    }
}