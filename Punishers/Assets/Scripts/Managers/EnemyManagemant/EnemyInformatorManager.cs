using System;
using UI.LevelManagment;
using UnityEngine;

public class EnemyInformatorManager : MonoBehaviour
{
    public static EnemyInformatorManager Instance { get; private set; }

    public Transform PlayerTransform { get; private set; }
    public LevelData CurrentLevel { get; private set; }

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

    private void OnEnable()
    {
        // Находим игрока при включении объекта
        PlayerTransform = GameObject.FindWithTag("Player")?.transform;
        LevelMenuUIManager.OnLevelSelected += SetCurrentLevel;
    }

    private void OnDisable()
    {
        LevelMenuUIManager.OnLevelSelected -= SetCurrentLevel;
    }
    
    public void SetCurrentLevel(LevelData levelData)
    {
        CurrentLevel = levelData;
        OnLevelWasLoaded();
    }

    private void OnLevelWasLoaded()
    {
        // Обновляем ссылку на игрока при загрузке новой сцены
        PlayerTransform = GameObject.FindWithTag("Player")?.transform;
    }
}