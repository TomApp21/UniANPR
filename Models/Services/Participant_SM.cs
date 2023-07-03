namespace UniANPR.Services.Race
{
    public class Participant_SM
    {
        public string ParticipantId { get; set; }
        public string ParticipantName { get; set; }
        public int RaceId { get; set; }
        public bool? Approved { get; set; }
        public int ParticipantFinished { get; set; }
        public string Numberplate { get; set; }
    }
}
