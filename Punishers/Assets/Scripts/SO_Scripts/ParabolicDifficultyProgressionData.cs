using UnityEngine;

namespace SO_Scripts
{
    [CreateAssetMenu(fileName = "NewGraphicProgression", menuName = "Progression/Parabolic Progression", order = 52)]
    public class ParabolicDifficultyProgressionData : ScriptableObject
    {
        [SerializeField] private float growthCoefficient = 1.9999f;
        [SerializeField] private float baseDivider = 10f;
        [SerializeField] private float graphicStartPoint = 1f;

        public float CalculateValue(float x)
        {
            return (Mathf.Pow(x, growthCoefficient) / baseDivider) + graphicStartPoint;
        }
    }
}