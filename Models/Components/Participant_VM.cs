using ThreeSC.NetStandardLib.StandardTools.Models;

namespace UniANPR.Models.Components
{

        public class Participant_VM
        {
            public String ParticipantId { get; set; }
            public string ParticipantName 
            {
                get;set;
            }
            public int RaceId { get; set; }
            public bool? Approved { get; set; }
            public int ParticipantFinished { get; set; }
        }
    
}
