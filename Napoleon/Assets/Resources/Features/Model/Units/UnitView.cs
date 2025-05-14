using UnityEngine;

namespace Resources.Features.Model.Units
{
    public class UnitView : MonoBehaviour
    {
        private IUnit unitLogic;
        private Vector3 defaultPosition;
        private Vector3 raisedPosition;
        private Color defaultColor;
        private SpriteRenderer spriteRenderer;

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
        }

        public void selectUnit()
        {
            Debug.Log($"unitview select unit {unitLogic.getNation().name}");
            transform.SetParent(unitLogic.getSelector().transform);
            transform.position = raisedPosition;
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
            raisedPosition = new Vector3(defaultPosition.x, defaultPosition.y + 0.1f, defaultPosition.z);
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
    }
}