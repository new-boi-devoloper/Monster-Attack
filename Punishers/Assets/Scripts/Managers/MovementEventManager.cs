using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Managers
{
    public class MovementEventManager : MonoBehaviour
    {
        public static MovementEventManager Instance { get; private set; }

        public delegate void MovementStateHandler(bool canMove);
        public event MovementStateHandler OnMovementStateChanged;

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

        public void SetMovementState(bool canMove)
        {
            OnMovementStateChanged?.Invoke(canMove);
        }
    }
}