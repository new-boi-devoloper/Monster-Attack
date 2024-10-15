using Managers;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IPausedMovement
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemyType enemyType;

    [Header("Speed Modifiers")] [SerializeField]
    private float goblinSpeedModifier = 2.0f;

    [SerializeField] private float flyingEyeSpeedModifier = 1.5f;
    [SerializeField] private float mushroomSpeedModifier = 0.8f;
    [SerializeField] private float skeletonSpeedModifier = 1.2f;

    public bool _isFacingRight { get; private set; } = true;
    private bool _canMove = true;

    private void OnEnable()
    {
        // Проверяем, что playerTransform в enemyData не null
        if (enemyData.playerTransform == null)
        {
            if (EnemyInformatorManager.Instance == null)
            {
                Debug.LogError("EnemyInformatorManager instance is null. Make sure it is initialized.");
                return;
            }

            if (EnemyInformatorManager.Instance.PlayerTransform == null)
            {
                Debug.LogError("PlayerTransform in EnemyInformatorManager is null. Make sure it is initialized.");
                return;
            }

            enemyData.playerTransform = EnemyInformatorManager.Instance.PlayerTransform;
        }

        // Подписываемся на событие изменения состояния движения
        if (MovementEventManager.Instance != null)
        {
            MovementEventManager.Instance.OnMovementStateChanged += SetMovementState;
        }
        else
        {
            Debug.LogError("MovementEventManager instance is null. Make sure it is initialized.");
        }
    }

    private void OnDestroy()
    {
        // Отписываемся от события при уничтожении объекта
        MovementEventManager.Instance.OnMovementStateChanged -= SetMovementState;
    }

    private void Update()
    {
        if (_canMove)
        {
            MoveToPlayer();
            Flip();
        }
    }

    private void MoveToPlayer()
    {
        if (enemyData.playerTransform)
        {
            // Используем MoveTowards для более плавного перемещения
            transform.position = Vector2.MoveTowards(
                transform.position,
                enemyData.playerTransform.position,
                GetModifiedSpeed() * Time.deltaTime);
        }
    }

    private float GetModifiedSpeed()
    {
        switch (enemyType)
        {
            case EnemyType.Goblin:
                return enemyData.speed * goblinSpeedModifier;
            case EnemyType.FlyingEye:
                return enemyData.speed * flyingEyeSpeedModifier;
            case EnemyType.Mushroom:
                return enemyData.speed * mushroomSpeedModifier;
            case EnemyType.Skeleton:
                return enemyData.speed * skeletonSpeedModifier;
            default:
                return enemyData.speed;
        }
    }

    private void Flip()
    {
        if (enemyData.playerTransform)
        {
            Vector3 directionToPlayer = enemyData.playerTransform.position - transform.position;

            if (directionToPlayer.x > 0 && !_isFacingRight)
            {
                // Игрок справа, враг должен смотреть вправо
                _isFacingRight = true;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
            else if (directionToPlayer.x < 0 && _isFacingRight)
            {
                // Игрок слева, враг должен смотреть влево
                _isFacingRight = false;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }
    }

    public void SetMovementState(bool canMove)
    {
        _canMove = canMove;
    }

    public enum EnemyType
    {
        Goblin,
        FlyingEye,
        Mushroom,
        Skeleton
    }
}