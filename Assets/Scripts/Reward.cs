using System;
using UnityEngine;

namespace Rewards
{
    [Serializable]
    internal class Reward
    {
        [field: SerializeField] public RewardType RewardType { get; private set; }
        [field: SerializeField] public Sprite IconResource { get; private set; }
        [field: SerializeField] public int CountResource { get; private set; }
    }
}