namespace UniANPR.Models.Components
{
    public class Lap_VM
    {
        public int RaceId { get; set; }
        public int LapNumber { get; set; }
        public DateTime? TimeCrossed { get; set; }
        public string ParticipantId { get; set; }

        public TimeSpan LastLapTime { get; set; }

        public int Position { get; set; }

        public TimeSpan FastestLap { get; set; }




        public string ParticipantName { get; set; }

    }
}
