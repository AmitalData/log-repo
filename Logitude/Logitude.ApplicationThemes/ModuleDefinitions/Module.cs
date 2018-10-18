using Microsoft.Practices.Composite.Modularity;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Logitude.ApplicationThemes.ModuleDefinitions
{
    public class Module : IModule
    {
        public Module()
        {
            //ResourceDictionary applicationThemes = new ResourceDictionary();
            //applicationThemes.Source = new Uri("/Logitude.ApplicationThemes;component/Themes/ApplicationThemes.xaml", UriKind.Relative);
            //Application.Current.Resources.MergedDictionaries.Add(applicationThemes);

            //ResourceDictionary applicationResourceDictionary = new ResourceDictionary();
            //applicationResourceDictionary.Source = new Uri("/Logitude.ApplicationThemes;component/ApplicationResourceDictionary.xaml", UriKind.Relative);
            //Application.Current.Resources.MergedDictionaries.Add(applicationResourceDictionary);

            //StiOptions.Silverlight.Themes.CurrentTheme = StiSilverlightThemes.Office2010Silver;
            //StiOptions.Silverlight.Themes.UseDefaultTheme = false;
        }

        public void Initialize()
        {
            
        }
    }
}
