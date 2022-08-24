using Logitude.DashboardModule.MetaDataTool.Helpers;
using Logitude.DashboardModule.MetaDataTool.Models;
using System;
using System.Dynamic;
using System.IO;
using System.Windows;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using Path = System.IO.Path;

namespace Logitude.DashboardModule.MetaDataTool
{
    /// <summary>
    /// Interaction logic for LoadFileWindow.xaml
    /// </summary>
    public partial class LoadFileWindow : Window
    {
        public LoadFileWindow()
        {
            InitializeComponent();
        }

        private void btnLoadLxmlFile_Click(object sender, RoutedEventArgs e)
        {
            string projectPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            DirectoryInfo solutionDir = Directory.GetParent(projectPath);

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".axml";
            dlg.Filter = "AXML files (*.axml)|*.axml|All files (*.*)|*.*";
            dlg.InitialDirectory = solutionDir.FullName + @"\Infrastructure.MetaData\EntityFiles"; ;

            bool? result = dlg.ShowDialog();

            if (result == false) return;
            try
            {
                FileStream stream = new FileStream(dlg.FileName, FileMode.Open);
                App.DirectOpenPath = dlg.FileName;
                XmlDocument document = new XmlDocument();
                document.Load(stream);
                AnalyticsFactsMetaDataViewModel model = XMLParser.Deserialize<AnalyticsFactsMetaDataViewModel>(document.OuterXml);
                OpenTabeWindow(model);
            }
            catch (Exception)
            {
                OpenTabeWindow(new AnalyticsFactsMetaDataViewModel());
            }

        }

        private void OpenTabeWindow(AnalyticsFactsMetaDataViewModel model)
        {
            AnalyticsFactsMetaDataWindow analyticsFactsMetaDataWindow = new AnalyticsFactsMetaDataWindow();
            analyticsFactsMetaDataWindow.DataContext = model;
            analyticsFactsMetaDataWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            analyticsFactsMetaDataWindow.WindowState = WindowState.Maximized;
            Application.Current.MainWindow = analyticsFactsMetaDataWindow;
            Close();
            analyticsFactsMetaDataWindow.Show();
        }
    }
}
