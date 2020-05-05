﻿namespace CoviIDApiCore.V1.DTOs.Organisation
{
    public class UpdateCountRequest
    {
        public string CoviId { get; set; }
        public string OrganisationId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }
}