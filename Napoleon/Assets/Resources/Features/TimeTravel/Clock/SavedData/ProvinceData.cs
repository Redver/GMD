namespace Resources.Features.TimeTravel.Clock.SavedData
{
    public class ProvinceData
    {
        public ProvinceData(string provinceName, Nation owner, bool isInCombat)
        {
            this.provinceName = provinceName;
            this.owner = owner;
            this.isInCombat = isInCombat;
        }

        public string ProvinceName
        {
            get => provinceName;
            set => provinceName = value;
        }

        public Nation Owner
        {
            get => owner;
            set => owner = value;
        }

        private string provinceName;
        private Nation owner;
        private bool isInCombat;

        public bool IsInCombat
        {
            get => isInCombat;
            set => isInCombat = value;
        }
    }
}