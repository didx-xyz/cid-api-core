using System.Collections.Generic;

namespace CoviIDApiCore.V1.Constants
{
    public class UrlConstants
    {
        public static Dictionary<Routes, string> PartialRoutes = new Dictionary<Routes, string>
        {
            { Routes.Agency, "agency/v1/" },
            { Routes.Custodian, "custodian/v1/api/" }
        };

        public enum Routes
        {
            Agency,
            Custodian
        }
    }
}
