using System.ComponentModel.DataAnnotations;
using UniANPR.Enum;

namespace DemoANPR.Models.Components
{
    public class Race_VM
    {
        public int RaceId { get; set; }

        [Display(Name = "Race Name:")]
        [Required(ErrorMessage = "You must specify a race name.")]
        public string RaceName { get; set;}

        public int RaceTrackId { get; set; }
        public int Spots { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now.AddHours(1);
        public DateTime EndTime { get; set; } = DateTime.Now.AddHours(2);
        public RaceStatus RaceStatus { get; set; }

        [Display(Name = "Required Laps:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int RequiredLaps { get; set; }
    }
}
