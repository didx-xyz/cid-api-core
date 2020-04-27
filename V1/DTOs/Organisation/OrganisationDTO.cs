using CoviIDApiCore.Models.Database;

namespace CoviIDApiCore.V1.DTOs.Organisation
{
    public class OrganisationDTO
    {
        public string Id { get; set; }
        public int? Balance { get; set; }
        public int Total { get; set; }

        public OrganisationDTO()
        {

        }

        public OrganisationDTO(Models.Database.Organisation organisation, OrganisationCounter orgCounter, int total)
        {
            Id = organisation.Id.ToString();
            Balance = orgCounter?.Balance ?? 0;
            Total = total;
        }
    }
}