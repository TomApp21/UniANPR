using System.ComponentModel.DataAnnotations;
using UniANPR.Enum;
using UniANPR.Services.Race;

namespace DemoANPR.Models.Services
{
    public class Race_SM
    {
        public int RaceId { get; set; }

        public string RaceName { get; set;}

        public int RaceTrackId { get; set; }
        public int Spots { get; set; }

        public DateTime StartTime { get; set; } = DateTime.Now.AddHours(1);
        public DateTime EndTime { get; set; } = DateTime.Now.AddHours(2);
        public RaceStatus RaceStatus { get; set; }
        public int RequiredLaps { get; set; }

        public int ActiveParticipants { get; set; }

        public bool ParticipantsAwaiting { get; set; }

        public List<Participant_SM> Participants { get; set; }
    }
}
