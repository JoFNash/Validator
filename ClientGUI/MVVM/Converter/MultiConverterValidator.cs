using System;
using System.Globalization;
using ClientGUI.MVVM.Core.Converter;

namespace ClientGUI.MVVM.Converter
{
    public class MultiConverterValidator : MultiConverterBase <MultiConverterValidator>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }
    }
}