using System;
using System.Collections;
using UnityEngine;

namespace Code
{
    [RequireComponent(typeof(Rigidbody))]
    public class Explodable : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private MeshRenderer _meshRenderer;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _meshRenderer = GetComponent<MeshRenderer>();
            //_rigidbody.isKinematic = true;
            _rigidbody.Sleep();
        }


        public IEnumerator Explode()
        {
            _rigidbody.WakeUp();
            
            yield return new WaitForSeconds(1f);
            
            yield return StartCoroutine(FadeOut());

        }
        
        private IEnumerator FadeOut()
        {
            float duration = 2f;
            float elapsedTime = 0f;
            Color initialColor = _meshRenderer.material.color;
            while (elapsedTime < duration)
            {
                _meshRenderer.material.color = Color.Lerp(initialColor, Color.clear, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            Destroy(this.gameObject);
        }
        
        
    }
}