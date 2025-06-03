using Resources.Features.MenuFeatures.Logic;
using TMPro;
using UnityEngine;

public class UpTimeLineView : ButtonUi
{
    private IButton previousTimeLine;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject clock;
    private TimelineClockLogic timelineClockLogic;
    
    private void Awake()
    {
        timelineClockLogic = clock.GetComponent<TimelineClockLogic>();
    }

    private void Start()
    {
        previousTimeLine = new UpTimelineLogic(timelineClockLogic);
        updateText();
    }

    public override void activateButton()
    {
        previousTimeLine.activateButton();
        updateText();
    }

    public void updateText()
    {
        text.text = timelineClockLogic.getCurrentTurn().ToString();
    }
}