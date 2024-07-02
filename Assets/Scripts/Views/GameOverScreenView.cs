using System;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreenView : ViewBase
{
    [SerializeField] private Button reloadButton;
    
    public Action OnClickReloadButton;
    
    private void OnEnable()
    {
        reloadButton.onClick.AddListener(() => OnClickReloadButton?.Invoke());
    }

    private void OnDisable()
    {
        reloadButton.onClick.RemoveAllListeners();
    }
}
