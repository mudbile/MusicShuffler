using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Music_Shuffler {
    public static class SkinHandler {
        public static SolidColorBrush TextColor1 = new SolidColorBrush(Color.FromRgb(236, 178, 236));
        public static SolidColorBrush TextColor2 = new SolidColorBrush(Colors.LightBlue);
        public static SolidColorBrush TextBoxTextColor = new SolidColorBrush(Color.FromRgb(236, 178, 236));
        public static SolidColorBrush TextBoxBackground = new SolidColorBrush(Colors.Black);
        public static SolidColorBrush BorderColor = new SolidColorBrush(Color.FromArgb(255, 46, 198, 205));
        public static SolidColorBrush BorderBackground = new SolidColorBrush(Colors.Black);
        public static Thickness BorderThickness = new Thickness(1);
        public static Thickness ButtonBorderThickness = new Thickness(1);
        public static Thickness TextBorderThickness = new Thickness(1);
        public static SolidColorBrush WindowBackground = new SolidColorBrush(Colors.Black);
        public static SolidColorBrush ButtonTextColor = new SolidColorBrush(Colors.LightBlue);
        public static SolidColorBrush ButtonBackground = new SolidColorBrush(Colors.Black);
        public static FontFamily MainFont = (FontFamily)(new FontFamilyConverter()).ConvertFromString("Tekton Pro");
        public static FontFamily CheckBoxesAndTextBoxesFont = (FontFamily)(new FontFamilyConverter()).ConvertFromString("Tekton");
        public static FontFamily ButtonFont = (FontFamily)(new FontFamilyConverter()).ConvertFromString("Lucida Fax");


        //Takes two styles and returns a ne style that contains all the setters in the baseStyle, 
        //and the setters and triggers in the typsetyle
        public static Style CreateBase(Style baseStyle, Style typeStyle) {
            Style newStyle = new Style();
            foreach (SetterBase setter in baseStyle.Setters) {
                newStyle.Setters.Add(setter);
            }
            foreach (SetterBase setter in typeStyle.Setters) {
                newStyle.Setters.Add(setter);
            }
            foreach (Trigger trigger in typeStyle.Triggers) {
                newStyle.Triggers.Add(trigger);
            }
            return newStyle;
        }


        //Call this when all the properties have been retrieved and are valid.
        //Changes the runtime style (doesn't save anything to disc).
        public static void UpdateStyles() {
            //cache the base text style
            Style textStyle = (Style)Application.Current.Resources["TextStyle"];

            //TextStyle
            Style newTextStyle = new Style();
            foreach (SetterBase setter in textStyle.Setters) {
                newTextStyle.Setters.Add(setter);
            }
            newTextStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, TextColor1));
            newTextStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.Transparent));
            newTextStyle.Setters.Add(new Setter(TextBlock.BackgroundProperty, Brushes.Transparent));
            newTextStyle.Setters.Add(new Setter(TextBlock.FontFamilyProperty, MainFont));
            newTextStyle.Setters.Add(new Setter(TextBlock.FontSizeProperty, Properties.Settings.Default.H2));
            Application.Current.Resources["TextStyle"] = newTextStyle;

            //TextStyle02
            Style newTextStyle02 = CreateBase(textStyle, (Style)Application.Current.Resources["TextStyle02"]);
            newTextStyle02.Setters.Add(new Setter(TextBlock.ForegroundProperty, TextColor2));
            newTextStyle02.Setters.Add(new Setter(TextBlock.FontFamilyProperty, CheckBoxesAndTextBoxesFont));
            newTextStyle02.Setters.Add(new Setter(TextBlock.BackgroundProperty, Brushes.Transparent));
            newTextStyle02.Setters.Add(new Setter(TextBlock.FontSizeProperty, Properties.Settings.Default.H3));
            Application.Current.Resources["TextStyle02"] = newTextStyle02;

            //Console
            Style consoleStyle = CreateBase(textStyle, (Style)Application.Current.Resources["ConsoleTextStyle"]); ;
            consoleStyle.Setters.Add(new Setter(TextBlock.FontSizeProperty, Properties.Settings.Default.ConsoleFontSize));
            Application.Current.Resources["ConsoleTextStyle"] = consoleStyle;

            //ButtonStyle
            Style newButtonStyle = CreateBase(textStyle, (Style)Application.Current.Resources["ButtonStyle"]);
            newButtonStyle.Setters.Add(new Setter(Control.ForegroundProperty, ButtonTextColor));
            newButtonStyle.Setters.Add(new Setter(Control.BackgroundProperty, ButtonBackground));
            newButtonStyle.Setters.Add(new Setter(Control.FontSizeProperty, Properties.Settings.Default.H2));
            newButtonStyle.Setters.Add(new Setter(Control.FontFamilyProperty, ButtonFont));
            newButtonStyle.Setters.Add(new Setter(Control.BorderBrushProperty, BorderColor));
            newButtonStyle.Setters.Add(new Setter(Control.BorderThicknessProperty, ButtonBorderThickness));
            Application.Current.Resources["ButtonStyle"] = newButtonStyle;

            //Borders
            Style newBorderStyle = CreateBase(textStyle, (Style)Application.Current.Resources["BorderStyle"]);
            newBorderStyle.Setters.Add(new Setter(Border.BorderBrushProperty, BorderColor));
            newBorderStyle.Setters.Add(new Setter(Border.BackgroundProperty, BorderBackground));
            newBorderStyle.Setters.Add(new Setter(Border.BorderThicknessProperty, BorderThickness));
            Application.Current.Resources["BorderStyle"] = newBorderStyle;

            //Headers
            Style newHeaderStyle = CreateBase(textStyle, (Style)Application.Current.Resources["HeaderStyle"]);
            newHeaderStyle.Setters.Add(new Setter(Control.ForegroundProperty, TextColor1));
            newHeaderStyle.Setters.Add(new Setter(Control.FontSizeProperty, Properties.Settings.Default.H1));
            newHeaderStyle.Setters.Add(new Setter(TextBlock.FontFamilyProperty, MainFont));
            Application.Current.Resources["HeaderStyle"] = newHeaderStyle;

            //Checkboxes
            Style newCheckBoxStyle = CreateBase(textStyle, (Style)Application.Current.Resources["CheckBoxStyle"]);
            newCheckBoxStyle.Setters.Add(new Setter(Control.ForegroundProperty, TextColor2));
            newCheckBoxStyle.Setters.Add(new Setter(Control.FontSizeProperty, Properties.Settings.Default.H3));
            newCheckBoxStyle.Setters.Add(new Setter(Control.BackgroundProperty, BorderBackground));
            newCheckBoxStyle.Setters.Add(new Setter(TextBlock.FontFamilyProperty, CheckBoxesAndTextBoxesFont));

            Application.Current.Resources["CheckBoxStyle"] = newCheckBoxStyle;

         
            //Textboxes
            Style newTextBoxStyle = CreateBase(textStyle, (Style)Application.Current.Resources["TextBoxStyle"]);
            newTextBoxStyle.Setters.Add(new Setter(Control.BorderBrushProperty, BorderColor));
            newTextBoxStyle.Setters.Add(new Setter(Control.BackgroundProperty, TextBoxBackground));
            newTextBoxStyle.Setters.Add(new Setter(Control.ForegroundProperty, TextBoxTextColor));
            newTextBoxStyle.Setters.Add(new Setter(TextBlock.FontFamilyProperty, CheckBoxesAndTextBoxesFont));
            newTextBoxStyle.Setters.Add(new Setter(TextBlock.FontSizeProperty, Properties.Settings.Default.H3));
            newTextBoxStyle.Setters.Add(new Setter(Control.BorderThicknessProperty, TextBorderThickness));
            Application.Current.Resources["TextBoxStyle"] = newTextBoxStyle;

            //main window background
            ((MainWindow)Application.Current.MainWindow).Background = WindowBackground;
        }
    }
}
