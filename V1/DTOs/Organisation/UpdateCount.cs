namespace CoviIDApiCore.V1.DTOs.Organisation
{
    public class UpdateCountRequest
    {
        public string CoviId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }

    public class UpdateCountResponse
    {
        public int Balance { get; set; }
    }
}