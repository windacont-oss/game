using ImbaLife.Core;
using UnityEngine;

namespace ImbaLife.Gameplay
{
    public sealed class InteractableTaskObject : MonoBehaviour
    {
        [SerializeField] private string taskId;
        [SerializeField] private int energyCost = 15;
        [SerializeField] private string actionText = "Взаимодействовать";

        private bool used;

        public string ActionText => actionText;

        public void Configure(string id, int cost, string action)
        {
            taskId = id;
            energyCost = cost;
            actionText = action;
        }

        public bool TryUse(TaskBoard board)
        {
            if (used || board == null) return false;

            if (!GameState.Instance.ConsumeEnergy(energyCost))
            {
                Debug.Log("Недостаточно энергии. Попробуй поесть или отдохнуть.");
                return false;
            }

            board.CompleteTask(taskId);
            used = true;
            return true;
        }

        public void ResetForNewDay()
        {
            used = false;
        }
    }
}
