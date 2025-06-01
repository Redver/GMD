using UnityEngine;
using Resources.map_assets.Selector_Scripts.SelectorMVP;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    [SerializeField] private GameObject NationObject;
    [SerializeField] private GameObject SelectorObject;
    [SerializeField] private GameObject SelectorsObject;

    public UnityEvent onEndTurnUnityEvent; 

    private SelectorView selector;
    private Nation nation;
    private Selectors selectors;

    void Start()
    {
        GetComponent<PlayerInput>().actions.Enable();
        selectors = SelectorsObject.GetComponent<Selectors>();
        nation = NationObject.GetComponent<Nation>();
        selector = SelectorObject.GetComponent<SelectorView>();
    }

    private void Awake()
    {
    }

    void Update()
    {
        
    }

    public void victoryCheck()
    {
        if (nation.victoryCheck())
        {
            nation.showVictoryScreen();
        }
    }

    public void OnEndTurn(InputAction.CallbackContext context)
    {
        if (context.canceled && canEndTurn())
        {
            if (selector.tryEndTurn())
            {
                victoryCheck();
                onEndTurnUnityEvent.Invoke();
            }
        }
    }

    public bool canEndTurn()
    {
        return selector.canEndTurn();
    }

    public void onStartTurn()
    {
        nation.onStartTurn();
        selector.onStartTurn(NationObject);
    }
}
