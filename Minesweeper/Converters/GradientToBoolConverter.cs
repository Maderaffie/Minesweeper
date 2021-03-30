using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Minesweeper.Converters
{
    public class GradientToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (parameter as string) switch
            {
                "CanYouFeelTheLoveTonight" => CheckForSavedGradient("#4568DC", "#B06AB3"),
                "CoolBlues" => CheckForSavedGradient("#2193b0", "#6dd5ed"),
                "WitchingHour" => CheckForSavedGradient("#c31432", "#240b36"),
                _ => false,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? parameter : null;
        }

        public bool CheckForSavedGradient(string leftGradient, string rightGradient)
        {
            Color leftColor = (Color)ColorConverter.ConvertFromString(leftGradient);
            Color rightColor = (Color)ColorConverter.ConvertFromString(rightGradient);
            return (Properties.Settings.Default.LeftGradient == leftColor &&
                Properties.Settings.Default.RightGradient == rightColor);
        }
    }
}
