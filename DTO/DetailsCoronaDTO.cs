using HMO.Models;

namespace HMO.DTO
{
    public class DetailsCoronaDTO
    {
        public IndexCoronaDTO Details {  get; set; }
        public CoronaVirus? Virus { get; set; } 
        public List<VaccinationDTO> Vaccines { get; set; }
    }
}
