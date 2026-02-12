using System;
using System.Collections.Generic;
using System.Linq;
using ImbaLife.Core;
using UnityEngine;

namespace ImbaLife.Gameplay
{
    public sealed class TaskBoard : MonoBehaviour
    {
        [SerializeField] private List<HouseTask> dailyTasks = new List<HouseTask>();
        [SerializeField] private int money;

        public IReadOnlyList<HouseTask> Tasks => dailyTasks;
        public int Money => money;

        public event Action OnTasksUpdated;
        public event Action OnAllTasksCompleted;

        private void Start()
        {
            SubscribeAll();
            OnTasksUpdated?.Invoke();
        }

        private void OnDestroy()
        {
            UnsubscribeAll();
        }

        public void ConfigureTasks(List<HouseTask> tasks)
        {
            UnsubscribeAll();
            dailyTasks = tasks ?? new List<HouseTask>();
            SubscribeAll();
            OnTasksUpdated?.Invoke();
        }

        public HouseTask GetTaskById(string id)
        {
            return dailyTasks.FirstOrDefault(t => t.Id == id);
        }

        public void CompleteTask(string id)
        {
            HouseTask task = GetTaskById(id);
            if (task == null)
            {
                Debug.LogWarning($"Task with id {id} is missing on TaskBoard.");
                return;
            }

            if (task.IsCompleted) return;

            task.Complete();
            money += task.RewardMoney;
            OnTasksUpdated?.Invoke();

            if (dailyTasks.Count > 0 && dailyTasks.All(t => t.IsCompleted))
            {
                OnAllTasksCompleted?.Invoke();
            }
        }

        public void PunishForSkippedTasks()
        {
            int skipped = dailyTasks.Count(t => !t.IsCompleted);
            if (skipped <= 0) return;

            int totalPenalty = dailyTasks
                .Where(t => !t.IsCompleted)
                .Sum(t => t.StressPenalty);

            GameState.Instance.AddStress(totalPenalty);
        }

        public void PrepareNextDay()
        {
            foreach (HouseTask task in dailyTasks)
            {
                task.Reset();
            }

            OnTasksUpdated?.Invoke();
        }

        private void SubscribeAll()
        {
            foreach (HouseTask task in dailyTasks)
            {
                task.OnTaskCompleted += HandleTaskCompleted;
            }
        }

        private void UnsubscribeAll()
        {
            foreach (HouseTask task in dailyTasks)
            {
                task.OnTaskCompleted -= HandleTaskCompleted;
            }
        }

        private void HandleTaskCompleted(HouseTask _) => OnTasksUpdated?.Invoke();
    }
}
