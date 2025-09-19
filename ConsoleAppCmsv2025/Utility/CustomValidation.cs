using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Utility
{
    public class CustomValidation
    {
        #region 1 - Username Validation
        // Username should not be empty and should contain only letters, numbers, underscore and dot
        public static bool IsValidUsername(string username)
        {
            return !string.IsNullOrWhiteSpace(username) &&
                   username.Length <= 10 &&
                   Regex.IsMatch(username, @"^[a-zA-Z0-9_.]+$");
        }
        #endregion

        #region 2 - Password Validation
        // Password should have at least 4 characters including at least one uppercase, lowercase, digit, and special character
        public static bool IsValidPassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password) &&
                   Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@\W_]).{4,}$");
        }
        #endregion

        #region 3 - Read Password with * symbol
        public static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            }
            while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }
        #endregion

        #region 4 - Positive Decimal Validation
        public static bool IsValidDecimal(decimal value)
        {
            return value >= 0;
        }
        #endregion

        #region 5 - Phone Number Validation
        // Validates Indian phone numbers (10 digits)
        public static bool IsValidPhone(string phone)
        {
            return !string.IsNullOrWhiteSpace(phone) &&
                   Regex.IsMatch(phone, @"^[6-9]\d{9}$");
        }
        #endregion

        #region 6 - MMR Number Validation
        // MMR number format: e.g., MMR2501
        public static bool IsValidMMRNumber(string mmr)
        {
            return !string.IsNullOrWhiteSpace(mmr) &&
                   Regex.IsMatch(mmr, @"^MMR\d{4}$");
        }
        #endregion

        #region 7 - Gender Validation
        public static bool IsValidGender(char gender)
        {
            return gender == 'M' || gender == 'F' || gender == 'O';
        }
        #endregion

        #region 8 - User Age Validation (>= 18)
        public static bool IsValidUserAge(DateTime dob)
        {
            int age = DateTime.Now.Year - dob.Year;

            // If birthday hasn't occurred yet this year, subtract 1
            if (DateTime.Now.DayOfYear < dob.DayOfYear)
                age--;

            return age >= 18;
        }
        #endregion

        #region 9 - Patient DOB Validation
        // Ensures DOB is not in the future
        public static bool IsValidPatientDOB(DateTime dob)
        {
            return dob <= DateTime.Now;
        }
        #endregion
    }
}
