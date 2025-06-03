using UnityEngine;

public class UpTimelineLogic :  Button
{
    private TimelineClockLogic turnLogic;
    public UpTimelineLogic(TimelineClockLogic timelineClockLogic)
    {
        turnLogic = timelineClockLogic;
    }

    public override void activateButton()
    {
        turnLogic.showPreviousTimeline();
    }
}
