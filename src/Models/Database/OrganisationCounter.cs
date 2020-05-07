using System;

namespace CoviIDApiCore.Models.Database
{
    public class OrganisationCounter : BaseModel<Guid>
    {
        public virtual Organisation Organisation { get; set; }
        public DateTime Date { get; set; }
        public int Movement { get; set; }
        public int Balance { get; set; }
        public string DeviceIdentifier { get; set; }

        public OrganisationCounter()
        {
        }
    }
}