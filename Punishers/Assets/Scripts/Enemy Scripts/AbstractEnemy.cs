using System;
using Cinemachine;
using Player_Scripts;
using UI.LevelManagment;
using UnityEngine;

namespace Enemy_Scripts
{
    public abstract class AbstractEnemy : MonoBehaviour, IPooledObject
    {
        public static event Action<GameObject> OnEnemyDead;

        public int PoolId { get; set; }

        protected PlayerCreature PlayerCreature;

        protected float Health { get; set; }
        protected float AttackPower { get; set; }
        protected virtual bool IsTouchingPlayer { get; set; }
        protected virtual bool IsDead { get; set; }

        public virtual void ChangeHealth(float amount)
        {
            Health -= amount;
            if (Health <= 0)
            {
                OnEnemyDeath(gameObject);
            }
        }

        protected virtual void Attack()
        {
            if (PlayerCreature != null)
            {
                PlayerCreature.ChangeHealth(AttackPower);
            }
            else
            {
                Debug.Log("no reference to player");
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                PlayerCreature = other.gameObject.GetComponent<PlayerCreature>();
                if (PlayerCreature != null)
                {
                    Attack();
                    IsTouchingPlayer = true;
                }
                else
                {
                    Debug.Log("PlayerCreature component not found on the player object");
                }
            }
        }

        protected virtual void OnEnemyDeath(GameObject obj)
        {
            IsDead = true;
            OnEnemyDead?.Invoke(obj);
        }

        public virtual void OnObjectSpawn()
        {
            IsDead = false;
            RestoreBaseStats();
            UpdateStats(EnemyInformatorManager.Instance.CurrentLevel.levelNumber);
        }

        // Метод для обновления статистики врага на основе текущего уровня
        public virtual void UpdateStats(int currentLevel)
        {
            Health *= WaveLevelProgression.Instance.GetEnemyHealth(currentLevel);
            AttackPower *= WaveLevelProgression.Instance.GetEnemyAttackPower(currentLevel);
        }

        // Метод для сохранения базовых значений из SO
        protected virtual void SaveBaseStatsFromSO(EnemyData enemyData)
        {
            Health = enemyData.health;
            AttackPower = enemyData.attackPower;
        }

        // Метод для восстановления базовых значений
        protected virtual void RestoreBaseStats()
        {
            // Здесь не нужно ничего делать, так как Health и AttackPower уже сохранены из SO
        }
    }
}