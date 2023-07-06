namespace UniANPR.Services.Race
{
    public partial class RaceService
    {
        private class Lap_DM
        {
            public int Id { get; set; }
            public int RaceId { get; set; }
            public string UserId { get; set; }
            public int LapNumber { get; set; }
            public DateTime? TimeCrossed { get; set; }

            public Int64 CumulativeTime { get; set; }
        }
    }
}
