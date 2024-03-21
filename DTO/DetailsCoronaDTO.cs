using HMO.Models;

namespace HMO.DTO
{
    public class DetailsCoronaDTO
    {
        public IndexCoronaDTO Details {  get; set; }
        public List<CoronaVaccine> Vaccines { get; set; }
    }
}
