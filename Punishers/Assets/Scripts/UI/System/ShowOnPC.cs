using UnityEngine;
using YG;

public class ShowOnPC : MonoBehaviour
{
    void Start()
    {
        // Проверяем, загружены ли данные окружения
        if (YandexGame.SDKEnabled)
        {
            bool isPC = YandexGame.EnvironmentData.isDesktop;
            gameObject.SetActive(isPC);
        }
        else
        {
            // Добавляем слушатель для загрузки данных после инициализации SDK
            YandexGame.GetDataEvent += OnEnvironmentDataLoaded;
        }
    }

    void OnEnvironmentDataLoaded()
    {
        // Удаляем слушатель, так как данные уже загружены
        YandexGame.GetDataEvent -= OnEnvironmentDataLoaded;

        // Используем данные из YandexGame.EnvironmentData
        bool isPC = YandexGame.EnvironmentData.isDesktop;
        gameObject.SetActive(isPC);
    }
}