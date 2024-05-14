using System;
using DG.Tweening;
using UnityEngine;

namespace Code.Core
{
    public class GatePuzzleControl : MonoBehaviour
    {
        [SerializeField] private GameObject _gateDoorLeft;
        [SerializeField] private GameObject _gateDoorRight;
        [SerializeField] private GameObject _leverObject;
        [SerializeField] private Light _light;
        private bool pulled = false;

        private void Start()
        {
            _light.color = Color.red;
        }

        public void OnLeverActivate()
        {
            if (pulled) return;
            pulled = true;
            _light.color = Color.green;

            _leverObject.transform.DOLocalRotate(new Vector3(-45, 0, 0), 1f);
            _gateDoorLeft.transform.DOBlendableLocalMoveBy(new Vector3(0f, 0, 1.9f), 4f);
            _gateDoorRight.transform.DOBlendableLocalMoveBy(new Vector3(0f, 0, -1.9f), 4f);
        }
    }
}