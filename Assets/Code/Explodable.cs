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
        
        public void Explode()
        {
            StartCoroutine(RunExplosion());
        }


        private IEnumerator RunExplosion()
        {
            _rigidbody.WakeUp();
            
            yield return new WaitForSeconds(1f);
            
            StartCoroutine(FadeOutAlpha());
        }
        
        private IEnumerator FadeOutAlpha()
        {
            float duration = 2f;
            float elapsedTime = 0f;
            Color startColor = _meshRenderer.material.color;
            float alpha = startColor.a;
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                Color newColor = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(alpha, 0, t));
                _meshRenderer.material.color = newColor;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            Destroy(this.gameObject);
        }
        
        
    }
}