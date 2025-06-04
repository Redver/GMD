using UnityEngine;

public class ClockButtonLogic : IButton
{
    private TimelineClockLogic clock;
    public ClockButtonLogic(TimelineClockLogic TimelineClock)
    {
       clock = TimelineClock; 
    }

    public void activateButton()
    {
        clock.makeNewTimeline();
    }
}
