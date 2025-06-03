using Resources.map_assets.Selector_Scripts.SelectorMVP;
using UnityEngine;

namespace Resources.Features.Model.Units
{
    public class UnitView : MonoBehaviour
    {
        private IUnit unitLogic;
        private Vector3 defaultPosition;
        private Color defaultColor;
        private SpriteRenderer spriteRenderer;
        private GameObject deathEffect;
        [SerializeField] private GameObject unitPrefab;

        public void Init(IUnit unitType, Province province)
        {
            unitLogic = unitType;
            unitLogic.setCurrentProvince(province);
            unitLogic.setView(this);
            updatePositions(province);
            Nation currentNation = province.getOwner();
            this.transform.position = defaultPosition;
            this.unitLogic.setNation(currentNation);
            currentNation.endTurnEvent.AddListener(endTurnEvent);
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                defaultColor = spriteRenderer.color;
            }

            if (currentNation.name == "GreatBritain")
            {
                deathEffect = UnityEngine.Resources.Load<GameObject>("Features/ParticleExplosion/Sprites/TeaParticle");
            }
            else
            {
                deathEffect = UnityEngine.Resources.Load<GameObject>("Features/ParticleExplosion/Sprites/BaguetteParticle");
            }
            currentNation.refreshBoatUi();
            currentNation.refreshSoldierUI();
        }

        public void selectUnit()
        {
            unitLogic.getSelector().GetComponent<SelectorView>().spreadUnits();
        }

        public IUnit getUnitLogic()
        {
            return unitLogic;
        }

        public void dropUnit(Province newProvince)
        {
            unitLogic.setCurrentProvince(newProvince);
            transform.SetParent(newProvince.transform);
            transform.localPosition = new Vector3(0.1f, 0.1f, 0.1f);
            updatePositions(newProvince);
        }

        public void updatePositions(Province province)
        {
            defaultPosition = province.transform.position;
        }

        public void deselectUnit()
        {
            this.transform.SetParent(unitLogic.getCurrentProvince().transform);
            this.transform.position = defaultPosition;
        }

        public void endTurnEvent()
        {
            unitLogic.onEndTurn();
        }

        public void greyOutUnit()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
        }

        public void resetUnitColour()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = defaultColor;
            }
        }

        public void destroy()
        {
            GameObject particles = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(particles, 1f);
            Destroy(gameObject);
            //gameObject.SetActive(false);
        }
    }
}