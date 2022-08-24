using Logitude.DashboardModule.MetaDataTool.Helpers;
using Logitude.DashboardModule.MetaDataTool.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;

namespace Logitude.DashboardModule.MetaDataTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string DirectOpenPath { get; set; }
        public static List<string> LXMLFilesPaths { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args == null || e.Args.Length <= 0)
            {
                LoadFileWindow loadFileWindow = new LoadFileWindow();
                loadFileWindow.Show();
                base.OnStartup(e);
                return;
            }

            DirectOpenPath = e.Args[0].ToString();
            if (string.IsNullOrEmpty(DirectOpenPath))
            {
                return;
            }

            FileStream stream;
            try
            {
                stream = new FileStream(DirectOpenPath, FileMode.Open);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                base.OnStartup(e);
                return;
            }

            XmlDocument document = new XmlDocument();
            document.Load(stream);
            AnalyticsFactsMetaDataViewModel analyticsFactsMetaDataView = XMLParser.Deserialize<AnalyticsFactsMetaDataViewModel>(document.OuterXml);
            AnalyticsFactsMetaDataWindow objectTableWindow = new AnalyticsFactsMetaDataWindow();
            objectTableWindow.DataContext = analyticsFactsMetaDataView;
            objectTableWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            objectTableWindow.WindowState = WindowState.Maximized;
            objectTableWindow.Show();

            stream.Close();
            stream.Dispose();

            LXMLFilesPaths = new List<string>();
            GetLXMLAndDXMLFilesPaths();


        }

        public void GetLXMLAndDXMLFilesPaths()
        {
            try
            {
                string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\";
                LXMLFilesPaths = Directory.GetFiles(projectDirectory, "*.axml", SearchOption.AllDirectories).ToList();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error While Loading Files: " + exception.Message);
            }
        }
    }
}
