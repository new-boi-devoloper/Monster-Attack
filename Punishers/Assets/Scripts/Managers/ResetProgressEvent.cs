using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetProgressEvent : MonoBehaviour
{
    public static event Action OnResetSaveClick;

    public void ResetData()
    {
        OnResetSaveClick?.Invoke();
    }
}
