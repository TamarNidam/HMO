namespace HMO.DTO
{
    public class DetailsMemberDTO
    {
        public int MemberId { get; set; }

        public string FullName { get; set; }

        public string IdentityCard { get; set; }

        public string ACity { get; set; }

        public string AStreet { get; set; }

        public string ANumber { get; set; }

        public DateOnly DateBirth { get; set; }

        public string Telephone { get; set; }

        public string MobilePhone { get; set; }

    }
}
