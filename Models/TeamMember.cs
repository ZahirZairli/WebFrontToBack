using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFrontToBack.Models
{
    public class TeamMember
    {
        public int Id { get; set; }
        [Required,MinLength(6)]
        public string Fullname { get; set; }
        [Required]
        public string Profession { get; set; }
        [Required]
        public string Imagepath { get; set; }
    }
}
