using UnityEngine;

namespace Resources.Features.Model.Units
{
    public class UnitView : MonoBehaviour
    {
        private IUnit unitLogic;
        private Vector3 defaultPosition;
        private Vector3 raisedPosition;

        public void Init(IUnit unitType, Province province)
        {
            unitLogic = unitType;
            unitLogic.setCurrentProvince(province);
            unitLogic.setView(this);
            updatePositions(province);
            this.transform.position = defaultPosition;
        }

        public void selectUnit()
        {
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
    }
}