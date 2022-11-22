
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Rewards
{
    internal sealed class SceneLoaderView : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Button _button;

        [SerializeField] private TMP_Text _text;
        [SerializeField] private string _buttonLabel;

        [Header("Scene")] 
        [SerializeField] private string _sceneNameToLoad;

        private void Awake()
        {
            _text.text = _buttonLabel;
            _button.onClick.AddListener(ButtonLoadPressed);
        }

        private void ButtonLoadPressed()
        {
            SceneManager.LoadScene(_sceneNameToLoad);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}