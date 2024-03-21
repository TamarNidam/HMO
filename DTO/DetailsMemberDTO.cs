using System.ComponentModel.DataAnnotations;

namespace HMO.DTO
{
    public class DetailsMemberDTO
    {
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        [RegularExpression(@"^\d{9}$", ErrorMessage = "Identity Card must be 9 digits")]
        public string IdentityCard { get; set; }
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "City must contain only letters and spaces")]
        [Display(Name = "City")]
        public string ACity { get; set; }
        [Required(ErrorMessage = "Street name is required")]
        [Display(Name = "Street")]
        public string AStreet { get; set; }

        [Required(ErrorMessage = "Namber house is required")]
        [Display(Name = "Number")]
        public string ANumber { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateOnly DateBirth { get; set; }

        [RegularExpression(@"^\d{0,}|^\d{7,}$", ErrorMessage = "Telephone number must be at least 7 digits")]
        public string Telephone { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile Phone must be 10 digits")]
        public string MobilePhone { get; set; }

    }
}
