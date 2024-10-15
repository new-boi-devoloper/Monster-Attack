using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using YG;

public class UserStatsDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinDisplay;

    private void OnEnable()
    {
        UserInfo.OnCoinChanged += ShowCoins;
        UserInfo.OnDataChanged += ShowCoins;
        ShowCoins(UserInfo.Instance.CoinCount);
    }

    private void OnDisable()
    {
        UserInfo.OnDataChanged -= ShowCoins;
        UserInfo.OnCoinChanged -= ShowCoins;
    }

    private void Start()
    {
        if (coinDisplay == null)
        {
            Debug.LogError("coinDisplay is not assigned.");
        }
    }

    private void ShowCoins(int userCoins)
    {
        if (coinDisplay != null)
        {
            coinDisplay.text = $"{userCoins}";
        }
        else
        {
            Debug.LogError("coinDisplay is null.");
        }
    }

    private void ShowCoins()
    {
        if (coinDisplay != null)
        {
            coinDisplay.text = $"{UserInfo.Instance.CoinCount}";
        }
        else
        {
            Debug.LogError("coinDisplay is null.");
        }
    }
}