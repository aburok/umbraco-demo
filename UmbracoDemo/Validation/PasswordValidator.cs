using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace UmbracoTest.Validation
{
    public static class PasswordValidator
    {
        private static Regex UpperOrLowerCase = new Regex(@"^(?=.*[a-z])(?=.*[A-Z]).+$");
        private static Regex DigitsOrSpecialCharacters = new Regex(@"[\:\\=!@#$%^&\*\(\)_\+\|~\-\`\{\}\"";<>\?\,\./]+|[0-9]+");

        public static PasswordMessage Validate(string password, string userName, string email)
        {
            if (password.Contains(userName))
            {
                return PasswordMessage.ContainsUserName;
            }
            if (password.Contains(email))
            {
                return PasswordMessage.ContainsEmail;
            }
            if (password.Length < 8)
            {
                return PasswordMessage.TooShort;
            }
            if (UpperOrLowerCase.IsMatch(password) == false)
            {
                return PasswordMessage.NoUpperAndLowerAlphaCharacters;
            }
            if (DigitsOrSpecialCharacters.IsMatch(password) == false)
            {
                return PasswordMessage.NoDigitsOrSpecialCharacters;
            }

            return PasswordMessage.Valid;
        }
    }

    public enum PasswordMessage
    {
        ContainsUserName,
        ContainsEmail,
        TooShort,
        NoUpperAndLowerAlphaCharacters,
        NoDigitsOrSpecialCharacters,
        Valid
    }

}