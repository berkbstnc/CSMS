using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CSMS.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required]
        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Address")]
        public string Address { get; set; }
        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}