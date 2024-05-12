using UnityEngine;
using UnityEngine.Events;

namespace Code.Core
{
    public class InteractableObject : MonoBehaviour
    {
        public InteractionType interactionType;
        public UnityEvent onMadeActive, onMadeInactive;
        public Vector3 holdPositionOffset, holdRotationOffset;
        
        public enum InteractionType
        {
            Pickup,
            Inspect,
            Activate,
            Disabled
        }
        
        public void Interact()
        {
            switch (interactionType)
            {
                case InteractionType.Pickup:
                    GameCore.PlayerInventory.TryAddItem(this.gameObject);
                    break;
                case InteractionType.Inspect:
                    GameCore.InteractionSystem.InspectObject(this);
                    break;
                case InteractionType.Activate:
                    this.onMadeActive.Invoke();
                    break;
                default:
                    return;
            }
        }
    }
}