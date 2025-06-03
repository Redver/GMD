using UnityEngine;

public class ForwardTimeArrowLogic :  Button
{
    private TimelineClockLogic turnLogic;
    public ForwardTimeArrowLogic(TimelineClockLogic timelineClockLogic)
    {
        turnLogic = timelineClockLogic;
    }

    public override void activateButton()
    {
        turnLogic.showNextTurn();
    }
}