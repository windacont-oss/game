using ImbaLife.Core;
using UnityEngine;

namespace ImbaLife.Gameplay
{
    public enum UtilityType
    {
        Bed,
        Fridge,
        Shower,
        Bin
    }

    public sealed class UtilityStation : MonoBehaviour
    {
        [SerializeField] private UtilityType type;
        [SerializeField] private int energyDelta;
        [SerializeField] private int stressDelta;
        [SerializeField] private string optionalTaskId;
        [SerializeField] private TaskBoard taskBoard;

        public void Use()
        {
            GameState.Instance.AddEnergy(energyDelta);
            GameState.Instance.AddStress(stressDelta);

            if (!string.IsNullOrWhiteSpace(optionalTaskId) && taskBoard != null)
            {
                taskBoard.CompleteTask(optionalTaskId);
            }

            if (type == UtilityType.Bed)
            {
                taskBoard?.PunishForSkippedTasks();
                taskBoard?.PrepareNextDay();
                GameState.Instance.StartNextDay();
            }
        }
    }
}
