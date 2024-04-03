using GalaSoft.MvvmLight.Command;
using Logitude.DashboardModule.MetaDataTool.Helpers;
using Logitude.DashboardModule.MetaDataTool.Models;
using Logitude.DashboardModule.MetaDataTool.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Logitude.DashboardModule.MetaDataTool.Models
{
    public class AnalyticsFactsMetaDataViewModel : AnalyticsFactsMetaData
    {
        public ObservableCollection<AnalyticsFactsFieldsMetaDataViewModel> TempFields { get; set; } = new ObservableCollection<AnalyticsFactsFieldsMetaDataViewModel>();
        public ObservableCollection<AnalyticsFactsFieldsMetaDataViewModel> AnalyticsFactsFieldsMetaDataViewModels { get; set; } = new ObservableCollection<AnalyticsFactsFieldsMetaDataViewModel>();

        AnalyticsFactsFieldsMetaDataViewModel selectedObjectField;

        public AnalyticsFactsFieldsMetaDataViewModel SelectedObjectField
        {
            get { return selectedObjectField; }
            set
            {
                selectedObjectField = value;
                FirePropertyChanged("SelectedObjectField");
            }
        }



        Visibility errorsVisibility = Visibility.Collapsed;
        public Visibility ErrorsVisibility
        {
            get { return errorsVisibility; }
            set { errorsVisibility = value; FirePropertyChanged("ErrorsVisibility"); }
        }

        Visibility fieldsEditControlVisibility;
        public Visibility FieldsEditControlVisibility
        {
            get { return fieldsEditControlVisibility; }
            set { fieldsEditControlVisibility = value; FirePropertyChanged("FieldsEditControlVisibility"); }
        }
        public RelayCommand<Window> SaveBtnCommand
        {
            get { return new RelayCommand<Window>(window => Save(window)); }
        }

        public RelayCommand<Window> CancelBtnCommand
        {
            get { return new RelayCommand<Window>(window => Cancel(window)); }
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

        internal void Save(Window window)
        {
            ValidateTable();

            if (!string.IsNullOrEmpty(ErrorMessages))
            {
                this.ErrorsVisibility = Visibility.Visible;
                return;
            }

            JsonHelper.GenerateJsonFileFromTool(this);
            //DxmlGeneratorClass.GenerateDXMLFileFromTool(this);
            window.Close();
            Application.Current.Shutdown();
        }

        private void ValidateTable()
        {
            ErrorMessages = "";
            StringBuilder str = new StringBuilder();

            if (string.IsNullOrEmpty(this.TableName)) str.AppendLine("Name is required!");
            if (string.IsNullOrEmpty(this.Name)) str.AppendLine("Table Name is required!");
            if (string.IsNullOrEmpty(this.ObjectTableName)) str.AppendLine("Object Table Name is required!");
            ValidateCommonFilterCode(str);
            foreach (var field in this.AnalyticsFactsFieldsMetaDataViewModels) field.Validate(str);


            ErrorMessages = str.ToString();
        }

        private void ValidateCommonFilterCode(StringBuilder str)
        {
            if (AnalyticsFactsFieldsMetaDataViewModels == null || AnalyticsFactsFieldsMetaDataViewModels.Count == 0) return;
            var duplicates = AnalyticsFactsFieldsMetaDataViewModels.Where(x=>x.CommonFilterCode != null && x.CommonFilterCode != "").GroupBy(x => x.CommonFilterCode).Where(x => x.Count() > 1).Select(x => x.Key).ToList();

            foreach (var item in duplicates)
            {
                str.AppendLine("Duplicate CommonFilter Code :" + item);
            }
        }

        internal void BuildObsList()
        {
            TempFields.Clear();
            AnalyticsFactsFieldsMetaDataViewModels.Clear();
            foreach (var item in AnalyticsFactsFieldsMetaDatas)
            {
                var analyticsFactsFieldsMetaDataViewModel = JsonConvert.DeserializeObject<AnalyticsFactsFieldsMetaDataViewModel>(JsonConvert.SerializeObject(item));
                TempFields.Add(analyticsFactsFieldsMetaDataViewModel);
                AnalyticsFactsFieldsMetaDataViewModels.Add(analyticsFactsFieldsMetaDataViewModel);
            }
            this.SelectedObjectField = TempFields.FirstOrDefault();
            FieldsEditControlVisibility = (AnalyticsFactsFieldsMetaDataViewModels.Count == 0) ? Visibility.Collapsed : Visibility.Visible;
        }

        internal void AddNewField(AnalyticsFactsFieldsMetaDataViewModel analyticsFactsFieldsMetaDataViewModel)
        {
            AnalyticsFactsFieldsMetaDataViewModels.Add(analyticsFactsFieldsMetaDataViewModel);
            TempFields.Add(analyticsFactsFieldsMetaDataViewModel);

            FieldsEditControlVisibility = (AnalyticsFactsFieldsMetaDataViewModels.Count == 0) ? Visibility.Collapsed : Visibility.Visible;

            this.SelectedObjectField = analyticsFactsFieldsMetaDataViewModel;
        }

        internal void Cancel(Window window)
        {
            window.Close();
            System.Windows.Application.Current.Shutdown();
        }

        public RelayCommand AddFieldCommand
        {
            get { return new RelayCommand(() => this.OnAddCLick()); }
        }

        private void OnAddCLick()
        {
            AnalyticsFactsFieldsMetaDataViewModel model = new AnalyticsFactsFieldsMetaDataViewModel(this, true);
            AnalyticsFactsFieldsMetaDataPage analyticsFactsFieldsMetaDataPage = new AnalyticsFactsFieldsMetaDataPage();
            analyticsFactsFieldsMetaDataPage.DataContext = model;

            var newWindow = new Window();
            newWindow.Width = 1200;
            newWindow.Height = 550;
            newWindow.Content = analyticsFactsFieldsMetaDataPage;
            newWindow.Show();
        }

        public RelayCommand<AnalyticsFactsFieldsMetaDataViewModel> RemoveFieldCommand
        {
            get { return new RelayCommand<AnalyticsFactsFieldsMetaDataViewModel>(m => this.RemoveFieldMethod(m)); }
        }

        public void RemoveFieldMethod(AnalyticsFactsFieldsMetaDataViewModel selected)
        {
            if (selected == null) return;

            this.AnalyticsFactsFieldsMetaDataViewModels.Remove(selected);
            this.TempFields.Remove(selected);
            FieldsEditControlVisibility = (AnalyticsFactsFieldsMetaDataViewModels.Count == 0) ? Visibility.Collapsed : Visibility.Visible;
            FirePropertyChanged("DWObjectFieldsList");
        }

        public RelayCommand<string> ObjectFieldsFilterTextChanged
        {
            get { return new RelayCommand<string>(i => this.ObjectFieldsFilterTextChangedMethod(i)); }
        }

        private void ObjectFieldsFilterTextChangedMethod(string filter)
        {
            TempFields.Clear();
            var temp = AnalyticsFactsFieldsMetaDataViewModels.Where(a => a.FieldCode.ToLower().Contains(filter.ToLower())).ToList();
            foreach (var item in temp)
            {
                if (SelectedObjectField != null)
                {
                    SelectedObjectField.ErrorsVisibility = Visibility.Collapsed;
                }

                item.ErrorsVisibility = Visibility.Collapsed;
                TempFields.Add(item);
            }
        }

    }
}
