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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Logitude.DashboardModule.MetaDataTool.Views
{
    /// <summary>
    /// Interaction logic for AnalyticsFactsFieldsMetaDataPage.xaml
    /// </summary>
    public partial class AnalyticsFactsFieldsMetaDataPage : UserControl
    {
        public AnalyticsFactsFieldsMetaDataPage()
        {
            InitializeComponent();
            LayoutUpdated += OUpdate;
            
        }

        private void OUpdate(object sender, EventArgs e)
        {
            if (mainGrid != null && DataContext == null) mainGrid.Visibility = Visibility.Hidden;
            else if (mainGrid != null && DataContext != null) mainGrid.Visibility = Visibility.Visible;
        }

    }
}
