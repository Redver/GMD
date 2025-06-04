using Resources.Features.MenuFeatures.Logic;
using TMPro;
using UnityEngine;

public class ClockButtonView : ButtonUi
{
    private IButton clockButton;
    [SerializeField] private GameObject clock;
    private TimelineClockLogic timelineClockLogic;
    
    private void Awake()
    {
        timelineClockLogic = clock.GetComponent<TimelineClockLogic>();
    }

    private void Start()
    {
        clockButton = new ClockButtonLogic(timelineClockLogic);
    }

    public override void activateButton()
    {
        clockButton.activateButton();
    }
}
