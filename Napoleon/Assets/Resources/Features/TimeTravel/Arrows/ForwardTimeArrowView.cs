using Resources.Features.MenuFeatures.Logic;
using TMPro;
using UnityEngine;

public class ForwardTimeArrowScript : ButtonUi
{
    private IButton forwardButton;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject clock;
    private TimelineClockLogic timelineClockLogic;
    
    private void Awake()
    {
        timelineClockLogic = clock.GetComponent<TimelineClockLogic>();
    }

    private void Start()
    {
        forwardButton = new ForwardTimeArrowLogic(timelineClockLogic);
        updateText();
    }

    public override void activateButton()
    {
        forwardButton.activateButton();
        updateText();
    }

    public void updateText()
    {
        text.text = timelineClockLogic.getCurrentTurn().ToString();
    }
}
