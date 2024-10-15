using System;
using System.Collections.Generic;
using Managers.EnemyManagemant;
using UnityEngine;
using YG;

namespace Managers
{
    public class UserInfo : MonoBehaviour
    {
        public static UserInfo Instance;
        public static event Action<int> OnCoinChanged; 
        public static event Action OnDataChanged; 
        public static event Action<float> OnHealthBonus; // Добавляем событие для бонуса здоровья
        
        #region Singleton

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeData();
                Debug.Log("1");
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion

        public int CoinCount { get; private set; }
        public int EnemiesKilledByUser { get; private set; }
        public List<int> PurchasedAttacks { get; private set; }
        public float HealthBonus { get; private set; } // Добавляем поле для хранения бонуса здоровья

        private void InitializeData()
        {
            CoinCount = 0;
            EnemiesKilledByUser = 0;
            PurchasedAttacks = new List<int>();
            HealthBonus = 0; // Инициализируем бонус здоровья
        }

        private void OnEnable()
        {
            YandexGame.GetDataEvent += LoadData;
            WaveManager.OnAllWavesCompleted += SaveData;
            ResetProgressEvent.OnResetSaveClick += ResetProgress;
            if (YandexGame.SDKEnabled)
            {
                LoadData();
            }
        }

        private void OnDisable()
        {
            YandexGame.GetDataEvent -= LoadData;
            WaveManager.OnAllWavesCompleted -= SaveData;
            ResetProgressEvent.OnResetSaveClick -= ResetProgress;
        }

        private void Update()
        {
            OnDataChanged?.Invoke();
        }

        public void ChangeCoin(int amount)
        {
            CoinCount += amount;
            //Debug.Log($"Coin count changed by {amount}. New coin count: {CoinCount}");
            OnCoinChanged?.Invoke(CoinCount);
            SaveData();
        }

        private void LoadData()
        {
            CoinCount = YandexGame.savesData.CoinCount;
            EnemiesKilledByUser = YandexGame.savesData.EnemiesKilledByUser;
            PurchasedAttacks = YandexGame.savesData.PurchasedAttacks;
            HealthBonus = YandexGame.savesData.HealthBonus; // Загружаем бонус здоровья

            OnDataChanged?.Invoke();
            //Debug.Log("Data loaded successfully.");
        }

        private void SaveData()
        {
            YandexGame.savesData.CoinCount = CoinCount;
            YandexGame.savesData.EnemiesKilledByUser = EnemiesKilledByUser;
            YandexGame.savesData.PurchasedAttacks = PurchasedAttacks;
            YandexGame.savesData.HealthBonus = HealthBonus; // Сохраняем бонус здоровья

            YandexGame.SaveProgress();
            //Debug.Log("Data saved successfully.");
        }

        public void ResetProgress()
        {
            Debug.Log("Entered ResetProgress Method");
            // Сброс данных до начальных значений
            CoinCount = 0;
            EnemiesKilledByUser = 0;
            if (PurchasedAttacks == null)
            {
                PurchasedAttacks = new List<int>();
            }
            PurchasedAttacks.Clear();
            HealthBonus = 0; // Сбрасываем бонус здоровья
            // Переменна в данных
            OnDataChanged?.Invoke();
    
            // Сохранение сброшенных данных
            SaveData();

            Debug.Log("Progress reset successfully.");
        }

        // Метод для добавления бонуса здоровья
        public void AddHealthBonus(float amount)
        {
            HealthBonus += amount;
            OnHealthBonus?.Invoke(HealthBonus);
            SaveData(); // Сохраняем данные после добавления бонуса здоровья
        }
        
        public void AddCoinBonus(int amount)
        {
            CoinCount += amount;
            SaveData(); // Сохраняем данные после добавления монет
        }

        // Метод для увеличения количества убитых врагов и записи рекорда
        public void IncreaseEnemiesKilled()
        {
            EnemiesKilledByUser++;
            YandexGame.NewLeaderboardScores("EnemiesKilledLeaderboard", EnemiesKilledByUser);
            SaveData();
        }

        // Метод для загрузки состояния покупок атак
        public void LoadPurchasedAttacks(Dictionary<int, AttackData> attackDictionary)
        {
            foreach (var attackId in PurchasedAttacks)
            {
                if (attackDictionary.TryGetValue(attackId, out var attackData))
                {
                    attackData.IsPurchased = true;
                }
            }
        }
    }
}