using System;

namespace CoviIDApiCore.V1.DTOs.WalletTestResult
{
    public class TestResultRequest
    {
        public string Key { get; set; }
        public TestType TestType { get; set; }
        public LaboratoryStatus LaboratoryStatus { get; set; }
        public ResultStatus ResultStatus { get; set; }
        public Laboratory Laboratory { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime TestedAt { get; set; }
        public bool HasConsent { get; set; }
    }
}
