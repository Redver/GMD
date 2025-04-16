using Resources.map_assets.Selector_Scripts.SelectorMVP;
using UnityEngine;

namespace Resources.Features.Model.Units
{
    public abstract class Unit : IUnit
    {
        protected UnitView view;
        public GameObject getSelector()
        {
            throw new System.NotImplementedException();
        }

        public void selectUnit(SelectorModel Selector)
        {
            throw new System.NotImplementedException();
        }

        public void setCurrentProvince(Province province)
        {
            throw new System.NotImplementedException();
        }

        public GameObject getCurrentProvince()
        {
            throw new System.NotImplementedException();
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
            view.dropUnit(newProvince);
        }

        public void setView(UnitView View)
        {
            this.view = View;
        }
    }
}