using DG.Tweening;
using UnityEngine;

namespace Code.Core
{
    public class GatePuzzleControl : MonoBehaviour
    {
        [SerializeField] private GameObject _gateDoorLeft;
        [SerializeField] private GameObject _gateDoorRight;
        [SerializeField] private GameObject _leverObject;

        public void OnButtonPress()
        {
            Debug.Log("moving the doors");
            _gateDoorLeft.transform.DOLocalMoveX(1.9f, 3f);
            _gateDoorRight.transform.DOLocalMoveX(-1.9f, 3f);
            _leverObject.transform.DOLocalRotate(new Vector3(0, -45, 0), 1f);
        }
    }
}