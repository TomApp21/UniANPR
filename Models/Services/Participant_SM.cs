﻿namespace UniANPR.Services.Race
{
    public class Participant_SM
    {
        public int ParticipantId { get; set; }
        public int ParticipantName { get; set; }
        public int RaceId { get; set; }
        public bool Approved { get; set; }
        public int ParticipantFinished { get; set; }
    }
}
