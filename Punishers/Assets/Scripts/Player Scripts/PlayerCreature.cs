using System;
using Enemy_Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player_Scripts
{
    public class PlayerCreature : MonoBehaviour
    {
        public static event Action OnPlayerDead;
        public static event Action OnAttack1;
        public static event Action OnAttack2;

        [SerializeField] internal PlayerData playerData;
        [SerializeField] private GameObject attack1ZoneCheck;
        [SerializeField] private GameObject attack2ZoneCheck;
        [SerializeField] private Transform startPosition; // Добавляем стартовую позицию
        [SerializeField] private TextMeshProUGUI healthUI; // Добавляем стартовую позицию

        public AttackData currentAttack1;
        public AttackData currentAttack2;

        public CooldownTimer cooldownTimer;

        private PlayerAnimator _playerAnimator;
        private PlayerMovement _playerMovement;

        private PlayAttackAnim _playAttack1Anim;
        private PlayAttackAnim _playAttack2Anim;

        public bool IsDead { get; private set; }

        private void OnEnable()
        {
            playerData.health = 100;
            PlayerDeathHandler.OnGameContinue += ContinueGame;
            Managers.UserInfo.OnHealthBonus += AddHealth; // Подписываемся на событие бонуса здоровья
            Managers.EnemyManagemant.WaveManager.OnAllWavesCompleted +=
                HandleLevelCompleted; // Подписываемся на событие завершения уровня
            ApplyHealthBonus(); // Применяем бонус здоровья при активации
            HealthChangedUI();
        }

        private void OnDisable()
        {
            PlayerDeathHandler.OnGameContinue -= ContinueGame;
            Managers.UserInfo.OnHealthBonus -= AddHealth; // Отписываемся от события бонуса здоровья
            Managers.EnemyManagemant.WaveManager.OnAllWavesCompleted -=
                HandleLevelCompleted; // Отписываемся от события завершения уровня
        }

        private void Start()
        {
            if (playerData == null)
            {
                Debug.LogError("PlayerData is not assigned in PlayerCreature.");
                return;
            }
            
            IsDead = false;

            _playAttack1Anim = attack1ZoneCheck.GetComponent<PlayAttackAnim>();
            _playAttack2Anim = attack2ZoneCheck.GetComponent<PlayAttackAnim>();

            _playerAnimator = GetComponent<PlayerAnimator>();
            _playerMovement = GetComponent<PlayerMovement>();

            cooldownTimer = GetComponent<CooldownTimer>();
        }

        private void Update()
        {
            HealthChangedUI();
        }

        public void ChangeHealth(float amount)
        {
            if (playerData.health - amount <= 0)
            {
                _playerAnimator.DeathAnim(IsDead);
                OnPlayerDead?.Invoke(); // Вызываем событие проигрыша игрока
                MoveToStartPosition(); // Перемещаем игрока в стартовую позицию после проигрыша
            }

            _playerAnimator.TakeHitAnim();
            playerData.health -= amount;
            HealthChangedUI();

            Debug.Log($"player health {playerData.health}");
        }

        private void HealthChangedUI()
        {
            healthUI.text = $"{(int)Math.Ceiling(playerData.health)}";
        }

        private void ContinueGame()
        {
            playerData.health = 100;
            HealthChangedUI();
        }

        public void Attack1(InputAction.CallbackContext context)
        {
            if (currentAttack1 != null)
            {
                _playAttack1Anim.PlayParticleSystem(currentAttack1.attackParticleSystemPrefab,
                    attack1ZoneCheck.transform.position, _playerMovement.IsFacingRight);

                _playerAnimator.Attack1Anim();

                AttackCalculation(currentAttack1, attack1ZoneCheck, playerData.attack1Range);

                // Вызываем событие для атаки 1
                OnAttack1?.Invoke();
            }
        }

        public void Attack2(InputAction.CallbackContext context)
        {
            if (currentAttack2 != null && cooldownTimer != null && cooldownTimer.CooldownTime.Value <= 0)
            {
                float scaleMultiplier = playerData.attack2Range.x;

                _playAttack2Anim.PlayParticleSystem(currentAttack2.attackParticleSystemPrefab,
                    attack2ZoneCheck.transform.position, _playerMovement.IsFacingRight, scaleMultiplier);

                _playerAnimator.Attack2Anim();

                AttackCalculation(currentAttack2, attack2ZoneCheck, playerData.attack2Range);

                cooldownTimer.StartCooldown(playerData.attack2Cooldown);

                // Вызываем событие для атаки 2
                OnAttack2?.Invoke();
            }
        }

        private void AttackCalculation(AttackData attack, GameObject zoneCheck, Vector2 attackRange)
        {
            var attackPosition = zoneCheck.gameObject.transform.position;

            var hitColliders = Physics2D.OverlapBoxAll(
                attackPosition,
                attackRange,
                0,
                playerData.attackLayer);

            foreach (var hitCollider in hitColliders)
            {
                Debug.Log("Hit " + hitCollider.name);

                var abstractEnemy = hitCollider.GetComponent<AbstractEnemy>();
                if (abstractEnemy != null)
                {
                    abstractEnemy.ChangeHealth(playerData.attackPower * attack.attackMultiplier);
                }
            }
        }

        // Метод для добавления здоровья с проверкой на максимальное значение
        private void AddHealth(float amount)
        {
            float newHealth = playerData.health + amount;
            if (newHealth > 200)
            {
                newHealth = 200;
            }

            playerData.health = newHealth;
            HealthChangedUI();

            Debug.Log($"Player health increased by {amount}. New health: {playerData.health}");
        }

        // Метод для перемещения игрока в стартовую позицию
        private void MoveToStartPosition()
        {
            if (startPosition != null)
            {
                transform.position = startPosition.position;
                HealthChangedUI();
            }
            else
            {
                Debug.LogWarning("Start position is not assigned.");
            }
        }

        // Обработчик события завершения уровня
        private void HandleLevelCompleted()
        {
            playerData.health = 100;
            MoveToStartPosition(); // Перемещаем игрока в стартовую позицию после завершения уровня
            HealthChangedUI();
        }

        // Метод для применения бонуса здоровья
        private void ApplyHealthBonus()
        {
            float healthBonus = Managers.UserInfo.Instance.HealthBonus;
            if (healthBonus > 0)
            {
                AddHealth(healthBonus);
                Managers.UserInfo.Instance.AddHealthBonus(-healthBonus); // Сбрасываем бонус здоровья
                HealthChangedUI();

            }
            HealthChangedUI();

        }
    }
}