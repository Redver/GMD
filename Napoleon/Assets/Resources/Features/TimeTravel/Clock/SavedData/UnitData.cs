﻿namespace Resources.Features.TimeTravel.Clock.SavedData
{
    public class UnitData
    {
        public UnitData(Province province, Nation nation, bool isBoat, int moves, bool inCombat)
        {
            this.province = province;
            this.nation = nation;
            this.isBoat = isBoat;
            this.moves = moves;
            this.inCombat = inCombat;
        }

        public int Moves
        {
            get => moves;
            set => moves = value;
        }

        public Nation Nation
        {
            get => nation;
            set => nation = value;
        }

        public Province Province
        {
            get => province;
            set => province = value;
        }

        public bool IsBoat
        {
            get => isBoat;
            set => isBoat = value;
        }

        private Province province;
        private Nation nation;
        private bool isBoat;
        private int moves;
        private bool inCombat;

        public bool InCombat
        {
            get => inCombat;
            set => inCombat = value;
        }
    }
}