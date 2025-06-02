using Resources.Features.MenuFeatures.Logic;
using TMPro;
using UnityEngine;

public class volumeUpView : ButtonUi
{
private IButton _volumeUpButton;
[SerializeField] private TextMeshProUGUI _text;
private void Start()
{
    _volumeUpButton = new volumeDownLogic();
    updateText();
}

public override void activateButton()
{
    _volumeUpButton.activateButton();
    updateText();
}

public void updateText()
{
    _text.text = AudioListener.volume.ToString();
}
}
