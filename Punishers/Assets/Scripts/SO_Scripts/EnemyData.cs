using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemySO/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("for system use! DONT TOUCH")]
    public Transform playerTransform;
    [Header("Management")]
    public int poolId;
    [Header("Enemy stats")]
    public float speed;
    public float attackPower;
    public float health;
}