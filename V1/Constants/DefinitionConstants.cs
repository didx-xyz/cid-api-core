﻿using System.Collections.Generic;

namespace CoviIDApiCore.V1.Constants
{
    public class DefinitionConstants
    {
        /// <summary>
        /// Pass in the selected schema and retrieve the corresponding definition Id
        /// This is added here to save costs to Streetcred API
        /// </summary>
        public static Dictionary<Schemas, string> DefinitionIds = new Dictionary<Schemas, string>
        {
            { Schemas.Person, "RYJoQ8UNadGrrfL7PBK8Wm:3:CL:95145:Verified Person" },
            { Schemas.CovidTest, "RYJoQ8UNadGrrfL7PBK8Wm:3:CL:94574:Covid Test" }
        };

        public enum Schemas
        {
            Person,
            CovidTest
        }

        public class Attributes
        {
            // Covid Test Schema
            public static readonly string ReferenceNumber = "testReferenceNumber";
            public static readonly string Laboratory = "issuedLaboratory";
            public static readonly string DateTested = "dateTested";
            public static readonly string DateIssued = "dateIssued";
            public static readonly string CovidStatus = "covidStatus";
            // Verifier Person Schema
            public static readonly string FirstName = "firstName";
            public static readonly string LastName = "lastName";
            public static readonly string Photo = "photo";
            public static readonly string MobileNumber = "mobileNumber";
            public static readonly string IdentificationType = "identificationType";
            public static readonly string IdentificationValue = "identificationValue";
        }
    }
}
