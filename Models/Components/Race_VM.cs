using System.ComponentModel.DataAnnotations;
using UniANPR.Enum;
using UniANPR.Models.Components;

namespace DemoANPR.Models.Components
{
    public class Race_VM
    {
        //public Race_VM()
        //{
        //    RaceParticipants = new List<Participant_VM>();
        //}
        public int RaceId { get; set; }

        [Display(Name = "Race Name:")]
        [Required(ErrorMessage = "You must specify a race name.")]
        public string RaceName { get; set;}

        [Display(Name = "Race Track:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "The field must not be empty.")]
        public int RaceTrackId { get; set; }


        [Display(Name = "Required Laps:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int Spots { get; set; }

        public DateTime StartTime { get; set; } = DateTime.Now.AddHours(1);
        public DateTime EndTime { get; set; } = DateTime.Now.AddHours(2);
        public RaceStatus RaceStatus { get; set; }

        [Display(Name = "Required Laps:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int RequiredLaps { get; set; }
    
        public bool ParticipantsAwaiting { get; set; }
    
        public string RaceTrackName { get; set; }
        public int RegisteredParticipants { get; set; }

        public bool RegistrationClosed
        {
            get
            {
                return (StartTime - DateTime.Now).TotalSeconds > 200;  // 86400
            }
            set
            {
                ;
            }
        }


        public List<Participant_VM> RaceParticipants { get; set; }

    }
}
