﻿using System;
using System.Collections.Generic;

namespace CoviIDApiCore.Models.Database
{
    public class Organisation : BaseModel<Guid>
    {
        public string Name { get; set; }
        public string Payload { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual IList<OrganisationCounter> Counter { get; set; }

        public Organisation()
        {
        }
    }
}