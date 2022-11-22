using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rewards
{
    internal class RewardView : MonoBehaviour
    {
        private const string CurrentSlotInActiveKey = nameof(CurrentSlotInActiveKey);
        private const string TimeGetRewardKey = nameof(TimeGetRewardKey);

        private string _currentSlotInActiveKey;
        private string _timeGetRewardKey;

        public RewardSettings RewardsData;

        [field: Header("Ui Elements")]
        [field: SerializeField] public TMP_Text TimerNewReward { get; private set; }
        [field: SerializeField] public Transform MountRootSlotsReward { get; private set; }
        [field: SerializeField] public ContainerSlotRewardView ContainerSlotRewardPrefab { get; private set; }
        [field: SerializeField] public Button GetRewardButton { get; private set; }
        [field: SerializeField] public Button ResetButton { get; private set; }

        private void Awake()
        {
            _currentSlotInActiveKey = GetPlayerPrefsKey(RewardsData.FrequencyType, CurrentSlotInActiveKey);
            _timeGetRewardKey = GetPlayerPrefsKey(RewardsData.FrequencyType, TimeGetRewardKey);
        }

        private string GetPlayerPrefsKey(FrequencyType frequencyType, string keyName)
        {
            return Enum.GetName(typeof(FrequencyType), frequencyType) + keyName;
        }

        public int CurrentSlotInActive
        {
            get => PlayerPrefs.GetInt(_currentSlotInActiveKey);
            set => PlayerPrefs.SetInt(_currentSlotInActiveKey, value);
        }

        public DateTime? TimeGetReward
        {
            get
            {
                string data = PlayerPrefs.GetString(_timeGetRewardKey);
                return !string.IsNullOrEmpty(data) ? DateTime.Parse(data) : null;
            }
            set
            {
                if (value != null)
                    PlayerPrefs.SetString(_timeGetRewardKey, value.ToString());
                else
                    PlayerPrefs.DeleteKey(_timeGetRewardKey);
            }
        }
    }
}
