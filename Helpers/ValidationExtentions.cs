using CoviIDApiCore.Exceptions;
using CoviIDApiCore.V1.Constants;
using CoviIDApiCore.V1.DTOs.Credentials;
using System;

namespace CoviIDApiCore.Helpers
{
    public static class ValidationExtentions
    {
        public static string ValidateLength(this string str)
        {
            if (str.Length >= 2 && str.Length <= 255)
            {
                return str;
            }
            throw new ValidationException(Messages.Val_Length);
        }

        public static long ValidateMobileNumber(this long num)
        {
            var lenght = num.ToString().Length;
            if (lenght > 9 && lenght < 16)
            {
                return num;
            }
            throw new ValidationException(Messages.Val_MobileNumber);
        }

        public static string ValidateIdentification(this string value, IdentificationTypes identificationType)
        {
            if (identificationType == IdentificationTypes.Passport)
            {
                if (value.Length > 6)
                {
                    return value;
                }
            }

            if (value.Length == 13)
            {
                return value;
            }

            throw new ValidationException(Messages.Val_Identification);
        }

        public static DateTime ValidateIsInPast(this DateTime date)
        {
            var test = new DateTime();
            if (date > DateTime.Today && date == test)
                throw new ValidationException(Messages.Val_DateInPast);

            return date;
        }

    }
}
