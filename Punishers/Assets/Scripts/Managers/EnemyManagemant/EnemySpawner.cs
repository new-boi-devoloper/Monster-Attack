using System;
using System.Collections.Generic;
using Enemy_Scripts;
using UI.LevelManagment;
using UniRx;
using UnityEngine;


namespace Managers.EnemyManagemant
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<int> enemiesIds; // Список идентификаторов пулов для врагов
        public List<GameObject> EnemiesInCombat { get; private set; }

        private ObjectPooler _objectPooler;
        private UserInfo _userInfo;
        private bool _allEnemiesDead = true;

        private List<Transform> _randomSpawnPositions; // Список позиций для спавна врагов
        private WaveManager _waveManager;
        private int _currentLevel;

        private void OnEnable()
        {
            AbstractEnemy.OnEnemyDead += EnemyKilled;
            LevelMenuUIManager.OnLevelSelected += HandleLevelSelected;
            Player_Scripts.PlayerCreature.OnPlayerDead += HandlePlayerDead; // Подписываемся на событие проигрыша игрока
            LevelMenuUIManager.OnLevelExited += ReturnAllEnemiesToPool; // Подписываемся на событие выхода с уровня
        }

        private void OnDisable()
        {
            AbstractEnemy.OnEnemyDead -= EnemyKilled;
            LevelMenuUIManager.OnLevelSelected -= HandleLevelSelected;
            Player_Scripts.PlayerCreature.OnPlayerDead -= HandlePlayerDead; // Отписываемся от события проигрыша игрока
            LevelMenuUIManager.OnLevelExited -= ReturnAllEnemiesToPool; // Отписываемся от события выхода с уровня
        }

        private void Start()
        {
            EnemiesInCombat = new List<GameObject>();
            _randomSpawnPositions = new List<Transform>();

            _objectPooler = ObjectPooler.Instance;
            if (_objectPooler == null)
            {
                Debug.LogError("ObjectPooler instance is null. Make sure it is initialized.");
            }

            _userInfo = UserInfo.Instance;
            if (_userInfo == null)
            {
                Debug.LogError("UserData instance is null. Make sure it is initialized.");
            }

            _waveManager = GetComponent<WaveManager>();
            if (_waveManager == null)
            {
                Debug.LogError("WaveManager component is missing. Make sure it is attached to the same GameObject.");
            }

            // Автоматически заполняем список позиций спавна
            var allChildren = GetComponentsInChildren<Transform>();
            foreach (var child in allChildren)
            {
                if (child != transform) // Исключаем родительский объект
                {
                    _randomSpawnPositions.Add(child);
                }
            }

            StartWaveSystem();
        }

        private void HandleLevelSelected(LevelData levelData)
        {
            _currentLevel = levelData.levelNumber;
        }

        private void StartWaveSystem()
        {
            Observable.EveryUpdate()
                .Where(_ => _waveManager.IsLevelSelected && _waveManager.IsWaveActive &&
                            (_allEnemiesDead || EnemiesInCombat.Count == 0))
                .ThrottleFirst(TimeSpan.FromSeconds(_waveManager.waveInterval))
                .Subscribe(_ => SpawnEnemies())
                .AddTo(this);
        }

        private void SpawnEnemies()
        {
            _allEnemiesDead = false;
            int enemiesToSpawn = _waveManager.GetEnemiesForNextWave();
            if (enemiesToSpawn == 0)
            {
                return;
            }

            var spawnInterval = _waveManager.spawnInterval;

            Observable.Interval(TimeSpan.FromSeconds(spawnInterval))
                .Take(enemiesToSpawn)
                .Subscribe(_ =>
                {
                    var randomId = PickRandomId();
                    var randomPosition = PickRandomPosition();

                    var enemy = _objectPooler.SpawnFromPool(randomId, randomPosition.position, randomPosition.rotation);
                    if (enemy != null)
                    {
                        var pooledObj = enemy.GetComponent<IPooledObject>();
                        if (pooledObj != null)
                        {
                            pooledObj.PoolId = randomId; // Устанавливаем идентификатор пула
                        }

                        var abstractEnemy = enemy.GetComponent<AbstractEnemy>();
                        if (abstractEnemy != null)
                        {
                            abstractEnemy.UpdateStats(_currentLevel);
                        }

                        EnemiesInCombat.Add(enemy);
                    }
                }).AddTo(this);
        }

        private void EnemyKilled(GameObject enemy)
        {
            //Coin Earn
            var earnedCoins = WaveLevelProgression.Instance.GetEnemyKillReward(_currentLevel);
            _userInfo.ChangeCoin(earnedCoins);
            UserInfo.Instance.IncreaseEnemiesKilled();
            
            if (EnemiesInCombat.Remove(enemy))
            {
                var pooledObj = enemy.GetComponent<IPooledObject>();
                if (pooledObj != null)
                {
                    int poolId = pooledObj.PoolId; // Получаем идентификатор пула
                    _objectPooler.ReturnToPool(poolId, enemy);
                }
            }
        }

        private int PickRandomId()
        {
            if (enemiesIds == null || enemiesIds.Count == 0)
            {
                throw new ArgumentException("The list of enemy IDs cannot be null or empty.");
            }

            var randomIndex = UnityEngine.Random.Range(0, enemiesIds.Count);
            return enemiesIds[randomIndex];
        }

        private Transform PickRandomPosition()
        {
            if (_randomSpawnPositions == null || _randomSpawnPositions.Count == 0)
            {
                throw new ArgumentException("The list of spawn positions cannot be null or empty.");
            }

            int randomIndex = UnityEngine.Random.Range(0, _randomSpawnPositions.Count);
            return _randomSpawnPositions[randomIndex];
        }

        private void HandlePlayerDead()
        {
            // Возвращаем всех врагов на сцене обратно в пул
            foreach (var enemy in EnemiesInCombat)
            {
                var pooledObj = enemy.GetComponent<IPooledObject>();
                if (pooledObj != null)
                {
                    int poolId = pooledObj.PoolId; // Получаем идентификатор пула
                    _objectPooler.ReturnToPool(poolId, enemy);
                }
            }

            EnemiesInCombat.Clear(); // Очищаем список врагов в бою
            _waveManager.IsWaveActive = false; // Останавливаем волны
        }

        private void ReturnAllEnemiesToPool()
        {
            // Возвращаем всех врагов на сцене обратно в пул
            foreach (var enemy in EnemiesInCombat)
            {
                var pooledObj = enemy.GetComponent<IPooledObject>();
                if (pooledObj != null)
                {
                    int poolId = pooledObj.PoolId; // Получаем идентификатор пула
                    _objectPooler.ReturnToPool(poolId, enemy);
                }
            }

            EnemiesInCombat.Clear(); // Очищаем список врагов в бою
            _waveManager.IsWaveActive = false; // Останавливаем волны
        }
    }
}