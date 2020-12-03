using DW_Editor_Tool.Helpers;
using DW_Editor_Tool.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DW_Editor_Tool.ViewModels
{
    public class DWObjectTableViewModel : PropertyChangedImplementation
    {
        public string Id { get; set; }

        private string code;
        public string Code { get { return code; } set { code = value; FirePropertyChanged("Code"); if (string.IsNullOrEmpty(this.Name)) this.Name = value; } }

        private string name;
        public string Name { get { return name; } set { name = value; FirePropertyChanged("Name"); } }

        private string typeCode;
        public string TypeCode { get { return typeCode; } set { typeCode = value; IsFactTable = ((value == "Fact") ? Visibility.Visible : Visibility.Collapsed); FirePropertyChanged("TypeCode"); } }
        
        private string pivotFieldCode;
        public string PivotFieldCode { get { return pivotFieldCode; } set { pivotFieldCode = value; FirePropertyChanged("PivotFieldCode"); } }


        private string additionalFactCode;
        public string AdditionalFactCode { get { return additionalFactCode; } set { additionalFactCode = value; FirePropertyChanged("AdditionalFactCode"); } }

        private string additionalFactForeignKey;
        public string AdditionalFactForeignKey { get { return additionalFactForeignKey; } set { additionalFactForeignKey = value; FirePropertyChanged("AdditionalFactForeignKey"); } }


        private string parentFactCode;
        public string ParentFactCode { get { return parentFactCode; } set { parentFactCode = value; FirePropertyChanged("ParentFactCode"); } }


        private string recordType;
        public string RecordType { get { return recordType; } set { recordType = value; FirePropertyChanged("RecordType"); } }


        private string displayName;
        public string DisplayName { get { return displayName; } set { displayName = value; FirePropertyChanged("DisplayName"); } }



        bool isClosed;
        public bool IsClosed
        {
            get { return isClosed; }
            set { isClosed = value; FirePropertyChanged("IsClosed"); }
        }

        bool hasPivotColumn;
        public bool HasPivotColumn
        {
            get { return hasPivotColumn; }
            set { hasPivotColumn = value; PivotFieldCodeVisisbilty = (value ? Visibility.Visible : Visibility.Collapsed); FirePropertyChanged("HasPivotColumn"); }
        }

        private string defaultFilterBy;
        public string DefaultFilterBy { get { return defaultFilterBy; } set { defaultFilterBy = value; FirePropertyChanged("DefaultFilterBy"); } }

        private string dataViewName;
        public string DataViewName { get { return dataViewName; } set { dataViewName = value; FirePropertyChanged("DataViewName"); } }

        public List<DWTableType> DWTableTypes { get { return new List<DWTableType> { new DWTableType("Fact", "Fact"), new DWTableType("Dimension", "Dimension") }; } }

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

        Visibility fieldsEditControlVisibility;

        public Visibility FieldsEditControlVisibility
        {
            get { return fieldsEditControlVisibility; }
            set { fieldsEditControlVisibility = value; FirePropertyChanged("FieldsEditControlVisibility"); }
        }

        public ObservableCollection<DWObjectFieldViewModel> DWObjectFieldsList { get; set; } = new ObservableCollection<DWObjectFieldViewModel>();
        public ObservableCollection<DWObjectFieldViewModel> TempDWObjectFieldsList { get; set; } = new ObservableCollection<DWObjectFieldViewModel>();

        DWObjectFieldViewModel selectedObjectField;

        public DWObjectFieldViewModel SelectedObjectField
        {
            get { return selectedObjectField; }
            set
            {
                selectedObjectField = value;
                // FieldLength = value.Length;
                FirePropertyChanged("SelectedObjectField");
            }
        }

        public Visibility isFactTable = Visibility.Collapsed;
        public Visibility IsFactTable
        {
            get
            {
                return isFactTable;
            }
            set
            {
                isFactTable = value;
                FirePropertyChanged("IsFactTable");
            }
        }

        public Visibility pivotFieldCodeVisisbilty = Visibility.Collapsed;
        public Visibility PivotFieldCodeVisisbilty
        {
            get
            {
                return pivotFieldCodeVisisbilty;
            }
            set
            {
                pivotFieldCodeVisisbilty = value;
                FirePropertyChanged("PivotFieldCodeVisisbilty");
            }
        }

        public DWObjectTableViewModel()
        {
            FieldsEditControlVisibility = (DWObjectFieldsList.Count == 0) ? Visibility.Collapsed : Visibility.Visible;
        }


        public RelayCommand AddFieldCommand
        {
            get { return new RelayCommand(() => this.AddFieldMethod()); }
        }
        public Window newWindow = new Window();
        private void AddFieldMethod()
        {
            DWObjectFieldViewModel model = new DWObjectFieldViewModel(this, true);
            DWObjectFieldView dWObjectFieldView = new DWObjectFieldView();
            dWObjectFieldView.DataContext = model;

            newWindow = new Window();
            newWindow.Width = 1200;
            newWindow.Height = 550;
            newWindow.Content = dWObjectFieldView;
            newWindow.Show();

        }

        public RelayCommand<DWObjectFieldViewModel> RemoveFieldCommand
        {
            get { return new RelayCommand<DWObjectFieldViewModel>(m => this.RemoveFieldMethod(m)); }
        }

        public void RemoveFieldMethod(DWObjectFieldViewModel selected)
        {
            if (selected != null)
            {
                this.DWObjectFieldsList.Remove(selected);
                this.TempDWObjectFieldsList.Remove(selected);
                FieldsEditControlVisibility = (DWObjectFieldsList.Count == 0) ? Visibility.Collapsed : Visibility.Visible;
                FirePropertyChanged("DWObjectFieldsList");
            }
        }

        public void BuildObsList(List<DWObjectFieldViewModel> fields)
        {
            DWObjectFieldsList.Clear();
            TempDWObjectFieldsList.Clear();
            foreach (DWObjectFieldViewModel item in fields)
            {
                DWObjectFieldsList.Add(item);
                TempDWObjectFieldsList.Add(item);
            }

            this.SelectedObjectField = DWObjectFieldsList.FirstOrDefault();
            FieldsEditControlVisibility = (DWObjectFieldsList.Count == 0) ? Visibility.Collapsed : Visibility.Visible;
        }

        public void AddNewField(DWObjectFieldViewModel item)
        {
            DWObjectFieldsList.Add(item);
            TempDWObjectFieldsList.Add(item);

            FieldsEditControlVisibility = (DWObjectFieldsList.Count == 0) ? Visibility.Collapsed : Visibility.Visible;

            this.SelectedObjectField = item;
        }


        private void ObjectFieldsFilterTextChangedMethod(string filter)
        {
            TempDWObjectFieldsList.Clear();
            var temp = DWObjectFieldsList.Where(a => a.Name.ToLower().Contains(filter.ToLower())).ToList();
            foreach (var item in temp)
            {
                if (SelectedObjectField != null)
                {
                    SelectedObjectField.ErrorsVisibility = Visibility.Collapsed;
                }

                item.ErrorsVisibility = Visibility.Collapsed;
                TempDWObjectFieldsList.Add(item);
            }
        }
        public RelayCommand<string> ObjectFieldsFilterTextChanged
        {
            get { return new RelayCommand<string>(i => this.ObjectFieldsFilterTextChangedMethod(i)); }

        }

        public RelayCommand<Window> SaveBtnCommand
        {
            get { return new RelayCommand<Window>(window => this.Save(window)); }
        }

        public void Save(Window window)
        {
            this.ErrorMessages = "";
            StringBuilder str = new StringBuilder();
            if (string.IsNullOrEmpty(this.Code))
                str.AppendLine("DW Table Code is required!");

            if (string.IsNullOrEmpty(this.Name))
                str.AppendLine("DW Table Name is required!");

            if (string.IsNullOrEmpty(this.TypeCode))
                str.AppendLine("DW Table TypeCode is required!");

            foreach (DWObjectFieldViewModel field in this.DWObjectFieldsList)
                field.Validate(str);

            ErrorMessages = str.ToString();
            if (!string.IsNullOrEmpty(ErrorMessages))
            {
                ErrorsVisibility = Visibility.Visible;
                return;
            }

            if (XmlGenerator.GenerateXmlToFile(this))
            {
                window.Close();
                System.Windows.Application.Current.Shutdown();
            }
        }

        public RelayCommand<Window> CancelBtnCommand
        {
            get { return new RelayCommand<Window>(window => this.Cancel(window)); }
        }

        public void Cancel(Window window)
        {
            window.Close();
            System.Windows.Application.Current.Shutdown();
        }

    }

    public class DWTableType
    {

        public DWTableType(string code, string name)
        {
            this.Code = code;
            this.Name = name;
        }

        public string Code { get; set; }
        public string Name { get; set; }
    }
}
