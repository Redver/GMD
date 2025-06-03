namespace Resources.Features.TimeTravel.Clock.SavedData
{
    public class NationData
    {
        public NationData(string nationName, float treasurey)
        {
            this.nationName = nationName;
            this.treasurey = treasurey;
        }

        public string NationName
        {
            get => nationName;
            set => nationName = value;
        }

        public float Treasurey
        {
            get => treasurey;
            set => treasurey = value;
        }

        private string nationName;
        private float treasurey;
    }
}