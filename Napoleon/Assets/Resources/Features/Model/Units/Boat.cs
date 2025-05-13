using System.Collections.Generic;

namespace Resources.Features.Model.Units
{
    public class Boat : Unit
    {
        
        public override bool canDropUnitHere(Province newProvince)
        {
            bool canMove = false;
            int spacesToMove;
            List<Province> neighboursToUnit = this.getCurrentProvince().GetComponent<Province>().getNeighbours();
            foreach (Province neighbour in neighboursToUnit)
            {
                if (neighbour == newProvince)
                {
                    canMove = true;
                }
                else
                {
                    //recursive check every neigbour until you find the path, and keep count of the depth
                    //infantry shouldnt be too hard to do either
                }
            }

            return true;
        }

        public override void resetMoves()
        {
            this.moves = 5;
        }

        public override void onEndTurn()
        {
            resetMoves();
        }
    }
}