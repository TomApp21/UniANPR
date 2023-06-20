namespace UniANPR.Services.Race.DAL
{
    public partial class RaceService
    {
        private class Race_DM
        {
            public int RaceId { get; set; }
            public int RaceTrackId { get; set; }
            public int Spots { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public int RaceStatus { get; set; }
            public int RequiredLaps { get; set; }
        }
    }
}
