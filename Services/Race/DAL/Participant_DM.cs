namespace UniANPR.Services.Race
{
    public partial class RaceService
    {
        private class Participant_DM
        {
            public int ParticipantId { get; set; }
            public int ParticipantName { get; set; }
            public int RaceId { get; set; }
            public bool Approved { get; set; }
            public int ParticipantFinished { get; set; }
        }
    }
}
