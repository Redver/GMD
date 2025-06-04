using Resources.Features.MenuFeatures.Logic;
using TMPro;
using UnityEngine;

public class BackTimeArrowView : ButtonUi
{
    private IButton backButton;
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
        backButton = new BackTimeArrowLogic(timelineClockLogic);
        clockButtonView = clock.GetComponent<ClockButtonView>();
        updateText();
    }

    public override void activateButton()
    {
        backButton.activateButton();
        clockButtonView.updateIfClockIsActive();
        updateText();
    }

    public void updateText()
    {
        text.text = timelineClockLogic.getCurrentTurn().ToString();
    }
}
