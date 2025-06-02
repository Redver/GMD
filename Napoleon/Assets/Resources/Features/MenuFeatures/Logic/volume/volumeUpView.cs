using System;
using Resources.Features.MenuFeatures.Logic;
using TMPro;
using UnityEngine;

public class volumeUpView : ButtonUi
{
private IButton _volumeUpButton;
[SerializeField] private TextMeshProUGUI _text;
private void Awake()
{
    _volumeUpButton = new volumeUpLogic();
    updateText();
}

private void Start()
{
    updateText();
}

public override void activateButton()
{
    _volumeUpButton.activateButton();
    updateText();
}

public void updateText()
{
    _text.text = Mathf.Round(AudioListener.volume * 100f).ToString();
}
}
