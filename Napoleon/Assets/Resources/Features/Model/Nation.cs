using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Nation : MonoBehaviour
{
    private string nationName;
    private float treasurey;
    private int unitCount;
    private int boatCount;
    [SerializeField] private int provinceCount;
    [SerializeField] GameObject capitalProvince;
    [SerializeField] private GameObject MoneyText;
    [SerializeField] private int startingTreasury = 100;
    public UnityEvent endTurnEvent; 
    private TextMeshProUGUI moneyUi;


    private void Awake()
    {
        nationName = gameObject.name;
        if (endTurnEvent == null)
        {
            endTurnEvent = new UnityEvent();
        }
        moneyUi = MoneyText.GetComponent<TextMeshProUGUI>();
        treasurey = startingTreasury;
        refreshTreasurey();
    }

    void Start()
    {
    }

    void Update()
    {
        
    }

    public bool canBuild(int cost)
    {
        if (cost > treasurey)
        {
            return false;
        }
        return true;
    }

    public void refreshTreasurey()
    {
        moneyUi.text = treasurey.ToString();
    }

    public void updateTreasurey()
    {
        float income = provinceCount * 3;
        float expenses = unitCount + (boatCount * 3);
        float net = income - expenses;
        treasurey += net;
        refreshTreasurey();
    }

    public void payForUnit()
    {
        treasurey -= 5;
        refreshTreasurey();
    }
    
    public void payForBoat()
    {
        treasurey -= 15;
        refreshTreasurey();
    }


    public void onEndTurn()
    { 
        updateProvinceCount();
        updateTreasurey();
        endTurnEvent.Invoke();
    }

    public void updateProvinceCount()
    {
        this.provinceCount = transform.childCount;
    }

    public void getNewCapitalProvince()
    {
        capitalProvince = transform.GetChild(0).gameObject;
    }

    public GameObject getCurrentCapitalProvince()
    {
        return capitalProvince;
    }

    public void onStartTurn()
    {
        if (capitalProvince.GetComponent<Province>().getOwner().nationName != this.nationName)
        {
            getNewCapitalProvince();
        }
    }

    public string getName()
    {
        return nationName;
    }

}
