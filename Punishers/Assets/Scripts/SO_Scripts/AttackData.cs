using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "AttackType/Attack1")]
public class AttackData : ScriptableObject
{
    [Header("Shop Display")]
    public Sprite attackSprite;
    public int cost; // Стоимость атаки в убитых врагах
    
    [Header("Base")]
    public int attackId;
    public AttackType attackType;
    
    [Header("Attack Debuffs")]
    public float attackMultiplier;

    [Header("Particle System")]
    public GameObject attackParticleSystemPrefab; // Prefab с ParticleSystem
    
    [HideInInspector]
    public bool IsPurchased;
}
public enum AttackType
{
    Attack1,
    Attack2
}