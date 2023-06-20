using UniANPR.Enum;

namespace DemoANPR.Models.Components
{
    public class Race_VM
    {
        public int RaceId { get; set; }
        public int RaceTrackId { get; set; }
        public int Spots { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public RaceStatus RaceStatus { get; set; }
        public int RequiredLaps { get; set; }
    }
}
