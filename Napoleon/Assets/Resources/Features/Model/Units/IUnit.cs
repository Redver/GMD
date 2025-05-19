using UnityEngine;

namespace Resources.Features.Model.Units
{
    public interface IUnit
    {
        public GameObject getSelector();
        public void setCurrentProvince(Province province);
        public GameObject getCurrentProvince();

        public void raiseSelectedUnit();
        public void lowerSelectedUnit();
        
        public void setView(UnitView View);
        public void dropSelectedUnit(Province newProvince);
        public UnitView getView();
        public bool canDropUnitHere(Province newProvince);
        public void setNation(Nation builderNation);
        public Nation getNation();
        
        public void firstContact();
        public void beginCombat();
        public void endCombat();

        public int getMoves();
        public void decreaseMoves();
        public void resetMoves();
        public void onEndTurn();
        
        public bool isInCombat();
        public void setInCombat();
        public void setNotInCombat();

        public bool canSelect();

        public bool IsBoat();
        
        public bool checkIfShouldBeDestroyed();
        public void destroy();
    }
}