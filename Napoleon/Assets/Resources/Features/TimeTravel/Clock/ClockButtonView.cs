using Resources.Features.MenuFeatures.Logic;
using TMPro;
using UnityEngine;

public class ClockButtonView : ButtonUi
{
    private IButton clockButton;
    [SerializeField] private GameObject clock;
    [SerializeField] private Selectors selectors;
    private TimelineClockLogic timelineClockLogic;
    private SpriteRenderer spriteRenderer;
    private Color clockColor;
    private bool greyedOut;
    
    private void Awake()
    {
        timelineClockLogic = clock.GetComponent<TimelineClockLogic>();
        this.spriteRenderer = clock.GetComponent<SpriteRenderer>();
        clockColor = spriteRenderer.color;
    }

    private void Start()
    {
        clockButton = new ClockButtonLogic(timelineClockLogic);
    }

    public override void activateButton()
    {
        if (!greyedOut)
        {
            clockButton.activateButton();
        }
    }

    public void updateIfClockIsActive()
    {
        if (timelineClockLogic.getCurrentTurn() % 2 == 0 &&
            (selectors.getActiveSelector().getNation().name == "GreatBritain"))
        {
            resetClockColour();
        }
        else if (timelineClockLogic.getCurrentTurn() % 2 == 1 &&
            (selectors.getActiveSelector().getNation().name == "France"))
        {
            resetClockColour();
        }
        else
        {
            greyOutClock();
        }
    }

    private void greyOutClock()
    {
        spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        greyedOut = true;
    }

    private void resetClockColour()
    {
        spriteRenderer.color = clockColor;
        greyedOut = false;
    }
}
