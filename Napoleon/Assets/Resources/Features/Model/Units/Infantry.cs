using Resources.map_assets.Selector_Scripts.SelectorMVP;
using UnityEngine;

namespace Resources.Features.Model.Units
{
    public class Infantry : Unit
    {
        public override bool canDropUnitHere(Province newProvince)
        {
            bool isNeighbourProvince = this.getCurrentProvince().GetComponent<Province>().getNeighbours().Contains(newProvince);
            bool hasMovement = this.moves > 0;

            return isNeighbourProvince && hasMovement;
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
            if (!isInCombat() && this.getCurrentProvince().GetComponent<Province>().getOwner() != this.nation)
            {
                this.getCurrentProvince().GetComponent<Province>().onChangedOwner(this.nation);
            }
        }
    }
}