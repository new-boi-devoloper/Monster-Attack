using UnityEngine;

namespace SO_Scripts
{
    [CreateAssetMenu(fileName = "NewRewardProgression", menuName = "Progression/Linear Progression", order = 53)]
    public class LinearProgressionData : ScriptableObject
    {
        [SerializeField] private float baseDivider = 2.5f;
        [SerializeField] private float rewardStartPoint = 10f;

        public float CalculateEnemyKillReward(int level)
        {
            return Mathf.RoundToInt((level / baseDivider + rewardStartPoint)/2.5f);
        }

        public float CalculateLevelCompletionReward(int level)
        {
            return Mathf.RoundToInt((level / baseDivider) * 2 + rewardStartPoint * 2);
        }
    }
}