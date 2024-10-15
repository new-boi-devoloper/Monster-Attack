using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Level Data", order = 51)]
public class LevelData : ScriptableObject
{
    public int levelNumber;
    public int reward;
    public int enemyCount;
    public int totalEnemies;

    public void Initialize(int levelNumber)
    {
        this.levelNumber = levelNumber;
        reward = WaveLevelProgression.Instance.GetLevelCompletionReward(levelNumber);
        enemyCount = WaveLevelProgression.Instance.GetEnemyCount(levelNumber);
        totalEnemies = enemyCount + (enemyCount * 2) + (enemyCount * 4); // Пример: totalEnemies может быть вычислен по-другому
    }
}