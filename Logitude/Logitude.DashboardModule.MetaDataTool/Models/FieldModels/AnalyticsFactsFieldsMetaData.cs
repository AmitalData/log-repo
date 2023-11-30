using Logitude.DashboardModule.MetaDataTool.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.MetaDataTool.Models.FieldModels
{
    public class AnalyticsFactsFieldsMetaData : PropertyChangedImplementation
    {
        public string HashString { get; set; } = Guid.NewGuid().ToString("N");


        private string dataTypeCode;
        public string DataTypeCode
        {
            get { return dataTypeCode; }
            set
            {
                dataTypeCode = value; FirePropertyChanged("DataTypeCode");
            }
        }


        private bool canMeasure;
        public bool CanMeasure
        {
            get { return canMeasure; }
            set
            {
                canMeasure = value; FirePropertyChanged("CanMeasure");
            }
        }


        private bool canGroup;
        public bool CanGroup
        {
            get { return canGroup; }
            set
            {
                canGroup = value; FirePropertyChanged("CanGroup");
            }
        }

        private bool canSecondaryGroup;
        public bool CanSecondaryGroup
        {
            get { return canSecondaryGroup; }
            set
            {
                canSecondaryGroup = value; FirePropertyChanged("CanSecondaryGroup");
            }
        }


        private string fieldCode;
        public string FieldCode
        {
            get { return fieldCode; }
            set
            {
                fieldCode = value; FirePropertyChanged("FieldCode");
            }
        }


        private string displayName;
        public string DisplayName
        {
            get { return displayName; }
            set
            {
                displayName = value; FirePropertyChanged("DisplayName");
            }
        }


        private string displayNamePlural;
        public string DisplayNamePlural
        {
            get { return displayNamePlural; }
            set
            {
                displayNamePlural = value; FirePropertyChanged("DisplayNamePlural");
            }
        }


        private string joinedTableName;
        public string JoinedTableName
        {
            get { return joinedTableName; }
            set
            {
                joinedTableName = value; FirePropertyChanged("JoinedTableName");
            }
        }

        private string joinedTableDBName;
        public string JoinedTableDBName
        {
            get { return joinedTableDBName; }
            set
            {
                joinedTableDBName = value; FirePropertyChanged("JoinedTableDBName");
            }
        }


        private string joinedTableKey;
        public string JoinedTableKey
        {
            get { return joinedTableKey; }
            set
            {
                joinedTableKey = value; FirePropertyChanged("JoinedTableKey");
            }
        }


        private string joinedTableDisplayField;
        public string JoinedTableDisplayField
        {
            get { return joinedTableDisplayField; }
            set
            {
                joinedTableDisplayField = value; FirePropertyChanged("JoinedTableDisplayField");
            }
        }

        private bool hasUnit;
        public bool HasUnit
        {
            get { return hasUnit; }
            set
            {
                hasUnit = value; FirePropertyChanged("HasUnit");
            }
        }

        private string unit;
        public string Unit
        {
            get { return unit; }
            set
            {
                unit = value; FirePropertyChanged("Unit");
            }
        }

        private string commonFilterCode;
        public string CommonFilterCode
        {
            get { return commonFilterCode; }
            set
            {
                commonFilterCode = value; FirePropertyChanged("CommonFilterCode");
            }
        }

        private bool allowTenantZeroFilter;
        public bool AllowTenantZeroFilter
        {
            get { return allowTenantZeroFilter; }
            set
            {
                allowTenantZeroFilter = value; FirePropertyChanged("AllowTenantZeroFilter");
            }
        }

    }
}
