using DG.Tweening;
using UnityEngine;

namespace Code.Core
{
    public class GatePuzzleControl : MonoBehaviour
    {
        [SerializeField] private GameObject _gateDoorLeft;
        [SerializeField] private GameObject _gateDoorRight;
        [SerializeField] private GameObject _leverObject;
        private bool pulled = false;

        public void OnLeverActivate()
        {
            if (pulled) return;
            pulled = true;

            _leverObject.transform.DOLocalRotate(new Vector3(-45, 0, 0), 1f);
            _gateDoorLeft.transform.DOBlendableLocalMoveBy(new Vector3(0f, 0, 1.9f), 4f);
            _gateDoorRight.transform.DOBlendableLocalMoveBy(new Vector3(0f, 0, -1.9f), 4f);
        }
    }
}