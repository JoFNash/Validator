using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ClientGUI.MVVM.Core.Converter;

namespace ClientGUI.MVVM.Converter
{
    public class MultiConverterValidator : MultiConverterBase <MultiConverterValidator>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine((string)values[0]);
            Console.WriteLine((string)values[1]);
            Console.WriteLine(values[2].ToString());
            var list = new List<string>();
            for (int i = 0; i < values.Length; i++)
                list.Append((string)values[i]);
            return (object)list;
        }
    }
}