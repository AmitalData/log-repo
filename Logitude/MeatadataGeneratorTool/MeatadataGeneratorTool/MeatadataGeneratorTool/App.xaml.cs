using MeatadataGeneratorTool.CloseTablesData;
using MeatadataGeneratorTool.DataContractsModule;
using MeatadataGeneratorTool.EventTypes;
using MeatadataGeneratorTool.Helpers;
using MeatadataGeneratorTool.MenuButtons;
using MeatadataGeneratorTool.QueryModule;
using MeatadataGeneratorTool.ScreensModule;
using MeatadataGeneratorTool.TabsModule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;

namespace MeatadataGeneratorTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string DirectOpenPath { get; set; }
        public static ObjectTableControl CurrentControl { get; set; }
        public static MainWindowControl MainControl { get; set; }
        public static string CurrentDBTableName { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args != null && e.Args.Length > 0)
            {
                //e.Args[0].ToString();//@@"C:\LatestTool\MetaDataGenerator\MetaDataGenerator\Teeeeem.lxml";//
                DirectOpenPath = e.Args[0].ToString();//@"C:\LatestTool\MetaDataGenerator\MetaDataGenerator\Test121.lxml";//"C:\LogitudeWorld - Offline 29-9-2015\main\Logitude.CRM.MetaData\EntityFiles\CallType.lxml";//e.Args[0].ToString(); //@"C:\LogitudeWorld\MetaDataGenerator\MetaDataGenerator\TestClose.lxml";//@"C:\LogitudeWorld\MetaDataGenerator\MetaDataGenerator\testXMLFile1.lxml";//;e.Args[0].ToString(); //@"C:\LogitudeWorld - OffLine 21-9-2015\main\Logitude.CRM.MetaData\EntityFiles\Test1.lxml";//e.Args[0].ToString(); //@"C:\LogitudeWorld\MetaDataGenerator\MetaDataGenerator\TestClose.lxml";//e.Args[0].ToString(); //@"C:\LogitudeWorld - New Offline\main\Logitude.CRM.MetaData\EntityFiles\Activity.lxml";//e.Args[0].ToString(); //@"C:\LogitudeWorld - NewOffline\main\Logitude.CRM.MetaData\EntityFiles\Activity.lxml";//e.Args[0].ToString();//@"C:\LogitudeWorld\MetaDataGenerator\MetaDataGenerator\TestClose.lxml";//e.Args[0].ToString();//@"C:\LogitudeWorld\MetaDataGenerator\MetaDataGenerator\testXMLFile1.lxml";//
                //DirectOpenPath = @"C:\LogitudeWorld\main\Logitude.MetaData\EntityFiles\QuoteModel\Quote.lxml";//e.Args[0].ToString();//@"C:\LatestTool\MetaDataGenerator\MetaDataGenerator\TestClose.lxml";//"C:\LogitudeWorld - Offline 29-9-2015\main\Logitude.CRM.MetaData\EntityFiles\CallType.lxml";//e.Args[0].ToString(); //@"C:\LogitudeWorld\MetaDataGenerator\MetaDataGenerator\TestClose.lxml";//@"C:\LogitudeWorld\MetaDataGenerator\MetaDataGenerator\testXMLFile1.lxml";//;e.Args[0].ToString(); //@"C:\LogitudeWorld - OffLine 21-9-2015\main\Logitude.CRM.MetaData\EntityFiles\Test1.lxml";//e.Args[0].ToString(); //@"C:\LogitudeWorld\MetaDataGenerator\MetaDataGenerator\TestClose.lxml";//e.Args[0].ToString(); //@"C:\LogitudeWorld - New Offline\main\Logitude.CRM.MetaData\EntityFiles\Activity.lxml";//e.Args[0].ToString(); //@"C:\LogitudeWorld - NewOffline\main\Logitude.CRM.MetaData\EntityFiles\Activity.lxml";//e.Args[0].ToString();//@"C:\LogitudeWorld\MetaDataGenerator\MetaDataGenerator\TestClose.lxml";//e.Args[0].ToString();//@"C:\LogitudeWorld\MetaDataGenerator\MetaDataGenerator\testXMLFile1.lxml";//


                if (!string.IsNullOrEmpty(App.DirectOpenPath))
                {
                    //MessageBox.Show(App.DirectOpenPath);


                    // byte[] byteArray = new byte[stream.Length];
                    //stream.Read(byteArray, 0, byteArray.Length);
                    FileStream stream = null;
                    try
                    {
                        stream = new FileStream(App.DirectOpenPath, FileMode.Open);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        base.OnStartup(e);
                        return;
                    }

                    try
                    {

                        XmlDocument document = new XmlDocument();
                        document.Load(stream);
                        XmlParserHelper ParserHelper = new XmlParserHelper();
                        ObjectTableViewModel model = ParserHelper.LoadObjectTableData(document);
                        CurrentControl = new ObjectTableControl();
                        CurrentControl.DataContext = model;

                        //CurrentControl.WindowStyle = WindowStyle.None;
                        CurrentControl.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        CurrentControl.WindowState = WindowState.Maximized;

                        CurrentControl.Show();




                        CurrentControl.Closed += CurrentControl_Closed;
                        stream.Close();
                    }

                    catch (Exception err)
                    {
                        //MessageBox.Show(err.Message);
                        if (DirectOpenPath.Contains(".lxml"))
                        {
                            //MessageBox.Show(err.Message + Environment.NewLine + err.StackTrace);
                            var FileName = Path.GetFileName(DirectOpenPath).Replace(".lxml", "");
                            MainWindowViewModel model = new MainWindowViewModel();
                            model.ObjectTableName = FileName;
                            MainControl = new MainWindowControl();
                            MainControl.DataContext = model;
                            MainControl.Show();
                            MainControl.Closed += MainControl_Closed;
                        }
                        else
                        {
                            MessageBox.Show(err.Message);
                        }
                    };
                    //XmlElement element = new XmlElement();


                    //XElement el = XElement.Parse(fileContent);
                    // MessageBox.Show(fileContent);
                }
            }
            else
            {
                LoadFileWindow loadFileWindow = new LoadFileWindow();
                loadFileWindow.Show();
            }
            base.OnStartup(e);
        }

        private void MainControl_Closed(object sender, EventArgs e)
        {
            MainControl.Close();
        }

        void CurrentControl_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

    }
}
