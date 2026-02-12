using System.Reflection;
using System.Text;
using ImbaLife.Core;
using ImbaLife.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ImbaLife.UI
{
    public sealed class HUDController : MonoBehaviour
    {
        [SerializeField] private TaskBoard taskBoard;
        [SerializeField] private Component dayText;
        [SerializeField] private Component energyText;
        [SerializeField] private Component stressText;
        [SerializeField] private Component moneyText;
        [SerializeField] private Component taskListText;
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
            SetLabelText(dayText, $"Day {state.DayNumber} ({state.CurrentPhase})");
            SetLabelText(energyText, $"Energy: {state.Energy}");
            SetLabelText(stressText, $"Stress: {state.Stress}");
            SetLabelText(moneyText, $"Money: {taskBoard.Money}");
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

            SetLabelText(taskListText, builder.ToString());
            SetLabelText(moneyText, $"Money: {taskBoard.Money}");
        }

        private void HandleAllTasksCompleted()
        {
            builder.AppendLine("Все дела закрыты. Можно спокойно отдыхать.");
            SetLabelText(taskListText, builder.ToString());
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

        private static void SetLabelText(Component target, string value)
        {
            if (target == null) return;

            PropertyInfo textProperty = target.GetType().GetProperty("text");
            if (textProperty != null && textProperty.PropertyType == typeof(string) && textProperty.CanWrite)
            {
                textProperty.SetValue(target, value, null);
                return;
            }

            FieldInfo textField = target.GetType().GetField("text");
            if (textField != null && textField.FieldType == typeof(string))
            {
                textField.SetValue(target, value);
            }
        }
    }
}
