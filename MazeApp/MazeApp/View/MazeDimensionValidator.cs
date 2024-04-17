using MazeApp.Model;
using System.Globalization;
using System.Windows.Controls;

namespace MazeApp.View
{
    public class MazeDimensionValidator : ValidationRule
    {

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int dimension = 0;
            if (!int.TryParse((string)value, out dimension))
            {
                return new ValidationResult(false, "Dimension must be an integer.");
            }
            if (dimension < Settings.MIN_DIM || dimension > Settings.MAX_DIM)
            {
                return new ValidationResult(false, "Dimension must be between " + Settings.MIN_DIM + " and " + Settings.MAX_DIM + ".");
            }
            return ValidationResult.ValidResult;
        }
    }
}
