using UnityEngine;

namespace Enemy_Scripts
{
    public class EnemyFlyingEye : AbstractEnemy
    {
        [SerializeField] private EnemyData enemyData;
        [SerializeField] private float attackPowerModifier = 1.2f; // Модификатор силы атаки
        [SerializeField] private float healthModifier = 0.8f; // Модификатор здоровья

        private bool _isTouchingPlayer;
        private bool _isDead;
        private EnemyAnimator _enemyAnimator;

        private void Start()
        {
            _enemyAnimator = GetComponent<EnemyAnimator>();
            if (enemyData == null)
            {
                Debug.LogError("enemyData is not assigned. Please assign it in the inspector.");
                return;
            }

            SaveBaseStatsFromSO(enemyData); // Сохраняем базовые значения из SO
            Health *= healthModifier;
            AttackPower *= attackPowerModifier;
            PoolId = enemyData.poolId;
            IsTouchingPlayer = _isTouchingPlayer;
            IsDead = _isDead;
        }

        protected override bool IsTouchingPlayer
        {
            get => _isTouchingPlayer;
            set => _isTouchingPlayer = value;
        }

        protected override bool IsDead
        {
            get => _isDead;
            set => _isDead = value;
        }

        public override void ChangeHealth(float amount)
        {
            base.ChangeHealth(amount);
            if (_enemyAnimator != null)
            {
                _enemyAnimator.TakeHitAnim();
            }
            else
            {
                Debug.Log($"No assigned animation to {gameObject} (TakeHit)");
            }
        }

        protected override void Attack()
        {
            base.Attack();
            if (_enemyAnimator != null)
            {
                _enemyAnimator.AttackAnim();
            }
            else
            {
                Debug.Log($"No assigned animation to {gameObject} (Attack)");
            }
        }

        protected override void OnCollisionEnter2D(Collision2D other)
        {
            base.OnCollisionEnter2D(other);
        }
        public override void UpdateStats(int currentLevel)
        {
            // Вызываем базовую реализацию
            base.UpdateStats(currentLevel);
        }

        protected override void OnEnemyDeath(GameObject obj)
        {
            base.OnEnemyDeath(obj);
            if (_enemyAnimator != null)
            {
                _enemyAnimator.DeathAnim(obj);
            }
            else
            {
                Debug.Log($"No assigned animation to {gameObject} (Death)");
            }
        }

        public override void OnObjectSpawn()
        {
            base.OnObjectSpawn();
            Health = enemyData.health * healthModifier; // Reset health when object is spawned
            AttackPower = enemyData.attackPower * attackPowerModifier; // Reset attack power when object is spawned
        }
    }
}