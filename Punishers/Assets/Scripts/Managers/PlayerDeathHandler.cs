using System;
using Managers;
using Player_Scripts;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerDeathHandler : MonoBehaviour
{
    public static event Action OnGameContinue;
    
    [SerializeField] private GameObject DeathContainer;

    private void OnEnable()
    {
        PlayerCreature.OnPlayerDead += PlayerDead;
    }

    private void OnDisable()
    {
        PlayerCreature.OnPlayerDead -= PlayerDead;
    }

    public void PlayerDead()
    {
        if (DeathContainer != null)
        {
            DeathContainer.SetActive(true);
            MovementEventManager.Instance.SetMovementState(false);
        }
        else
        {
            Debug.LogError("DeathContainer(UI) in PlayerDeathHandler is noy assign");
        }
    }

    public void RepeatGame()
    { 
        DeathContainer.SetActive(false);
        OnGameContinue?.Invoke();
        MovementEventManager.Instance.SetMovementState(true);
    }
}