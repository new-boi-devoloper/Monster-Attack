using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerSO/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Base Variables")]
    public float health;
    
    [Header("Movement Variables")]
    public float speed;
    public float jumpingPower;

    [Header("Attack Variables")]
    public float attackPower;
    public Vector2 attack1Range; // Размер атаки 1
    public Vector2 attack2Range; // Размер атаки 2
    public LayerMask attackLayer; // Слой коллайдеров для атаки
    
    [Header("Cooldown Variables")]
    public float attack2Cooldown; // Время зарядки для Attack2
}