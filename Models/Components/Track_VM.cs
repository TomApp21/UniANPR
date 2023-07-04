using System.ComponentModel.DataAnnotations;
using UniANPR.Enum;

namespace UniANPR.Models.Components
{
    public class Track_VM
    {
        public int TrackId { get; set; }

        [Display(Name = "Track Name:")]
        [Required(ErrorMessage = "You must specify a track name.")]
        public string TrackName { get; set;}
    }
}
