using System.ComponentModel.DataAnnotations;

namespace HMO.DTO
{
    public class VaccinationDTO
    {
        public int VaccineId { get; set; }
        public int VaccinationCount { get; set; } = 0;
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;

        [RegularExpression(@"^\d{9}$", ErrorMessage = "Identity Card must be 9 digits")]
        public string MemberIdentityCard { get; set; }
        public DateOnly DateVaccine { get; set; }
        public string ManufacturerVaccine { get; set; }
    }
}
