using System.Collections.Generic;
using UnityEngine;

namespace Rewards
{
    [CreateAssetMenu(fileName = nameof(RewardSettings), menuName = "Settings/" + nameof(RewardSettings))]
    internal sealed class RewardSettings : ScriptableObject
    {
        [Header("Frequency")] 
        public FrequencyType FrequencyType;

        [Header("Settings Time Get Reward")] 
        public float TimeCooldown;
        public float TimeDeadline;
        
        [Header("Settings Rewards")]
        public List<Reward> Rewards;
    }
}