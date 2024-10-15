using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "WaveData", menuName = "ManagersSO/WaveData", order = 1)]
public class WaveData : ScriptableObject
{
    public int enemiesPerWave = 2; // Количество врагов в каждой волне
    public float enemyIncreaseCoeff = 1.5f; // Коэффициент увеличения количества врагов
    public float spawnInterval = 1.0f; // Интервал между появлением врагов в волне
    public float waveInterval = 5f; // Интервал появления волн
}