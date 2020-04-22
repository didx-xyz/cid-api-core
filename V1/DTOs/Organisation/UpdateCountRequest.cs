namespace CoviIDApiCore.V1.DTOs.Organisation
{
    public class UpdateCountRequest
    {
        public int Movement { get; set; }
        public string DeviceIdentifier { get; set; }

        public bool isValid()
        {
            return !(Movement > 1 || Movement < -1);
        }
    }
}