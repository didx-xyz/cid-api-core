using System;

namespace CoviIDApiCore.Models.Database
{
    public class OrganisationCounter : BaseModel<Guid>
    {
        public virtual Organisation Organisation { get; set; }
        public DateTime Date { get; set; }
        public int Movement { get; set; }
        public int Balance { get; set; }

        public string GeoLocation { get; set; }
        public ScanType ScanType { get; set; }

        public OrganisationCounter()
        {
        }
    }

    public enum ScanType
    {
        CheckIn,
        CheckOut,
        Denied
    }
}