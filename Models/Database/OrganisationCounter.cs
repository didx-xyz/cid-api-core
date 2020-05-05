using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoviIDApiCore.Models.Database
{
    public class OrganisationCounter : BaseModel<Guid>
    {
        //TODO: Coviid
        public virtual Organisation Organisation { get; set; }
        public DateTime Date { get; set; }
        public int Balance { get; set; }
        [Column(TypeName = "decimal(12,8)")]
        public decimal Latitude { get; set; }
        [Column(TypeName = "decimal(12,8)")]
        public decimal Longitude { get; set; }
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