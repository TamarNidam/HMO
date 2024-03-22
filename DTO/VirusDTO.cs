using System.ComponentModel.DataAnnotations;

namespace HMO.DTO
{
    public class VirusDTO
    {
        public int VirusId { get; set; }

        public int? MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;

        [RegularExpression(@"^\d{9}$", ErrorMessage = "Identity Card must be 9 digits")]
        public string MemberIdentityCard { get; set; }

        public DateOnly? DatePositiveResult { get; set; }

        public DateOnly? DateRecovery { get; set; }
    }
}
