using Resources.map_assets.Selector_Scripts.SelectorMVP;
using Unity.VisualScripting;
using UnityEngine;

namespace Resources.Features.Model.Units
{
    public abstract class Unit : IUnit
    {
        protected UnitView view;
        protected Province province;

        public Unit()
        {
            
        }

        public GameObject getSelector()
        {
            return province.getSelector();
        }

        public void selectUnit(SelectorModel Selector)
        {
            throw new System.NotImplementedException();
        }

        public void setCurrentProvince(Province province)
        {
            this.province = province;
        }

        public GameObject getCurrentProvince()
        {
            return province.GameObject();
        }

        public void raiseSelectedUnit()
        {
            view.selectUnit();
        }

        public void lowerSelectedUnit()
        {
           view.deselectUnit();
        }

        public void dropSelectedUnit(Province newProvince)
        {
            province.updateUnitCount();
            view.dropUnit(newProvince);
            newProvince.addUnitToStack(this);
        }

        public void setView(UnitView View)
        {
            this.view = View;
        }

        public UnitView getView()
        {
            return view;
        }
    }
}