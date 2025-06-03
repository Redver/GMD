using Resources.Features.MenuFeatures.Logic;
using TMPro;
using UnityEngine;

public class BackTimeArrowView : ButtonUi
{
    private IButton backButton;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject clock;
    private TimelineClockLogic timelineClockLogic;
    
    private void Awake()
    {
        timelineClockLogic = clock.GetComponent<TimelineClockLogic>();
    }

    private void Start()
    {
        backButton = new BackTimeArrowLogic(timelineClockLogic);
        updateText();
    }

    public override void activateButton()
    {
        backButton.activateButton();
        updateText();
    }

    public void updateText()
    {
        text.text = timelineClockLogic.getCurrentTurn().ToString();
    }
}
