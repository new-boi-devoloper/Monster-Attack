using SO_Scripts;
using UnityEngine;
using UnityEngine.Serialization;

public class WaveLevelProgression : MonoBehaviour
{
    public static WaveLevelProgression Instance { get; private set; }

    [SerializeField] private ParabolicDifficultyProgressionData parabolicDifficultyProgression;
    [SerializeField] private LinearProgressionData linearProgressionData;
    [SerializeField] private ParabolicDifficultyProgressionData slowparabolicDifficultyProgression;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetCurrentLevel()
    {
        return 1; // меняется внутри LevelMenuUIManager 
    }

    public int GetEnemyCount(int level)
    {
        // Пример: возвращаем текущее количество врагов на основе уровня
        return Mathf.RoundToInt(parabolicDifficultyProgression.CalculateValue(level));
    }

    public int GetEnemyKillReward(int level)
    {
        return Mathf.RoundToInt(linearProgressionData.CalculateEnemyKillReward(level)/2.5f);
    }

    public int GetLevelCompletionReward(int level)
    {
        return Mathf.RoundToInt(linearProgressionData.CalculateLevelCompletionReward(level));
    }

    public float GetEnemyProgressionValue(int level)
    {
        return parabolicDifficultyProgression.CalculateValue(level);
    }

    public float GetEnemyHealth(int level)
    {
        return slowparabolicDifficultyProgression.CalculateValue(level);
    }

    public float GetEnemyAttackPower(int level)
    {
        return slowparabolicDifficultyProgression.CalculateValue(level);
    }
}