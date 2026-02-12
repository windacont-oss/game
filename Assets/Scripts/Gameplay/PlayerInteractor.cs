using UnityEngine;

namespace ImbaLife.Gameplay
{
    public sealed class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float interactionDistance = 2.2f;
        [SerializeField] private LayerMask interactionMask = ~0;
        [SerializeField] private TaskBoard taskBoard;

        public void Configure(Camera cam, TaskBoard board)
        {
            playerCamera = cam;
            taskBoard = board;
        }

        private void Awake()
        {
            if (playerCamera == null)
            {
                playerCamera = Camera.main;
            }
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.E) || playerCamera == null) return;

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
                    return;
                }

                UtilityStation utilityStation = hit.collider.GetComponent<UtilityStation>();
                if (utilityStation != null)
                {
                    utilityStation.Use();
                }
            }
        }
    }
}
