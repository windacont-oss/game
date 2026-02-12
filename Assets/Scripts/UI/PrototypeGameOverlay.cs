using System.Text;
using ImbaLife.Core;
using ImbaLife.Gameplay;
using UnityEngine;

namespace ImbaLife.UI
{
    public sealed class PrototypeGameOverlay : MonoBehaviour
    {
        [SerializeField] private TaskBoard taskBoard;
        [SerializeField] private PrototypeSessionController sessionController;
        [SerializeField] private PlayerInteractor playerInteractor;

        private readonly StringBuilder builder = new StringBuilder();
        private bool showTaskList = true;

        public void Configure(TaskBoard board, PrototypeSessionController session, PlayerInteractor interactor)
        {
            taskBoard = board;
            sessionController = session;
            playerInteractor = interactor;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                showTaskList = !showTaskList;
            }

            if (sessionController != null && sessionController.IsEnded && Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1f;
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
        }

        private void OnGUI()
        {
            if (GameState.Instance == null || taskBoard == null) return;

            DrawCrosshair();
            DrawStatusPanel();
            DrawPrompt();
            DrawEndScreen();
        }

        private void DrawCrosshair()
        {
            GUI.Label(new Rect(Screen.width * 0.5f - 4f, Screen.height * 0.5f - 10f, 20f, 20f), "+");
        }

        private void DrawStatusPanel()
        {
            Rect panel = new Rect(16f, 16f, 470f, 300f);
            GUI.Box(panel, "IMBA БЫТ");

            GameState state = GameState.Instance;
            int daysLeft = sessionController != null ? sessionController.DaysLeftToWin : 0;
            float progress = sessionController != null ? sessionController.DayProgress01 : 0f;
            GUI.Label(new Rect(30f, 45f, 420f, 22f), $"День: {state.DayNumber} | Фаза: {state.CurrentPhase} | До победы: {daysLeft}");
            GUI.Label(new Rect(30f, 68f, 420f, 22f), $"Энергия: {state.Energy} | Стресс: {state.Stress} | Деньги: {taskBoard.Money}");
            GUI.Label(new Rect(30f, 91f, 420f, 22f), $"Прогресс дня: {(progress * 100f):0}%");
            GUI.Label(new Rect(30f, 115f, 420f, 22f), "WASD - ходьба | Мышь - обзор | E - действие | TAB - скрыть список дел");

            if (!showTaskList)
            {
                GUI.Label(new Rect(30f, 145f, 430f, 22f), "Список дел скрыт (TAB чтобы показать)");
                return;
            }

            builder.Clear();
            foreach (HouseTask task in taskBoard.Tasks)
            {
                builder.Append(task.IsCompleted ? "[x] " : "[ ] ");
                builder.Append(task.Title);
                builder.AppendLine();
            }

            GUI.TextArea(new Rect(30f, 145f, 430f, 150f), builder.ToString());
        }

        private void DrawPrompt()
        {
            if (playerInteractor == null || string.IsNullOrEmpty(playerInteractor.CurrentPrompt)) return;
            float width = 460f;
            Rect rect = new Rect(Screen.width * 0.5f - width * 0.5f, Screen.height - 90f, width, 30f);
            GUI.Box(rect, playerInteractor.CurrentPrompt);
        }

        private void DrawEndScreen()
        {
            if (sessionController == null || !sessionController.IsEnded) return;

            string title = sessionController.IsVictory ? "Победа: ты вывез быт и выжил." : "Поражение: стресс тебя съел.";
            Rect rect = new Rect(Screen.width * 0.5f - 230f, Screen.height * 0.5f - 70f, 460f, 140f);
            GUI.Box(rect, title + "\nНажми R для перезапуска сцены.");

        }
    }
}
