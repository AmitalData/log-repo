using System;
using System.Collections.Generic;
using Logitude.DashboardModule.MetaDataTool.Helpers;
using Logitude.DashboardModule.MetaDataTool.Models.FieldModels;

namespace Logitude.DashboardModule.MetaDataTool.Models
{
    public class AnalyticsFactsMetaData : PropertyChangedImplementation
    {
        public string HashString { get; set; } = Guid.NewGuid().ToString("N");

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

        private string objectTableName;
        public string ObjectTableName
        {
            get { return objectTableName; }
            set
            {
                objectTableName = value; FirePropertyChanged("ObjectTableName");
            }
        }

        public List<AnalyticsFactsFieldsMetaData> AnalyticsFactsFieldsMetaDatas { get; set; }
    }
}
