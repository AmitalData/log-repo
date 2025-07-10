using DW_Editor_Tool.Helpers;
using DW_Editor_Tool.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DW_Editor_Tool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string CurrentFilePath { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainView = new MainWindow();

            try
            {
                if (e.Args != null && e.Args.Length > 0)
                {
                    CurrentFilePath = e.Args[0].ToString();
                    if (!string.IsNullOrEmpty(CurrentFilePath))
                    {
                        if (App.CurrentFilePath.Contains(".dwml"))
                        {
                            DWObjectTableViewModel viewModel = XmlGenerator.GetDWViewModelFromFile();
                            if (viewModel != null)
                                mainView.DataContext = viewModel;
                        }
                        else
                            MessageBox.Show("Invalid file format!");
                    }

                }
                else
                {

                    DWObjectTableViewModel viewModel = new DWObjectTableViewModel();
                    mainView.DataContext = viewModel;


                }

                mainView.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug(ex);
            }

            base.OnStartup(e);
        }
    }
}
