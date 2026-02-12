using ImbaLife.Core;
using UnityEngine;

namespace ImbaLife.Gameplay
{
    public sealed class PrototypeSessionController : MonoBehaviour
    {
        [SerializeField] private TaskBoard taskBoard;
        [SerializeField] private float dayDurationSeconds = 210f;
        [SerializeField] private float stressTickInterval = 25f;
        [SerializeField] private int surviveDaysToWin = 4;

        private float dayTimer;
        private float stressTimer;
        private bool subscribed;

        public bool IsVictory { get; private set; }

        public void Configure(TaskBoard board)
        {
            taskBoard = board;
            TrySubscribeTaskBoard();
        }

        public bool IsEnded { get; private set; }
        public float DayProgress01 => Mathf.Clamp01(dayTimer / Mathf.Max(1f, dayDurationSeconds));
        public int DaysLeftToWin => Mathf.Max(0, surviveDaysToWin - GameState.Instance.DayNumber + 1);

        private void OnEnable()
        {
            TrySubscribeTaskBoard();

            if (GameState.Instance != null)
            {
                GameState.Instance.OnGameOver += HandleGameOver;
            }
        }

        private void Start()
        {
            TrySubscribeTaskBoard();
        }

        private void OnDisable()
        {
            if (taskBoard != null && subscribed)
            {
                taskBoard.OnAllTasksCompleted -= HandleAllTasksCompleted;
                subscribed = false;
            }

            if (GameState.Instance != null)
            {
                GameState.Instance.OnGameOver -= HandleGameOver;
            }
        }


        private void TrySubscribeTaskBoard()
        {
            if (taskBoard == null || subscribed) return;
            taskBoard.OnAllTasksCompleted += HandleAllTasksCompleted;
            subscribed = true;
        }

        private void Update()
        {
            if (IsEnded || GameState.Instance == null || taskBoard == null) return;

            dayTimer += Time.deltaTime;
            stressTimer += Time.deltaTime;

            UpdatePhaseFromTimer();

            if (stressTimer >= stressTickInterval)
            {
                stressTimer = 0f;
                ApplyAmbientStress();
            }

            if (dayTimer >= dayDurationSeconds)
            {
                EndCurrentDay();
            }

            if (GameState.Instance.DayNumber >= surviveDaysToWin)
            {
                IsVictory = true;
                IsEnded = true;
                Time.timeScale = 0f;
            }
        }

        private void UpdatePhaseFromTimer()
        {
            float progress = DayProgress01;
            if (progress < 0.25f)
            {
                GameState.Instance.SetPhase(DayPhase.Morning);
            }
            else if (progress < 0.6f)
            {
                GameState.Instance.SetPhase(DayPhase.Day);
            }
            else if (progress < 0.85f)
            {
                GameState.Instance.SetPhase(DayPhase.Evening);
            }
            else
            {
                GameState.Instance.SetPhase(DayPhase.Night);
            }
        }

        private void ApplyAmbientStress()
        {
            int incompleteCount = 0;
            foreach (HouseTask task in taskBoard.Tasks)
            {
                if (!task.IsCompleted)
                {
                    incompleteCount++;
                }
            }

            if (incompleteCount <= 0) return;

            int baseStress = GameState.Instance.CurrentPhase == DayPhase.Night ? 3 : 1;
            GameState.Instance.AddStress(baseStress + incompleteCount);
        }

        private void EndCurrentDay()
        {
            dayTimer = 0f;
            stressTimer = 0f;

            taskBoard.PunishForSkippedTasks();
            taskBoard.PrepareNextDay();

            foreach (InteractableTaskObject interactable in FindObjectsOfType<InteractableTaskObject>())
            {
                interactable.ResetForNewDay();
            }

            GameState.Instance.StartNextDay();
        }

        private void HandleAllTasksCompleted()
        {
            GameState.Instance.AddStress(-8);
            GameState.Instance.AddEnergy(10);
        }

        private void HandleGameOver()
        {
            IsEnded = true;
        }
    }
}
