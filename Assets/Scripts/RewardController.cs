using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rewards
{
    internal class RewardController
    {
        private readonly RewardView _view;

        private List<ContainerSlotRewardView> _slots;
        private Coroutine _coroutine;

        private bool _isGetReward;
        private bool _isInitialized;

        private string _frequencyName;

        public RewardController(RewardView view)
        {
            _view = view;
            _frequencyName = Enum.GetName(typeof(FrequencyType), _view.RewardsData.FrequencyType);
        }

        public void Init()
        {
            if (_isInitialized)
                return;

            InitSlots();
            RefreshUI();
            StartRewardUpdating();
            SubscribeButtons();

            _isInitialized = true;
        }

        public void Deinit()
        {
            if(!_isInitialized)
                return;

            DeinitSlots();
            StopRewardsUpdating();
            UnsubscribeButtons();
        }

        private void UnsubscribeButtons()
        {
            _view.GetRewardButton.onClick.RemoveListener(ClaimReward);
            _view.ResetButton.onClick.RemoveListener(ResetRewardsState);
        }

        private void StopRewardsUpdating()
        {
            if(_coroutine == null)
                return;
            
            _view.StopCoroutine(_coroutine);
            _coroutine = null;
        }

        private void DeinitSlots()
        {
            foreach (ContainerSlotRewardView slot in _slots)
                Object.Destroy(slot.gameObject);
            
            _slots.Clear();
        }

        private void SubscribeButtons()
        {
            _view.GetRewardButton.onClick.AddListener(ClaimReward);
            _view.ResetButton.onClick.AddListener(ResetRewardsState);
        }

        private void ResetRewardsState()
        {
            _view.TimeGetReward = null;
            _view.CurrentSlotInActive = 0;
        }

        private void ClaimReward()
        {
            if(!_isGetReward)
                return;

            Reward reward = _view.RewardsData.Rewards[_view.CurrentSlotInActive];

            switch (reward.RewardType)
            {
                case RewardType.Wood:
                    ResourceView.Instance.AddWood(reward.CountResource);
                    break;
                case RewardType.Diamond:
                    ResourceView.Instance.AddDiamond(reward.CountResource);
                    break;
            }
            
            _view.TimeGetReward = DateTime.UtcNow;
            _view.CurrentSlotInActive++;
            
            RefreshRewardsState();

        }

        private void StartRewardUpdating() => _coroutine = _view.StartCoroutine(RewardsStateUpdater());

        private IEnumerator RewardsStateUpdater()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(1);

            while (true)
            {
                RefreshRewardsState();
                RefreshUI();
                yield return waitForSeconds;
            }
        }

        private void RefreshRewardsState()
        {
            bool gotRewardEarlier = _view.TimeGetReward.HasValue;
            
            if (!gotRewardEarlier)
            {
                _isGetReward = true;
                return;
            }

            TimeSpan timeFromLastRewardGetting = DateTime.UtcNow - _view.TimeGetReward.Value;

            bool isDeadlineElapsed = timeFromLastRewardGetting.Seconds >= _view.RewardsData.TimeDeadline;

            bool isTimeToGetNewReward = timeFromLastRewardGetting.Seconds >= _view.RewardsData.TimeCooldown;

            if (isDeadlineElapsed)
                ResetRewardsState();

            _isGetReward = isTimeToGetNewReward;
        }

        private void RefreshUI()
        {
            _view.GetRewardButton.interactable = _isGetReward;
            _view.TimerNewReward.text = GetTimerNewRewardText();
            RefreshSlots();
        }

        private void RefreshSlots()
        {
            for(var i = 0; i < _slots.Count; i++)
            {
                Reward reward = _view.RewardsData.Rewards[i];
                int countDay = i + 1;
                bool isSelected = i == _view.CurrentSlotInActive;
                
                _slots[i].SetData(reward,countDay, isSelected, _frequencyName);
            }
        }

        private string GetTimerNewRewardText()
        {
            if (_isGetReward)
                return "The reward is ready to be received!";

            if (_view.TimeGetReward.HasValue)
            {
                DateTime nextClaimTime = _view.TimeGetReward.Value.AddSeconds(_view.RewardsData.TimeCooldown);
                TimeSpan currentClaimCooldown = nextClaimTime - DateTime.UtcNow;
                
                string timeGetReward =
                    $"{currentClaimCooldown.Days:D2}:{currentClaimCooldown.Hours:D2}:" +
                    $"{currentClaimCooldown.Minutes:D2}:{currentClaimCooldown.Seconds:D2}";
                
                return $"Time to get the next reward: {timeGetReward}";
            }
            
            return string.Empty;
        }

        private void InitSlots()
        {
            _slots = new List<ContainerSlotRewardView>();

            for (int i = 0; i < _view.RewardsData.Rewards.Count; i++)
            {
                ContainerSlotRewardView instanceSlot = CreateSlotRewardView();
                _slots.Add(instanceSlot);
            }
        }

        private ContainerSlotRewardView CreateSlotRewardView() =>
            Object.Instantiate
            (
                _view.ContainerSlotRewardPrefab,
                _view.MountRootSlotsReward,
                false
            );
        
    }
}