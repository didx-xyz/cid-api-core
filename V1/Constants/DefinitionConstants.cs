using System.Collections.Generic;

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
            { Schemas.PersonalDetials, "RYJoQ8UNadGrrfL7PBK8Wm:3:CL:94331:CoviID" },
            { Schemas.Identification, "RYJoQ8UNadGrrfL7PBK8Wm:3:CL:94335:CoviID" },
            { Schemas.CovidTest, "RYJoQ8UNadGrrfL7PBK8Wm:3:CL:94366:CoviID" }

        };

        public enum Schemas
        {
            PersonalDetials,
            Identification,
            CovidTest
        }
    }
}
