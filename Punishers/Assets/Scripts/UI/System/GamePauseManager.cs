using System;
using Managers;
using UnityEngine;
using UI.LevelManagment;

public class GamePauseManager : MonoBehaviour
{
    private void OnEnable()
    {
        LevelMenuUIManager.OnMenuClosed += OnMenuClosed;
        OnMenuClosed();
    }

    private void OnDisable()
    {
        LevelMenuUIManager.OnMenuClosed -= OnMenuClosed;
    }

    public void OnMenuOpened()
    {
        MovementEventManager.Instance.SetMovementState(false);
    }

    public void OnMenuClosed()
    {
        MovementEventManager.Instance.SetMovementState(true);
    }
}