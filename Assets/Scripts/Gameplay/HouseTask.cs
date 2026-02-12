using System;
using UnityEngine;

namespace ImbaLife.Gameplay
{
    [Serializable]
    public class HouseTask
    {
        [SerializeField] private string id;
        [SerializeField] private string title;
        [SerializeField] private string description;
        [SerializeField] private int stressPenalty = 10;
        [SerializeField] private int rewardMoney = 100;

        public string Id => id;
        public string Title => title;
        public string Description => description;
        public int StressPenalty => stressPenalty;
        public int RewardMoney => rewardMoney;

        public bool IsCompleted { get; private set; }

        public event Action<HouseTask> OnTaskCompleted;

        public void Complete()
        {
            if (IsCompleted) return;
            IsCompleted = true;
            OnTaskCompleted?.Invoke(this);
        }

        public void Reset()
        {
            IsCompleted = false;
        }
    }
}
