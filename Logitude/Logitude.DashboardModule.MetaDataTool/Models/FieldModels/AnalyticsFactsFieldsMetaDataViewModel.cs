using GalaSoft.MvvmLight.Command;
using Logitude.DashboardModule.MetaDataTool.Models.FieldModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Logitude.DashboardModule.MetaDataTool.Models
{
    public class AnalyticsFactsFieldsMetaDataViewModel : AnalyticsFactsFieldsMetaData
    {
        private AnalyticsFactsMetaDataViewModel analyticsFactsMetaDataViewModel;


        public AnalyticsFactsFieldsMetaDataViewModel()
        {
            this.ButtonsVisibility = Visibility.Collapsed;
        }

        public AnalyticsFactsFieldsMetaDataViewModel(AnalyticsFactsMetaDataViewModel analyticsFactsMetaDataViewModel, bool isNew)
        {
            this.analyticsFactsMetaDataViewModel = analyticsFactsMetaDataViewModel;
            this.ButtonsVisibility = isNew ? Visibility.Visible : Visibility.Collapsed;
        }

        public List<string> DataTypesList { get { return new List<string>() { "Text", "nText", "Date", "DateTime", "Boolean", "Decimal", "Integer", "SqlVariant", "LookUp" }; } }


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
            this.analyticsFactsMetaDataViewModel.AddNewField(this);
            this.ButtonsVisibility = Visibility.Collapsed;
            ((Window)control.Parent).Close();
        }

        public StringBuilder Validate(StringBuilder str = null)
        {
            if (str == null) str = new StringBuilder();

            if (string.IsNullOrEmpty(this.FieldCode)) str.AppendLine("Field Code is required!");

            if (string.IsNullOrEmpty(this.DisplayName)) str.AppendLine("Display Name is required!");

            if (string.IsNullOrEmpty(this.DisplayNamePlural)) str.AppendLine("Display Name Plural is required!");

            if (string.IsNullOrEmpty(this.DataTypeCode)) str.AppendLine("Data Type Code is required!");

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
    }
}
