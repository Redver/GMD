using UnityEngine;

public class DownTimeLineLogic :  Button
{
    private TimelineClockLogic turnLogic;
    public DownTimeLineLogic(TimelineClockLogic timelineClockLogic)
    {
        turnLogic = timelineClockLogic;
    }

    public override void activateButton()
    {
        turnLogic.showNextTimeline();
    }
}
