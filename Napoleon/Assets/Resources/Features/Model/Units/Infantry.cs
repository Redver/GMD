using Resources.map_assets.Selector_Scripts.SelectorMVP;
using UnityEngine;

namespace Resources.Features.Model.Units
{
    public class Infantry : IUnit
    {
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
    }
    //notes for next time: set up prefabs, set up the object pool, implement these methods.
}