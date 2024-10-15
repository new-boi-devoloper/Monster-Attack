using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Commercial
{
    public class CommercialManager : MonoBehaviour
    {
        [SerializeField] private int coinReward;
        [SerializeField] private int healthReward;
        [SerializeField] private Button doubleHealthButton; // Добавляем ссылку на кнопку удвоения здоровья
        [SerializeField] private Color originalColor = Color.white; // Оригинальный цвет кнопки
        [SerializeField] private Color disabledColor = Color.gray; // Цвет кнопки, когда она неактивна

        private int _levelsPassed = 0;
        private int levelsBetweenAds = 3; // Показывать рекламу каждые 3 уровня

        private void OnEnable()
        {
            Managers.EnemyManagemant.WaveManager.OnAllWavesCompleted += HandleLevelCompleted;
            Managers.UserInfo.OnHealthBonus += UpdateDoubleHealthButton; // Подписываемся на событие бонуса здоровья
        }

        private void OnDisable()
        {
            Managers.EnemyManagemant.WaveManager.OnAllWavesCompleted -= HandleLevelCompleted;
            Managers.UserInfo.OnHealthBonus -= UpdateDoubleHealthButton; // Отписываемся от события бонуса здоровья
        }

        private void HandleLevelCompleted()
        {
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

        // Метод для показа рекламы за вознаграждение монетами
        public void ShowRewardedAdForCoins()
        {
            YandexGame.RewardVideoEvent += RewardedForCoins;
            YandexGame.RewVideoShow(1); // Используем ID 1 для вознаграждения монетами
        }

        // Метод для показа рекламы за вознаграждение здоровьем
        public void ShowRewardedAdForHealth()
        {
            if (Managers.UserInfo.Instance.HealthBonus <= 0) // Проверяем, не был ли уже применен бонус
            {
                YandexGame.RewardVideoEvent += RewardedForHealth;
                YandexGame.RewVideoShow(2); // Используем ID 2 для вознаграждения здоровьем
            }
        }

        // Метод для обработки вознаграждения монетами
        private void RewardedForCoins(int id)
        {
            if (id == 1)
            {
                GiveMoneyBonus(coinReward); // Пример: даем 100 монет
            }
            YandexGame.RewardVideoEvent -= RewardedForCoins;
        }

        // Метод для обработки вознаграждения здоровьем
        private void RewardedForHealth(int id)
        {
            if (id == 2)
            {
                GiveHealthBonus(healthReward); // Пример: даем 50 единиц здоровья
            }
            YandexGame.RewardVideoEvent -= RewardedForHealth;
        }

        public void GiveHealthBonus(float amount)
        {
            Managers.UserInfo.Instance.AddHealthBonus(amount); // Пример: добавляем 50 единиц здоровья
        }

        public void GiveMoneyBonus(int amount)
        {
            Managers.UserInfo.Instance.AddCoinBonus(amount);
        }

        // Метод для обновления состояния кнопки удвоения здоровья
        private void UpdateDoubleHealthButton(float amount)
        {
            if (Managers.UserInfo.Instance.HealthBonus > 0)
            {
                doubleHealthButton.interactable = false; // Делаем кнопку неактивной, если бонус уже был применен
                doubleHealthButton.image.color = disabledColor; // Изменяем цвет кнопки на серый
            }
            else
            {
                doubleHealthButton.interactable = true; // Делаем кнопку активной, если бонус еще не был применен
                doubleHealthButton.image.color = originalColor; // Возвращаем оригинальный цвет кнопки
            }
        }
    }
}