using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HospitalProject.ViewModels
{
    public class RoleCreateViewModel
    {
        [Required(ErrorMessage = "Role ismi boş bırakılamaz")]
        [Display(Name = "Role ismi :")]
        public string? Name { get; set; }
    }
}
