using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public class TitleValidator : ValidationRule
    {
        public string EmptyError { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = (value ?? string.Empty).ToString();
            if (String.IsNullOrWhiteSpace(input) || String.IsNullOrEmpty(input))
                return new ValidationResult(false, Application.Current.Resources["StrEmpty"]);
            return new ValidationResult(true,null);
        }
    }

    public class ToValidator : ValidationRule
    {
        public string EmptyError { get; set; }
        public string IncorrectError { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = (value ?? string.Empty).ToString();
            if (String.IsNullOrWhiteSpace(input) || String.IsNullOrEmpty(input))
                return new ValidationResult(false, Application.Current.Resources["StrEmpty"]);

            Regex regex = new Regex("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+[.][a-zA-Z0-9-.]+$");
            if (!regex.IsMatch(input))
                return new ValidationResult(false, Application.Current.Resources["StrIncorrect"]);

            return new ValidationResult(true, null);
        }
    }

    public class BodyValidator : ValidationRule
    {
        public string EmptyError { get; set; }
        public string LengthError { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = (value ?? string.Empty).ToString();
            if (String.IsNullOrWhiteSpace(input) || String.IsNullOrEmpty(input))
                return new ValidationResult(false, Application.Current.Resources["StrEmpty"]);
            if (input.Length < 8)
                return new ValidationResult(false, Application.Current.Resources["StrTooShort"]);
            return new ValidationResult(true, null);
        }
    }
}
