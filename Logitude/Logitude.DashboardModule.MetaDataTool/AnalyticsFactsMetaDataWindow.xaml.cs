using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Logitude.DashboardModule.MetaDataTool
{
    /// <summary>
    /// Interaction logic for AnalyticsFactsMetaDataWindow.xaml
    /// </summary>
    public partial class AnalyticsFactsMetaDataWindow : Window
    {
        public AnalyticsFactsMetaDataWindow()
        {
            InitializeComponent();
            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            this.Closed -= MainWindow_Closed;
            System.Windows.Application.Current.Shutdown();
        }
    }
}
