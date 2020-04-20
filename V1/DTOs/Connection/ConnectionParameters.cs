namespace CoviIDApiCore.V1.DTOs.Connection
{
    public class ConnectionParameters
    {
        public string ConnectionId { get; set; }
        public bool Multiparty { get; set; }
        /// <summary>
        /// The Agent/Tenant Name
        /// </summary>
        public string Name { get; set; }
    }
}
