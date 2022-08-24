using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Xml.Serialization;
using GalaSoft.MvvmLight.Command;
using Logitude.DashboardModule.MetaDataTool.Helpers;

namespace Logitude.DashboardModule.MetaDataTool.Models
{
    public class AnalyticsFactsMetaData : PropertyChangedImplementation
    {
        public string HashString { get; set; } = Guid.NewGuid().ToString("N");

        public AnalyticsFactsMetaData()
        {
            AnalyticsFactsFieldsMetaDatas = new List<AnalyticsFactsFieldsMetaDataViewModel>();
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value; FirePropertyChanged("Name");
            }
        }

        private string tableName;
        public string TableName
        {
            get { return tableName; }
            set
            {
                tableName = value; FirePropertyChanged("TableName");
            }
        }

        public List<AnalyticsFactsFieldsMetaDataViewModel> AnalyticsFactsFieldsMetaDatas { get; set; }

    }
}
