using Logitude.DashboardModule.MetaDataTool.Helpers;
using Logitude.DashboardModule.MetaDataTool.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace Logitude.DashboardModule.MetaDataTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();
        public static string DirectOpenPath { get; set; }
        public static List<string> LJSONFilesPaths { get; set; }
        public static List<string> DXMLFilesPaths { get; set; }

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
            AnalyticsFactsMetaDataWindow objectTableWindow = new AnalyticsFactsMetaDataWindow();
            objectTableWindow.DataContext = JsonHelper.GetAnalyticsFactsMetaDataViewModel(DirectOpenPath);
            objectTableWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            objectTableWindow.WindowState = WindowState.Maximized;
            objectTableWindow.Show();

            LJSONFilesPaths = new List<string>();

            worker.DoWork += GetLJSONAndDXMLFilesPaths;
            worker.RunWorkerAsync();
        }

        private void GetLJSONAndDXMLFilesPaths(object sender, DoWorkEventArgs e)
        {
            try
            {
                LJSONFilesPaths = new List<string>();
                DXMLFilesPaths = new List<string>();

                string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\";
                if (projectDirectory.Contains(@"\Logitude\"))
                {
                    string logitudePath = projectDirectory.Split(new string[] { @"\Logitude\" }, StringSplitOptions.None)[0];
                    LJSONFilesPaths = Directory.GetFiles(logitudePath + @"\Logitude\", "*.ljson", SearchOption.AllDirectories).Where(l => !l.ToLower().Contains("logitudefrontend")).ToList();
                    DXMLFilesPaths = Directory.GetFiles(logitudePath + @"\Logitude\", "*.dxml", SearchOption.AllDirectories).ToList();
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show("Error While Loading Files: " + exception.Message);
            }
        }

   
    }
}
