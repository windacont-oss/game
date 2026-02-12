using UnityEngine;

namespace ImbaLife.Gameplay
{
    public sealed class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float interactionDistance = 2.2f;
        [SerializeField] private LayerMask interactionMask = ~0;
        [SerializeField] private TaskBoard taskBoard;

        public string CurrentPrompt { get; private set; }

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
            if (playerCamera == null)
            {
                CurrentPrompt = string.Empty;
                return;
            }

            bool hasHit = Physics.Raycast(
                playerCamera.transform.position,
                playerCamera.transform.forward,
                out RaycastHit hit,
                interactionDistance,
                interactionMask);

            CurrentPrompt = ResolvePrompt(hasHit, hit);

            if (!Input.GetKeyDown(KeyCode.E) || !hasHit) return;

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

        private static string ResolvePrompt(bool hasHit, RaycastHit hit)
        {
            if (!hasHit) return string.Empty;

            InteractableTaskObject interactable = hit.collider.GetComponent<InteractableTaskObject>();
            if (interactable != null)
            {
                return $"E: {interactable.ActionText} ({hit.collider.gameObject.name})";
            }

            UtilityStation utility = hit.collider.GetComponent<UtilityStation>();
            if (utility != null)
            {
                return $"E: Использовать {hit.collider.gameObject.name}";
            }

            return string.Empty;
        }
    }
}
