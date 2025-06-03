namespace Resources.Features.TimeTravel.Clock.SavedData
{
    public class ProvinceData
    {
        public ProvinceData(string provinceName, Nation owner)
        {
            this.provinceName = provinceName;
            this.owner = owner;
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
    }
}