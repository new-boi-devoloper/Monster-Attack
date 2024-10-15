using UnityEngine;
using YG;

public class ShowOnMobile : MonoBehaviour
{
    void Start()
    {
        // Проверяем, загружены ли данные окружения
        if (YandexGame.SDKEnabled)
        {
            bool isMobile = YandexGame.EnvironmentData.isMobile;
            gameObject.SetActive(isMobile);
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
        bool isMobile = YandexGame.EnvironmentData.isMobile;
        gameObject.SetActive(isMobile);
    }
}