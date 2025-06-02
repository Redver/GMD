using System;
using Resources.Features.MenuFeatures.Logic;
using TMPro;
using UnityEngine;

public class volumeDownView : ButtonUi
{
    private IButton _volumeDownButton;
    [SerializeField] private TextMeshProUGUI _text;
    private void Awake()
    {
        _volumeDownButton = new volumeDownLogic();
    }

    private void Start()
    {
        updateText();
    }

    public override void activateButton()
    {
        _volumeDownButton.activateButton();
        updateText();
    }

    public void updateText()
    {
        _text.text = Mathf.Round(AudioListener.volume * 100f).ToString();
    }
}
