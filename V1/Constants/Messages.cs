namespace CoviIDApiCore.V1.Constants
{
    public class Messages
    {
        #region Misc
        public static readonly string Misc_Success = "Success.";
        public static readonly string Misc_SomethingWentWrong = "Oops! Something went wrong. Please try again later.";
        public static readonly string Misc_ThirdParty = "Third party error.";
        public static readonly string Misc_Unauthorized = $"Unauthorised.";
        #endregion

        #region DbErrors
        public static readonly string FailedToSave = "Failed to save entry(s) to database. See inner exception for details.";
        public static readonly string CreateNull = "Object is null whilst attempting to add to the database.";
        public static readonly string AddRangeNull = "Object list is null whilst attempting to add entries to the database.";
        public static readonly string DeleteNull = "Object is null whilst attempting to delete from the database.";
        public static readonly string GetNull = "Object is null whilst attempting to get from the database.";
        public static readonly string InvalidPageInfo = "Invalid page information. Page number and size must be greater than 0";
        #endregion

        #region Organisations
        public static readonly string Org_NotExists = $"Organisation does not exist.";
        public static readonly string Org_PayloadInvalid = $"Payload is invalid.";
        public static readonly string Org_NegBalance = $"Balance can not be less than 0.";

        #endregion
    }
}