using System.ComponentModel.DataAnnotations;
using UniANPR.Enum;
using UniANPR.Services.Race;

namespace UniANPR.Models.Services
{
    public class Lap_SM
    {
        public int RaceId { get; set; }
        public string ParticipantId { get; set; }
        public int LapNumber { get; set; }
        public DateTime? TimeCrossed { get; set; }
        public TimeSpan CumulativeTime { get; set; }


        
    }
}
