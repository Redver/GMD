using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Resources.Features.Model.Units
{
    public abstract class Unit : IUnit
    {
        protected UnitView view;
        protected Province province;
        protected Nation nation;
        protected int moves = 0;
        protected bool inCombat;

        public Unit()
        {
            
        }

        public GameObject getSelector()
        {
            return province.getSelector();
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
            nation.refreshBoatUi();
            nation.refreshSoldierUI();
            view.dropUnit(newProvince);
            newProvince.addUnitToStack(this);
            checkForEnemyUnits(newProvince);
        }

        public void checkForEnemyUnits(Province droppedProvince)
        {
            var (friendlyStack, enemyStack) = droppedProvince.SplitUnitStackByNation();
            if (friendlyStack.Count > 0 && enemyStack.Count > 0)
            {
                firstContact();
            } 
        }

        public void firstContact()
        {
            inCombat = true;
            this.province.firstCombat();
        }

        public void beginCombat()
        {
            inCombat = true;
        }

        public void endCombat()
        {
            inCombat = false;
        }

        public void setView(UnitView View)
        {
            this.view = View;
        }

        public UnitView getView()
        {
            return view;
        }

        public virtual bool canDropUnitHere(Province newProvince)
        {
            throw new Exception("Unit should never be initialised as this base class");
        }

        public void setNation(Nation builderNation)
        {
            this.nation = builderNation;
        }

        public Nation getNation()
        {
            return this.nation;
        }

        public int getMoves()
        {
            return moves;   
        }

        public virtual void decreaseMoves()
        {
            throw new Exception("Unit should never be initialised as this base class");
        }

        public virtual void resetMoves()
        {
            throw new Exception("Unit should never be initialised as this base class");
        }

        public virtual void onEndTurn()
        {
            throw new Exception("Unit should never be initialised as this base class");
        }
        
        public abstract bool IsBoat();
        public abstract bool checkIfShouldBeDestroyed();

        public void destroy()
        {
            province.removeThisUnitFromStack(this);
            this.view.destroy();
        }

        public bool canSelect()
        {
            return !isInCombat() && moves > 0;
        }

        public bool isInCombat()
        {
            return inCombat;
        }

        public void setInCombat()
        {
            this.inCombat = true;
        }

        public void setNotInCombat()
        {
            this.inCombat = false;
        }
        
    }
}