namespace UniANPR.Models.Components
{
    public class Lap_VM
    {
        public int RaceId { get; set; }
        public int LapNumber { get; set; }
        public DateTime? TimeCrossed { get; set; }

        public int TeamId { get; set; }
    }
}
