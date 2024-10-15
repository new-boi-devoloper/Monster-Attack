using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI.LevelManagment
{
    public class LevelMenuUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject levelPrefab; // Префаб уровня
        [SerializeField] private int totalLevels = 20; // Общее количество уровней
        [SerializeField] private GameObject menuWindow;

        public static event System.Action<LevelData> OnLevelSelected; //Отправляет выбранный уровень игроком
        public static event System.Action OnLevelExited; // Добавляем событие выхода с уров
        public static event System.Action OnMenuClosed; // Событие для уведомления о закрытии меню

        private void Start()
        {
            GenerateLevels();
        }

        private void GenerateLevels()
        {
            if (levelPrefab == null)
            {
                Debug.LogError("LevelPrefab is not assigned.");
                return;
            }

            for (int i = 0; i < totalLevels; i++)
            {
                GameObject level = Instantiate(levelPrefab, transform);
                if (level == null)
                {
                    Debug.LogError("Failed to instantiate level prefab.");
                    continue;
                }

                LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
                int currentLevel = WaveLevelProgression.Instance.GetCurrentLevel() + i;
                levelData.Initialize(currentLevel);

                PopulateLevel(level, levelData);

                // Добавьте обработчик события нажатия кнопки
                Button levelButton = level.GetComponentInChildren<Button>();
                if (levelButton != null)
                {
                    levelButton.onClick.AddListener(() =>
                    {
                        OnLevelSelected?.Invoke(levelData);
                        DisableMenuWindow();
                        OnMenuClosed?.Invoke(); // Уведомление о закрытии меню
                    });
                }
                else
                {
                    Debug.LogError("Level button is null.");
                }
            }
        }

        private void PopulateLevel(GameObject level, LevelData data)
        {
            // Проверка на null перед доступом к компонентам
            TextMeshProUGUI rewardText = level.transform.Find("RewardText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI enemyCountText = level.transform.Find("EnemyCountText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI levelNumberText = level.transform.Find("LevelNumberText")?.GetComponent<TextMeshProUGUI>();

            if (rewardText == null)
            {
                Debug.LogError("RewardText not found or not assigned.");
            }
            else
            {
                rewardText.text = data.reward.ToString();
            }

            if (enemyCountText == null)
            {
                Debug.LogError("EnemyCountText not found or not assigned.");
            }
            else
            {
                enemyCountText.text = data.totalEnemies.ToString();
            }

            if (levelNumberText == null)
            {
                Debug.LogError("LevelNumberText not found or not assigned.");
            }
            else
            {
                levelNumberText.text = data.levelNumber.ToString();
            }
        }

        private void DisableMenuWindow()
        {
            if (menuWindow != null)
            {
                menuWindow.SetActive(false);
            }
            else
            {
                Debug.LogError("MenuWindow is not assigned.");
            }
        }
        public void ExitLevel()
        {
            OnLevelExited?.Invoke(); // Вызываем событие выхода с уровня
        }
    }
}
