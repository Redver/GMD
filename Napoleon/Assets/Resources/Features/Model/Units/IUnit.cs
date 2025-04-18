﻿using Resources.map_assets.Selector_Scripts.SelectorMVP;
using UnityEngine;

namespace Resources.Features.Model.Units
{
    public interface IUnit
    {
        public GameObject getSelector();
        public void selectUnit(SelectorModel Selector);
        public void setCurrentProvince(Province province);
        public GameObject getCurrentProvince();

        public void raiseSelectedUnit();
        public void lowerSelectedUnit();
        
        public void setView(UnitView View);
        public void dropSelectedUnit(Province newProvince);
        public UnitView getView();
    }
}