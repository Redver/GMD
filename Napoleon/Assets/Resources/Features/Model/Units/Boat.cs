using System.Collections.Generic;
using Resources.map_assets.Selector_Scripts.SelectorMVP;

namespace Resources.Features.Model.Units
{
    public class Boat : Unit
    {
        public override bool canDropUnitHere(Province newProvince)
        {
            int spacesToMove = this.province.findDistanceBetween(newProvince);
            if ( spacesToMove <= moves)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void resetMoves()
        {
            this.moves = 5;
            this.view.resetUnitColour();
        }

        public override void decreaseMoves()
        {
            int spacesOfMoves = this.province.findDistanceBetween(this.getSelector().GetComponent<SelectorView>().getSelectedProvinceObject().GetComponent<Province>());
            moves = moves - spacesOfMoves;
        }

        public override void onEndTurn()
        {
            resetMoves();
        }
    }
}