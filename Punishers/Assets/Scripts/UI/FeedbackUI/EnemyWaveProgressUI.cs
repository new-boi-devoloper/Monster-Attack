// using Managers;
// using Managers.EnemyManagemant;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace UI
// {
//     public class EnemyWaveProgressUI : MonoBehaviour
//     {
//         [SerializeField] private Slider progressSlider;
//         [SerializeField] private EnemySpawner enemySpawner;
//         [SerializeField] private WaveManager waveManager;
//
//         private void Start()
//         {
//             if (enemySpawner == null)
//             {
//                 Debug.LogError("EnemySpawner not found.");
//                 return;
//             }
//
//             if (waveManager == null)
//             {
//                 Debug.LogError("WaveManager not found.");
//                 return;
//             }
//
//             // Устанавливаем максимальное значение слайдера
//             progressSlider.maxValue = waveManager.EnemiesPerWave;
//             progressSlider.value = 0; // Устанавливаем начальное значение слайдера в 0
//         }
//
//         private void Update()
//         {
//             UpdateProgress();
//             UpdateMaxValue();
//         }
//
//         private void UpdateProgress()
//         {
//             // Обновляем значение слайдера
//             progressSlider.value = enemySpawner.EnemiesInCombat.Count;
//         }
//
//         private void UpdateMaxValue()
//         {
//             progressSlider.maxValue = waveManager.EnemiesPerWave;
//         }
//     }
// }