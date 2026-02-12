using System.Text;
using ImbaLife.Core;
using ImbaLife.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ImbaLife.UI
{
    public sealed class HUDController : MonoBehaviour
    {
        [SerializeField] private TaskBoard taskBoard;
        [SerializeField] private Text dayText;
        [SerializeField] private Text energyText;
        [SerializeField] private Text stressText;
        [SerializeField] private Text moneyText;
        [SerializeField] private Text taskListText;
        [SerializeField] private GameObject gameOverPanel;

        private readonly StringBuilder builder = new StringBuilder();

        private void OnEnable()
        {
            if (GameState.Instance == null)
            {
                Debug.LogError("GameState instance is missing in scene.");
                return;
            }

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
            if (taskBoard == null) return;

            GameState state = GameState.Instance;
            dayText.text = $"Day {state.DayNumber} ({state.CurrentPhase})";
            energyText.text = $"Energy: {state.Energy}";
            stressText.text = $"Stress: {state.Stress}";
            moneyText.text = $"Money: {taskBoard.Money}";
        }

        private void RefreshTasks()
        {
            if (taskBoard == null) return;

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
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }

            Time.timeScale = 0f;
        }

        public void RestartScene()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
