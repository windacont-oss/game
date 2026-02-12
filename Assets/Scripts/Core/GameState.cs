using System;
using UnityEngine;

namespace ImbaLife.Core
{
    public enum DayPhase
    {
        Morning,
        Day,
        Evening,
        Night
    }

    public sealed class GameState : MonoBehaviour
    {
        public static GameState Instance { get; private set; }

        [SerializeField] private int maxStress = 100;
        [SerializeField] private int maxEnergy = 100;

        public int DayNumber { get; private set; } = 1;
        public int Stress { get; private set; }
        public int Energy { get; private set; } = 100;
        public DayPhase CurrentPhase { get; private set; } = DayPhase.Morning;

        public event Action OnStatsChanged;
        public event Action<DayPhase> OnPhaseChanged;
        public event Action OnGameOver;

        private bool gameOver;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            Energy = maxEnergy;
        }

        public void AddStress(int amount)
        {
            if (gameOver) return;
            Stress = Mathf.Clamp(Stress + amount, 0, maxStress);
            OnStatsChanged?.Invoke();

            if (Stress >= maxStress)
            {
                TriggerGameOver();
            }
        }

        public void AddEnergy(int amount)
        {
            if (gameOver) return;
            Energy = Mathf.Clamp(Energy + amount, 0, maxEnergy);
            OnStatsChanged?.Invoke();
        }

        public bool ConsumeEnergy(int amount)
        {
            if (gameOver) return false;

            if (Energy < amount)
            {
                AddStress(6);
                return false;
            }

            Energy = Mathf.Clamp(Energy - amount, 0, maxEnergy);
            OnStatsChanged?.Invoke();
            return true;
        }

        public void SetPhase(DayPhase phase)
        {
            if (gameOver) return;
            CurrentPhase = phase;
            OnPhaseChanged?.Invoke(phase);
        }

        public void StartNextDay()
        {
            if (gameOver) return;

            DayNumber++;
            CurrentPhase = DayPhase.Morning;
            AddEnergy(45);
            AddStress(-15);
            OnPhaseChanged?.Invoke(CurrentPhase);
        }

        public void ResetState()
        {
            gameOver = false;
            DayNumber = 1;
            Stress = 0;
            Energy = maxEnergy;
            CurrentPhase = DayPhase.Morning;
            OnStatsChanged?.Invoke();
            OnPhaseChanged?.Invoke(CurrentPhase);
        }

        private void TriggerGameOver()
        {
            gameOver = true;
            OnGameOver?.Invoke();
        }
    }
}
