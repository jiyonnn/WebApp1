using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace App1.Validation
{
    public class DecimalPrecisionScaleAttribute : ValidationAttribute
    {
        private readonly int _precision;
        private readonly int _scale;

        public DecimalPrecisionScaleAttribute(int precision, int scale)
        {
            _precision = precision;
            _scale = scale;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return ValidationResult.Success;
            }

            if (value is not decimal decimalValue)
            {
                return new ValidationResult(ErrorMessage);
            }

            var valueText = decimalValue.ToString(CultureInfo.InvariantCulture);
            var parts = valueText.Split('.');
            var wholeDigits = parts[0].TrimStart('-').Length;
            var decimalDigits = parts.Length > 1 ? parts[1].Length : 0;

            if (wholeDigits + decimalDigits > _precision || decimalDigits > _scale)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
