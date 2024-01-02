using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebFrontToBack.Areas.Admin.ViewModels
{
    public class CreateTeamMemberVM
    {
        public int Id { get; set; }
        [Required, MinLength(6)]
        public string Fullname { get; set; }
        [Required]
        public string Profession { get; set; }
        [Required, NotMapped]  //NotMapped migration olan zaman bazaya tetbiq edilmesin menasini verir
        public IFormFile Photo { get; set; } //IFormFile c# da fayla qarsiliq gelen tipdir.
    }
}
