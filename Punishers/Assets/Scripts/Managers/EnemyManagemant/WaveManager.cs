
using UnityEngine;
using System.Collections.Generic;
using UI.LevelManagment;

namespace Managers.EnemyManagemant
{
    public class WaveManager : MonoBehaviour
    {
        public static event System.Action OnAllWavesCompleted;

        public WaveData waveData;

        public float spawnInterval { get; private set; } = 0.1f; // Время между появлением врагов внутри волны
        public float waveInterval { get; private set; } = 5f; // Время между волнами
        public bool IsLevelSelected { get; private set; }
        public bool IsWaveActive { get; set; }

        private int _initialEnemyCount;
        private List<int> _waveEnemyCounts;

        private void OnEnable()
        {
            LevelMenuUIManager.OnLevelSelected += HandleLevelSelected;
            Player_Scripts.PlayerCreature.OnPlayerDead += HandlePlayerDead; // Подписываемся на событие проигрыша игрока
        }

        private void OnDisable()
        {
            LevelMenuUIManager.OnLevelSelected -= HandleLevelSelected;
            Player_Scripts.PlayerCreature.OnPlayerDead -= HandlePlayerDead; // Отписываемся от события проигрыша игрока
        }

        private void Awake()
        {
            if (waveData == null)
            {
                Debug.LogError("WaveData is not assigned.");
                return;
            }

            IsLevelSelected = false;
            IsWaveActive = false;
        }

        private void HandleLevelSelected(LevelData levelData)
        {
            _initialEnemyCount = levelData.enemyCount;
            CalculateWaveEnemyCounts();
            IsLevelSelected = true;
            IsWaveActive = true;
        }

        private void CalculateWaveEnemyCounts()
        {
            _waveEnemyCounts = new List<int>();
            _waveEnemyCounts.Add(_initialEnemyCount);
            _waveEnemyCounts.Add(_initialEnemyCount * 2);
            _waveEnemyCounts.Add(_initialEnemyCount * 4);
        }

        public int GetEnemiesForNextWave()
        {
            if (_waveEnemyCounts.Count > 0)
            {
                int enemies = _waveEnemyCounts[0];
                _waveEnemyCounts.RemoveAt(0);
                return enemies;
            }

            IsWaveActive = false;
            OnAllWavesCompleted?.Invoke(); // Вызываем событие завершения уровня
            
            Debug.Log(" спавним ноль хуле");

            return 0;
        }

        private void DecreaseWave() // если в пуле не хватает врагов, то мы понижаем кол-во _enemiesPerWave
        {
            // Этот метод больше не нужен, так как мы не увеличиваем количество врагов
        }

        private void HandlePlayerDead()
        {
            IsWaveActive = false; // Останавливаем волны после проигрыша игрока
        }
    }
}