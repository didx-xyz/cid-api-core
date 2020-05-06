using System;

namespace CoviIDApiCore.V1.DTOs.Credentials
{
    public class CovidTestCredentialParameters
    {
        public DateTime DateTested { get; set; }
        internal DateTime DateIssued { get; set; }
        public ResultStatus CovidStatus { get; set; }
        public Laboratory Laboratory { get; set; }
        public string ReferenceNumber { get; set; }
        public bool HasConsent { get; set; }
    }
}
