using UnityEngine;

namespace ImbaLife.Gameplay
{
    public sealed class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float interactionDistance = 2.2f;
        [SerializeField] private LayerMask interactionMask;
        [SerializeField] private TaskBoard taskBoard;

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.E)) return;

            if (Physics.Raycast(
                    playerCamera.transform.position,
                    playerCamera.transform.forward,
                    out RaycastHit hit,
                    interactionDistance,
                    interactionMask))
            {
                InteractableTaskObject interactable = hit.collider.GetComponent<InteractableTaskObject>();
                if (interactable != null)
                {
                    interactable.TryUse(taskBoard);
                }
            }
        }
    }
}
