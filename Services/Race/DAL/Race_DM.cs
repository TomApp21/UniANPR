namespace UniANPR.Services.Race
{
    public partial class RaceService
    {
        private class Race_DM
        {
            public int Id { get; set; }
            public int RaceTrackId { get; set; }
            public int SpotLimit { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public int RaceStatusEnumValue { get; set; }
            public string RaceName { get; set; }
            public int RequiredLaps { get; set; }
        }
    }
}
