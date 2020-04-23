using System.Collections.Generic;

namespace CoviIDApiCore.V1.Constants
{
    public class ParameterConstants
    {
        #region Strings
        public static readonly string CompanyName = "Company Name";
        public static readonly string EmailAdress = "Email Address";
        #endregion

        public static Dictionary<EmailTemplates, string> TemplateIds = new Dictionary<EmailTemplates, string>
        {
            { EmailTemplates.OrganisationWelcome, "d-5ceb0422ddfd4850b361255bcc30fde2" }
        };

        public static Dictionary<EmailTemplates, string> EmailSubjects = new Dictionary<EmailTemplates, string>
        {
            { EmailTemplates.OrganisationWelcome, "Welcome to the Covi-ID platform!" }
        };

        public enum EmailTemplates
        {
            OrganisationWelcome
        }
    }
}