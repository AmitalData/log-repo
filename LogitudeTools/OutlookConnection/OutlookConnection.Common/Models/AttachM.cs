using OutlookConnection.Common.DocumentTypeWcfServiceReference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OutlookConnection.Common.Models
{
    public class AttachM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
                //handler(this, new PropertyChangedEventArgs("SaveCommandCanExecute"));
            }
            //SaveCommandCanExecute = HasError();
        }

        public byte[] Inner { get; set; }
        public bool _IsEditable;
        private bool _IsSelected;
        private string _Note { get; set; }
        private string _DocType;
        public bool IsEmail { get; set; }
        public int index { get; set; }
        public int Size { get; set; }
        public string Extension { get; set; }
        public string Pic { get; set; }
        string _Name;
        public string _DocTypeName { get; set; }


        public string Note
        {
            get { return _Note; }
            set { _Note = value; RaisePropertyChanged("Note"); }
        }


        public string DocType
        {
            get { return _DocType; }
            set 
            { 
                _DocType = value;
                RaisePropertyChanged("DocType");
            }
        }

        public string DocTypeName
        {
            get { return _DocTypeName; }
            set
            {
                _DocTypeName = value;
                RaisePropertyChanged("DocTypeName");
            }
        }

        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name == value)
                {
                    if (IsEditable)
                    {
                        IsEditable = false;
                    }
                    return;
                }
                _Name = value;
                RaisePropertyChanged("Name");
                if (IsEditable)
                {
                    IsEditable = false;
                }
            }
        }


        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;


                    RaisePropertyChanged("IsSelected");
                }
            }

        }

        public bool IsEditable
        {
            get { return _IsEditable; }
            set
            {
                if (_IsEditable != value)
                {
                    _IsEditable = value;


                    RaisePropertyChanged("IsEditable");
                }
            }

        }
        private Visibility selectedItemVisibility = Visibility.Collapsed;
        public Visibility SelectedItemVisibility
        {
            get
            {
                return selectedItemVisibility;
            }
            set
            {
                selectedItemVisibility = value;
                this.RaisePropertyChanged("SelectedItemVisibility");
            }
        }

        private Visibility listsVisibility = Visibility.Visible;
        public Visibility ListsVisibility
        {
            get
            {
                return listsVisibility;
            }
            set
            {
                listsVisibility = value;
                this.RaisePropertyChanged("ListsVisibility");
            }
        }

        public DocumentTypeList _DocumentType;
        public DocumentTypeList DocumentType
        {
            get { return _DocumentType; }
            set
            { 
                _DocumentType = value;
                if (_DocumentType != null)
                {
                    DocTypeName = _DocumentType.Name;
                }
                else
                {
                    DocTypeName = "";
                }
                IsListOpened = false;
                RaisePropertyChanged("DocumentType");
            }
        }

        //private ObservableCollection<DocumentTypeList> _DocTypeList;
        //public ObservableCollection<DocumentTypeList> DocTypeList
        //{
        //    get { return _DocTypeList; }
        //    set { _DocTypeList = value; RaisePropertyChanged("DocTypeList"); }
        //}

        public object DataContext { get; set; }


        public bool _IsListOpened;
        public bool IsListOpened
        {
            get { return _IsListOpened; }
            set
            {
                _IsListOpened = value;
                RaisePropertyChanged("IsListOpened");
            }
        }

        /// <summary>
        /// //
        // Summary:
        //     Saves the attachment to the specified path.
        //
        // Parameters:
        //   Path:
        //     The location at which to save the attachment.
        //void SaveAsFile(string Path);
        /// </summary>
        //public dynamic  RealAttach { get; set; }


    }
}
