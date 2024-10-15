using Managers;
using Managers.EnemyManagemant;
using TMPro;
using UI.LevelManagment;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI
{
    public class WinHandler : MonoBehaviour
    {
        [Header("Coin Display")]
        [SerializeField] private GameObject coinDisplay;

        [Header("Win Window")]
        [SerializeField] private GameObject winContainer;
        [SerializeField] private TextMeshProUGUI claimRewardText;
        [SerializeField] private TextMeshProUGUI doubleRewardText;

        private int _levelsPassed = 0;
        private int levelsBetweenAds = 3; // Показывать рекламу каждые 3 уровня
        
        private LevelData _currentLevelData;
        private int _rewardAmount;

        private void OnEnable()
        {
            LevelMenuUIManager.OnLevelSelected += HandleLevelSelected;
            WaveManager.OnAllWavesCompleted += ShowWinWindow;
        }

        private void OnDisable()
        {
            LevelMenuUIManager.OnLevelSelected -= HandleLevelSelected;
            WaveManager.OnAllWavesCompleted -= ShowWinWindow;
        }


        private void HandleLevelSelected(LevelData levelData)
        {
            _currentLevelData = levelData;
            _levelsPassed++;
        }
        
        public void ShowAdIfLevelsPassed()
        {
            if (_levelsPassed >= levelsBetweenAds)
            {
                ShowFullscreenAd();
                _levelsPassed = 0; // Сбрасываем счетчик после показа рекламы
            }
        }

        private void ShowFullscreenAd()
        {
            YandexGame.FullscreenShow();
        }


        private void ShowWinWindow()
        {
            if (_currentLevelData != null)
            {
                coinDisplay.SetActive(true);
                _rewardAmount = _currentLevelData.reward;

                winContainer.SetActive(true);

                // Устанавливаем текст на кнопках
                claimRewardText.text = $"+{_rewardAmount}";
                doubleRewardText.text = $"+{_rewardAmount * 2}";
            }
            else
            {
                Debug.LogError("No LevelData available for reward calculation or not all waves completed.");
            }
        }

        // Публичный метод для начисления награды
        public void ClaimReward()
        {
            if (UserInfo.Instance != null)
            {
                UserInfo.Instance.ChangeCoin(_rewardAmount);

            }
            else
            {
                Debug.LogError("UserInfo instance is null.");
            }
        }

        // Публичный метод для удвоения награды
        public void DoubleReward()
        {
            YandexGame.RewVideoShow(0); // Используем ID 0 для удвоения награды
            YandexGame.RewardVideoEvent += OnRewardedVideoCompleted;
        }

        private void OnRewardedVideoCompleted(int id)
        {
            if (id == 0)
            {
                if (UserInfo.Instance != null)
                {
                    UserInfo.Instance.ChangeCoin(_rewardAmount * 2);
                }
                else
                {
                    Debug.LogError("UserInfo instance is null.");
                }
            }
            YandexGame.RewardVideoEvent -= OnRewardedVideoCompleted;
        }
    }
}