using Resources.Features.MenuFeatures.Logic;
using TMPro;
using UnityEngine;

public class DownTimeLineView : ButtonUi
{
    private IButton nextTimeLine;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject clock;
    private TimelineClockLogic timelineClockLogic;
    
    private void Awake()
    {
        timelineClockLogic = clock.GetComponent<TimelineClockLogic>();
    }

    private void Start()
    {
        nextTimeLine = new DownTimeLineLogic(timelineClockLogic);
        updateText();
    }

    public override void activateButton()
    {
        nextTimeLine.activateButton();
        updateText();
    }

    public void updateText()
    {
        text.text = timelineClockLogic.getCurrentTurn().ToString();
    }
}