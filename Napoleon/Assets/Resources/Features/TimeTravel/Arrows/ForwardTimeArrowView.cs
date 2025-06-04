using Resources.Features.MenuFeatures.Logic;
using TMPro;
using UnityEngine;

public class ForwardTimeArrowScript : ButtonUi
{
    private IButton forwardButton;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject clock;
    private TimelineClockLogic timelineClockLogic;
    private ClockButtonView clockButtonView;

    
    private void Awake()
    {
        timelineClockLogic = clock.GetComponent<TimelineClockLogic>();
    }

    private void Start()
    {
        forwardButton = new ForwardTimeArrowLogic(timelineClockLogic);
        clockButtonView = clock.GetComponent<ClockButtonView>();
        updateText();
    }

    public override void activateButton()
    {
        forwardButton.activateButton();
        clockButtonView.updateIfClockIsActive();
        updateText();
    }

    public void updateText()
    {
        text.text = timelineClockLogic.getCurrentTurn().ToString();
    }
}
