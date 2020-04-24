using System.Collections.Generic;

namespace CoviIDApiCore.V1.Constants
{
    public class UrlConstants
    {
        public static Dictionary<Routes, string> PartialRoutes = new Dictionary<Routes, string>
        {
            { Routes.Agency, "agency/v1/" },
            { Routes.Custodian, "custodian/v1/api/" },
            { Routes.Sendgrid, "v3/mail/send"},
            { Routes.ClickatellSend, "/messages"}
        };

        public enum Routes
        {
            Agency,
            Custodian,
            Sendgrid,
            ClickatellSend
        }
    }
}
