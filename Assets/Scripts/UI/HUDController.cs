using System.Text;
using ImbaLife.Core;
using ImbaLife.Gameplay;
using TMPro;
using UnityEngine;

namespace ImbaLife.UI
{
    public sealed class HUDController : MonoBehaviour
    {
        [SerializeField] private TaskBoard taskBoard;
        [SerializeField] private TMP_Text dayText;
        [SerializeField] private TMP_Text energyText;
        [SerializeField] private TMP_Text stressText;
        [SerializeField] private TMP_Text moneyText;
        [SerializeField] private TMP_Text taskListText;
        [SerializeField] private GameObject gameOverPanel;

        private readonly StringBuilder builder = new();

        private void OnEnable()
        {
            GameState.Instance.OnStatsChanged += RefreshStats;
            GameState.Instance.OnPhaseChanged += HandlePhaseChanged;
            GameState.Instance.OnGameOver += ShowGameOver;

            if (taskBoard != null)
            {
                taskBoard.OnTasksUpdated += RefreshTasks;
                taskBoard.OnAllTasksCompleted += HandleAllTasksCompleted;
            }

            RefreshStats();
            RefreshTasks();
        }

        private void OnDisable()
        {
            if (GameState.Instance == null) return;

            GameState.Instance.OnStatsChanged -= RefreshStats;
            GameState.Instance.OnPhaseChanged -= HandlePhaseChanged;
            GameState.Instance.OnGameOver -= ShowGameOver;

            if (taskBoard != null)
            {
                taskBoard.OnTasksUpdated -= RefreshTasks;
                taskBoard.OnAllTasksCompleted -= HandleAllTasksCompleted;
            }
        }

        private void HandlePhaseChanged(DayPhase _) => RefreshStats();

        private void RefreshStats()
        {
            GameState state = GameState.Instance;
            dayText.text = $"Day {state.DayNumber} ({state.CurrentPhase})";
            energyText.text = $"Energy: {state.Energy}";
            stressText.text = $"Stress: {state.Stress}";
            moneyText.text = $"Money: {taskBoard.Money}";
        }

        private void RefreshTasks()
        {
            builder.Clear();

            foreach (HouseTask task in taskBoard.Tasks)
            {
                string status = task.IsCompleted ? "[x]" : "[ ]";
                builder.AppendLine($"{status} {task.Title}");
            }

            taskListText.text = builder.ToString();
            moneyText.text = $"Money: {taskBoard.Money}";
        }

        private void HandleAllTasksCompleted()
        {
            builder.AppendLine("Все дела закрыты. Можно спокойно отдыхать.");
            taskListText.text = builder.ToString();
        }

        private void ShowGameOver()
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        public void RestartScene()
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}
