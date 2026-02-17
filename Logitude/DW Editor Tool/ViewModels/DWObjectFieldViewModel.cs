using DW_Editor_Tool.Helpers;
using DW_Editor_Tool.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DW_Editor_Tool.ViewModels
{
    public class DWObjectFieldViewModel : PropertyChangedImplementation
    {
        public string Id { get; set; }

        private string code;
        public string Code { get { return code; } set { code = value; FirePropertyChanged("Code"); } }

        private string name;
        public string Name { get { return name; } set { name = value; FirePropertyChanged("Name"); if (string.IsNullOrEmpty(this.Code)) this.Code = "["+value+"]"; } }

        private string dwObjectTableCode;
        public string DwObjectTableCode { get { return dwObjectTableCode; } set { dwObjectTableCode = value; FirePropertyChanged("DwObjectTableCode"); } }

        private string dataTypeCode;
        public string DataTypeCode
        {
            get { return dataTypeCode; }
            set
            {
                dataTypeCode = value; FirePropertyChanged("DataTypeCode");
                TextTypeVisibility = ((value == "Text" || value == "nText" || value == "Dimension") ? Visibility.Visible : Visibility.Collapsed);
                DimensionTabeVisisbilty = ((value == "Dimension") ? Visibility.Visible : Visibility.Collapsed);

                if(dataTypeCode!= "Dimension")
                {
                    DimensionTableCode = null;
                }

            }
        }

        private string dimensionTableCode;
        public string DimensionTableCode
        {
            get {

                return dimensionTableCode;

            }

            set
            {
                dimensionTableCode = value;

                FirePropertyChanged("DimensionTableCode");
            }

        }

        private int minLength;
        public int MinLength { get { return minLength; } set { minLength = value; FirePropertyChanged("MinLength"); } }

        private int maxLength;
        public int MaxLength { get { return maxLength; } set { maxLength = value; FirePropertyChanged("MaxLength"); } }

        bool isRequired;
        public bool IsRequired
        {
            get { return isRequired; }
            set { isRequired = value; FirePropertyChanged("IsRequired"); }
        }

        bool isPrimaryKey;
        public bool IsPrimaryKey
        {
            get { return isPrimaryKey; }
            set { isPrimaryKey = value; FirePropertyChanged("IsPrimaryKey"); }
        }

        bool isMeasurement;
        public bool IsMeasurement
        {
            get { return isMeasurement; }
            set { isMeasurement = value; FirePropertyChanged("IsMeasurement"); }
        }

        bool isCustom;
        public bool IsCustom
        {
            get { return isCustom; }
            set { isCustom = value; FirePropertyChanged("IsCustom"); }
        }

        


        string aggregationTypeCode;
        public string AggregationTypeCode
        {
            get { return aggregationTypeCode; }
            set { aggregationTypeCode = value; FirePropertyChanged("AggregationTypeCode"); }
        }

        string category1;
        public string Category1
        {
            get { return category1; }
            set { category1 = value; FirePropertyChanged("Category1"); }
        }

        string category2;
        public string Category2
        {
            get { return category2; }
            set { category2 = value; FirePropertyChanged("Category2"); }
        }

        string lOVAdditionalFields;
        public string LOVAdditionalColumns
        {
            get { return lOVAdditionalFields; }
            set { lOVAdditionalFields = value; FirePropertyChanged("LOVAdditionalColumns"); }
        }

		bool hideTree;
		public bool HideTree
		{
			get { return hideTree; }
			set { hideTree = value; FirePropertyChanged("HideTree"); }
		}

        bool cannotFilter;
        public bool CannotFilter
        {
            get { return cannotFilter; }
            set { cannotFilter = value; FirePropertyChanged("CannotFilter"); }
        }

        string helpText;
        public string HelpText
        {
            get { return helpText; }
            set { helpText = value; FirePropertyChanged("HelpText"); }
        }
        // is Measurement , Aggregation Type

        public List<string> DataTypesList { get { return new List<string>() { "Text", "nText", "Date", "DateTime", "Boolean", "Decimal", "Integer", "Dimension", "SqlVariant" }; } }
        public List<string> AggregationTypesList { get { return new List<string>() { "SUM", "COUNT" }; } }

        Visibility errorsVisibility = Visibility.Collapsed;
        public Visibility ErrorsVisibility
        {
            get { return errorsVisibility; }
            set { errorsVisibility = value; FirePropertyChanged("ErrorsVisibility"); }
        }

        public string errorMessages;
        public string ErrorMessages
        {
            get
            {
                return errorMessages;
            }
            set
            {
                errorMessages = value;
                FirePropertyChanged("ErrorMessages");
            }
        }

        public Visibility buttonsVisibility;
        public Visibility ButtonsVisibility
        {
            get
            {
                return buttonsVisibility;
            }
            set
            {
                buttonsVisibility = value;
                FirePropertyChanged("ButtonsVisibility");
            }
        }

        public Visibility dimensionTabeVisisbilty = Visibility.Collapsed;
        public Visibility DimensionTabeVisisbilty
        {
            get
            {
                return dimensionTabeVisisbilty;
            }
            set
            {
                dimensionTabeVisisbilty = value;
                FirePropertyChanged("DimensionTabeVisisbilty");
            }
        }

        public Visibility textTypeVisibility = Visibility.Collapsed;
        public Visibility TextTypeVisibility
        {
            get
            {
                return textTypeVisibility;
            }
            set
            {
                textTypeVisibility = value;
                FirePropertyChanged("TextTypeVisibility");
            }
        }



        DWObjectTableViewModel dwTableViewModel;
        public DWObjectFieldViewModel(DWObjectTableViewModel tableViewModel, bool isNew)
        {
            this.ButtonsVisibility = (isNew ? Visibility.Visible : Visibility.Collapsed);
            this.dwTableViewModel = tableViewModel;
        }




        public RelayCommand<UserControl> SaveBtnCommand
        {
            get { return new RelayCommand<UserControl>(window => this.Save(window)); }
        }

        public void Save(UserControl control)
        {
            this.ErrorMessages = "";
            StringBuilder str = Validate();

            ErrorMessages = str.ToString();
            if (!string.IsNullOrEmpty(ErrorMessages))
            {
                ErrorsVisibility = Visibility.Visible;
                return;
            }

            //if (XmlGenerator.GenerateXmlToFile(this))
            //{
            //    window.Close();
            //    System.Windows.Application.Current.Shutdown();
            //}


            this.dwTableViewModel.AddNewField(this);
            this.ButtonsVisibility = Visibility.Collapsed;
            ((Window)control.Parent).Close();
        }

        public StringBuilder Validate(StringBuilder str = null)
        {
            if (str == null)
                str = new StringBuilder();

            if (string.IsNullOrEmpty(this.Code))
                str.AppendLine("DW Field Code is required!");

            if (string.IsNullOrEmpty(this.Name))
                str.AppendLine("DW Field Name is required!");

            if (string.IsNullOrEmpty(this.DataTypeCode))
                str.AppendLine("DW Field Data Type Code is required!");

            if ((this.DataTypeCode == "Text" || this.DataTypeCode == "nText") && this.MaxLength == 0)
                str.AppendLine(this.Name + ": Max length is required for text or ntext fields!");

            if (this.IsMeasurement && string.IsNullOrEmpty(this.AggregationTypeCode))
            {
                str.AppendLine("DW Field AggregationTypeCode is required!");
            }
            return str;
        }

        public RelayCommand<UserControl> CancelBtnCommand
        {
            get { return new RelayCommand<UserControl>(control => this.Cancel(control)); }
        }



        public void Cancel(UserControl control)
        {
            ((Window)control.Parent).Close();


        }

        bool displayInQueryBuilder = true;
        public bool DisplayInQueryBuilder
        {
            get { return displayInQueryBuilder; }
            set { displayInQueryBuilder = value; FirePropertyChanged("DisplayInQueryBuilder"); }
        }


    }
}
