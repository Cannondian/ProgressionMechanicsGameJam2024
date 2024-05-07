using System;
using UnityEngine;

namespace Code.Core
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private int _capacity = 4;
        public int Capacity => _capacity;
        
        [SerializeField] private int _currentCount = 0;
        public int CurrentCount => _currentCount;
        
        private int _currentIndex = 0;
        private GameObject[] _items;
        
        [SerializeField] private Transform _heldItemTransform;
        public Transform HandTransform => _heldItemTransform;
        
        
        private void Awake()
        {
            _items = new GameObject[_capacity];
        }
        
        public bool TryAddItem(GameObject item)
        {
            if (CurrentCount < _capacity)
            {
                for (int i = 0; i < _capacity; i++)
                {
                    if (_items[i] == null)
                    {
                        //item.SetActive(false);
                        _currentIndex = i;
                        Debug.Log("Item added to slot " + i);
                        item.transform.parent = transform;
                        _items[_currentIndex] = item;
                        _currentCount++;
                        if (item.TryGetComponent<InteractableObject>(out var interactable))
                        {
                            interactable.onPickup.Invoke();
                        }
                        EquipItem(_currentIndex);
                        return true;
                    }
                }
            }
            return false;
        }
        
        public void DropItem(GameObject item)
        {
            for (int i = 0; i < _capacity; i++)
            {
                if (_items[i] == item)
                {
                    DropItem(i);
                    return;
                }
            }
        }

        public void DropItem(int index)
        { 
            if (_items[index] == null) return;
            var item = _items[index];
            item.SetActive(true);
            item.transform.parent = null;
            _items[index] = null;
            _currentCount--;
            if (item.TryGetComponent<InteractableObject>(out var interactable))
            {
                interactable.onDrop.Invoke();
            }
        }

        public void EquipItem(int index)
        {
            if (_items[index] != null)
            {
                var item = _items[index];
                item.transform.parent = _heldItemTransform;
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = _heldItemTransform.localRotation;
                item.SetActive(true);
                if (item.TryGetComponent(out Rigidbody rb))
                {
                    rb.isKinematic = true;
                }
                if (item.TryGetComponent(out Collider col))
                {
                    col.isTrigger = true;
                }
                if (item.TryGetComponent<InteractableObject>(out var interactable))
                {
                    _heldItemTransform.position += interactable.holdPositionOffset;
                }
            }
        }
        
        public void UnequipItem(int index)
        {
            if (_items[index] != null)
            {
                var item = _items[index];
                item.transform.parent = transform;
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
                if (item.TryGetComponent(out Rigidbody rb))
                {
                    rb.isKinematic = false;
                }

                if (item.TryGetComponent(out Collider col))
                {
                    col.isTrigger = false;
                }
                if (item.TryGetComponent<InteractableObject>(out var interactable))
                {
                    _heldItemTransform.position -= interactable.holdPositionOffset;
                }
                item.SetActive(false);
            }
        }
        
        private void AdjustCurrentIndex(int i)
        {
            var last = _currentIndex;
            _currentIndex += i;
            if (_currentIndex >= _capacity)
            {
                _currentIndex = 0;
            }
            else if (_currentIndex < 0)
            {
                _currentIndex = _capacity - 1;
            }
            UnequipItem(last);
            EquipItem(_currentIndex);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                DropItem(_currentIndex);
            }
            
            // navigate through inventory with scroll wheel
            var scroll = Input.mouseScrollDelta.y;
            if (scroll > 0)
            {
                AdjustCurrentIndex(1);
            }
            else if (scroll < 0)
            {
                AdjustCurrentIndex(-1);
            }
            
            // or use numbers
            for (int i = 0; i < _capacity; i++)
            {
                if (Input.GetKeyDown((i + 1).ToString()))
                {
                    EquipItem(i);
                }
            }
        }
    }
}