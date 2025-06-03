using UnityEngine;

public class BackTimeArrowLogic : Button
{
    private TimelineClockLogic turnLogic;
    public BackTimeArrowLogic(TimelineClockLogic timelineClockLogic)
    {
        turnLogic = timelineClockLogic;
    }

    public override void activateButton()
    {
        turnLogic.showPreviousTurn();
    }
}
