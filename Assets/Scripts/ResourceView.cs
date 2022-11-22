using System;
using UnityEngine;

namespace Rewards
{
    internal class ResourceView : MonoBehaviour
    {
        private const string WoodKey = nameof(WoodKey);
        private const string DiamondKey = nameof(DiamondKey);

        private static ResourceView _instance;

        public static ResourceView Instance => _instance;

        [SerializeField] private ResourceSlotView _resourceWood;
        [SerializeField] private ResourceSlotView _resourceDiamond;

        private int Wood
        {
            get => PlayerPrefs.GetInt(WoodKey);
            set => PlayerPrefs.SetInt(WoodKey, value);
        }

        private int Diamond
        {
            get => PlayerPrefs.GetInt(DiamondKey);
            set => PlayerPrefs.SetInt(DiamondKey, value);
        }

        private void Awake() => _instance = this;

        private void OnDestroy() => _instance = null;

        private void Start()
        {
            _resourceWood.SetData(Wood);
            _resourceDiamond.SetData(Diamond);
        }

        public void AddWood(int value)
        {
            Wood += value;
            _resourceWood.SetData(Wood);
        }

        public void AddDiamond(int value)
        {
            Diamond += value;
            _resourceDiamond.SetData(Diamond);
        }
    }
}