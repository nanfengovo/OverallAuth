using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace OverallAuthWPF.Additional_properties
{
    public static class TextBoxHelper
    {
        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.RegisterAttached(
                "PlaceholderText",
                typeof(string),
                typeof(TextBoxHelper),
                new PropertyMetadata(""));

        public static string GetPlaceholderText(TextBox obj) => (string)obj.GetValue(PlaceholderTextProperty);
        public static void SetPlaceholderText(TextBox obj, string value) => obj.SetValue(PlaceholderTextProperty, value);
    }
}
