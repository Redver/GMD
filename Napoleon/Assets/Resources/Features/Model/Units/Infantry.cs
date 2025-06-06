﻿using UnityEngine;

namespace Resources.Features.Model.Units
{
    public class Infantry : Unit
    {
        public override bool canDropUnitHere(Province newProvince)
        {
            bool isNeighbourProvince = this.getCurrentProvince().GetComponent<Province>().getNeighbours().Contains(newProvince);
            bool hasMovement = this.moves > 0;
            bool isLand = newProvince.CompareTag(nameof(Province));
            bool hasBoat = newProvince.GetComponent<Province>().hasFriendlyBoat(this);
            
            bool isLandOrBoated = isLand || hasBoat;
            
            return isNeighbourProvince && hasMovement && isLandOrBoated;
        }

        public override void resetMoves()
        {
            this.moves = 1;
            this.view.resetUnitColour();
        }

        public override void decreaseMoves()
        {
            this.moves--;
            this.view.greyOutUnit();
        }
        
        public override void onEndTurn()
        {
            resetMoves();
            if (!isInCombat() && this.getCurrentProvince().GetComponent<Province>().getOwner() != this.nation && this.getCurrentProvince().CompareTag(nameof(Province)))
            {
                this.getCurrentProvince().GetComponent<Province>().onChangedOwner(this.nation);
            }
            if (checkIfShouldBeDestroyed())
            {
                destroy();
            }
        }
        
        public override bool IsBoat()
        {
            return false;
        }

        public override bool checkIfShouldBeDestroyed()
        {
            if (this.province.gameObject.CompareTag("SeaTile") && !(this.province.hasFriendlyBoat(this)))
            {
                return true;
            }
            return false;
        }
    }
}