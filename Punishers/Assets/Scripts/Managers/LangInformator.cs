using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class LangInformator : MonoBehaviour
{
    [System.Serializable]
    public class LangGameObjectPair
    {
        public GameObject gameObject;
        public string langID;
    }
    
    private string _chosenLanguage;
    [SerializeField] private List<LangGameObjectPair> langGameObjectPairs;
    private const string FirstStartKey = "FirstStart";

    private void OnEnable()
    {
        YandexGame.SwitchLangEvent += OnLanguageSwitched;
        _chosenLanguage = YandexGame.lang;
    }

    private void OnDisable()
    {
        YandexGame.SwitchLangEvent -= OnLanguageSwitched;
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey(FirstStartKey))
        {
            PlayerPrefs.SetInt(FirstStartKey, 1);
            PlayerPrefs.Save();
            UpdateGameObjects(true);
        }
        else
        {
            UpdateGameObjects(false);
        }
    }

    private void OnLanguageSwitched(string newLanguage)
    {
        _chosenLanguage = newLanguage;
        UpdateGameObjects(false);
    }

    private void UpdateGameObjects(bool b)
    {
        foreach (var pair in langGameObjectPairs)
        {
            if (pair.langID == _chosenLanguage)
            {
                pair.gameObject.SetActive(true);
            }
            else
            {
                pair.gameObject.SetActive(false);
            }
        }
    }
}